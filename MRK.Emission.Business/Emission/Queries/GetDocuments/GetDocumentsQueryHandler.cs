using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MRK.Emission.Business.Emission.Queries.GetDocuments
{
    public class GetDocumentsQueryHandler : IRequestHandler<GetDocumentsQueryRequest, GetDocumentsQueryResponse>
    {
        public Task<GetDocumentsQueryResponse> Handle(GetDocumentsQueryRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
