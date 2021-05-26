using MediatR;
using Microsoft.Extensions.Logging;
using MRK.Emission.DataAccess.Repositories;
using MRK.Emission.Domain.Enums;
using MRK.Emission.Domain.Models;
using MRK.Emission.Service.SUZ;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace MRK.Emission.Business.Emission.Commands.ProceedOrderDocuments
{
    public class ProceedOrderDocumentsCommandHandler : IRequestHandler<ProceedOrderDocumentsCommand, ProceedOrderDocumentResponse>
    {
        private readonly IEmissionRepository _repository;
        private readonly ISuzService _suz;
        private readonly ILogger<ProceedOrderDocumentsCommandHandler> _logger;

        public ProceedOrderDocumentsCommandHandler(IEmissionRepository repository,
            ISuzService suz,
            ILogger<ProceedOrderDocumentsCommandHandler> logger)
        {
            _repository = repository;
            _suz = suz;
            _logger = logger;
        }

        public async Task<ProceedOrderDocumentResponse> Handle(ProceedOrderDocumentsCommand request, CancellationToken cancellationToken)
        {
            var response = new ProceedOrderDocumentResponse();  

            if(!_suz.Auth())
            {
                response.ErrorMessage = $"Unable to get access token!";
                return response;
            }

            if (!await _suz.PingAsync("ProceedOrderDocumentsCommand", cancellationToken))
            {
                response.ErrorMessage = $"SUZ host is unreachable!";
                return response;
            }

            Expression<Func<OrderDocument, bool>> expression = od =>
                od.documentStatus.Equals(OrderDocumentStatus.CREATED) ||
                od.documentStatus.Equals(OrderDocumentStatus.POSTED);

            var docs = await _repository.GetOrderDocumentsAsync(expression, cancellationToken);

            foreach (OrderDocument d in docs)
            {
                Expression<Func<OrderDocumentLine, bool>> exp = odl =>
                    odl.documentId.Equals(d.documentId) && odl.clientName.Equals(d.clientName);

                d.documentLines = await _repository.GetOrderDocumentLinesAsync(exp, cancellationToken);

                switch (d.documentStatus)
                {
                    case OrderDocumentStatus.CREATED:
                        var doc = await SendDoc(d, cancellationToken);
                        await _repository.UpdateAsync(doc, cancellationToken);
                        break;
                    case OrderDocumentStatus.POSTED:
                        bool ok = true;
                        //Проверить, что все строки выполнены
                        d.documentLines?.ForEach(dl =>
                        {
                            if (ok && (dl.documentLineStatus != DocumentLineStatus.COMPLITED && dl.documentLineStatus != DocumentLineStatus.ERROR))
                                ok = false;
                        });
                        //Если ок, перевести статус документа
                        if (ok)
                        {
                            d.documentStatus = OrderDocumentStatus.COMPLITED;
                            await _repository.UpdateAsync(d, cancellationToken);


                        }
                        break;
                }
            }

            return response;
        }

        private async Task<OrderDocument> SendDoc(OrderDocument document, CancellationToken cancellationToken)
        {
            List<OrderDocumentLine> tmpLines = new List<OrderDocumentLine>();
            List<OrderDocumentLine> resLines = new List<OrderDocumentLine>();
            int lineNum = 0;

            foreach (OrderDocumentLine dLine in document.documentLines)
            {
                //Если строка документа уже имеет связанный заказ, её не нужно больше посылать.
                if (!string.IsNullOrEmpty(dLine.orderId))
                {
                    resLines.Add(dLine);
                    continue;
                }

                if (lineNum > 0 && lineNum % 10 == 0)
                {
                    var lines = await _suz.SendDocLinesAsync(tmpLines, cancellationToken);
                    resLines.AddRange(lines);
                    tmpLines = new List<OrderDocumentLine>();
                }

                tmpLines.Add(dLine);
                lineNum++;
            }

            if (tmpLines.Count > 0)
            {
                var lines = await _suz.SendDocLinesAsync(tmpLines, cancellationToken);
                resLines.AddRange(lines);
            }

            document.documentLines = resLines;

            bool allPosted = true;

            document.documentLines.ForEach(l =>
            {
                if (string.IsNullOrEmpty(l.orderId))
                    allPosted = false;
            });

            if (allPosted)
                document.documentStatus = OrderDocumentStatus.POSTED;

            return document;
        }
    }
}
