using MediatR;
using MRK.Emission.DataAccess.Repositories;
using MRK.Emission.Domain.Enums;
using MRK.Emission.Domain.Models;
using MRK.Emission.Service.SUZ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace MRK.Emission.Business.Emission.Commands.ProceedOrders
{
    public class ProceedOrdersCommandHandler : IRequestHandler<ProceedOrdersCommand, ProceedOrdersResponse>
    {
        private readonly IEmissionRepository _repository;
        private readonly ISuzService _suz;

        public ProceedOrdersCommandHandler(IEmissionRepository repository,
            ISuzService suz)
        {
            _repository = repository;
            _suz = suz;
        }

        public async Task<ProceedOrdersResponse> Handle(ProceedOrdersCommand request, CancellationToken cancellationToken)
        {
            var response = new ProceedOrdersResponse();

            if (!_suz.Auth())
            {
                response.ErrorMessage = $"Unable to get access token!";
                return response;
            }

            if (!await _suz.PingAsync("ProceedOrderDocumentsCommand", cancellationToken))
            {
                response.ErrorMessage = $"SUZ host is unreachable!";
                return response;
            }

            Expression<Func<OrderDocumentLine, bool>> expression = dl => 
                !dl.documentLineStatus.Equals(DocumentLineStatus.COMPLITED) &&
                !dl.documentLineStatus.Equals(DocumentLineStatus.ERROR);

            var docLines = await _repository.GetOrderDocumentLinesAsync(expression, cancellationToken);

            foreach (OrderDocumentLine curDocLine in docLines)
            {
                var bufferInfo = await _suz.GetBufferInfoAsync(curDocLine, cancellationToken);

                if (bufferInfo == null)
                    continue;

                switch (bufferInfo.bufferStatus)
                {
                    case "PENDING":
                        break;
                    case "ACTIVE":
                        int codesCount = bufferInfo.availableCodes;
                        codesCount = Math.Min(200, codesCount);

                        if (codesCount > 0)
                            await GenerateCisInfos(curDocLine, codesCount, cancellationToken);
                        break;
                    case "EXHAUSTED":
                        break;
                    case "REJECTED":
                        break;
                }

                Expression<Func<OrderDocumentLine, bool>> exp = sumLine =>
                    sumLine.orderId.Equals(curDocLine.orderId) &&
                    sumLine.gtin.Equals(curDocLine.gtin);

                var cnt = (await _repository.GetOrderDocumentLinesAsync(exp, cancellationToken))
                    .Sum(a => a.qty);

                Expression<Func<CisInfo, bool>> cisExp = cis =>
                    cis.gtin.Equals(curDocLine.gtin) &&
                    cis.orderId.Equals(curDocLine.orderId);

                var infos = await _repository.CountCisInfosAsync(cisExp, cancellationToken);

                if (infos > 0 && infos.Equals(cnt))
                {
                    curDocLine.documentLineStatus = DocumentLineStatus.COMPLITED;

                    await _repository.UpdateAsync(curDocLine, cancellationToken);
                    await _suz.CloseOrderAsync(curDocLine, cancellationToken);
                }
            }

            return response;
        }

        public async Task<List<CisInfo>> GenerateCisInfos(OrderDocumentLine docLine, 
            int codesCount, 
            CancellationToken cancellationToken)
        {
            (var dl, var infos) = await _suz.GetCodesAsync(docLine, codesCount, cancellationToken);

            await _repository.AddRangeAsync(infos, cancellationToken);
            await _repository.UpdateAsync(dl, cancellationToken);

            return infos;
        }
    }
}
