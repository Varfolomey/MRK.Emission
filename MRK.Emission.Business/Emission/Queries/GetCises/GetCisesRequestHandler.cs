using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MRK.Emission.Business.Emission.Queries.GetCises
{
    public class GetCisesRequestHandler : IRequestHandler<GetCisesQueryRequest, GetCisesResponse>
    {
        public Task<GetCisesResponse> Handle(GetCisesQueryRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
