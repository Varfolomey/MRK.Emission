using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using MRK.Emission.Business.Emission.Commands.PostOrder;
using MRK.Emission.Business.Emission.Queries.GetCises;
using MRK.Emission.Business.Emission.Queries.GetDocuments;
using MRK.Emission.GrpcClient.Contracts;
using MRK.Emission.GrpcClient.Contracts.Models.Emission.GetCises;
using MRK.Emission.GrpcClient.Contracts.Models.Emission.GetDocuments;
using MRK.Emission.GrpcClient.Contracts.Models.Emission.PostOrder;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MRK.Emission.Api
{
    public class EmissionService : IEmissionClientService
    {
        private readonly ILogger<EmissionService> _logger;
        private readonly IMediator _mediator;
        public EmissionService(ILogger<EmissionService> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async IAsyncEnumerable<CisInfo> GetCises(GetCisesRequest request, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var cises = await _mediator.Send(request.Adapt<GetCisesQueryRequest>(), cancellationToken);

            while (!cancellationToken.IsCancellationRequested)
            {
                yield return cises.CisList.Adapt<CisInfo>();
            }
        }

        public async Task<GetDocumentsResponse> GetDocuments(GetDocumentsRequest request, CancellationToken cancellationToken = default)
        {
            var response = await _mediator.Send(request.Adapt<GetDocumentsQueryRequest>(), cancellationToken);

            return response.Adapt<GetDocumentsResponse>();
        }

        public async Task<PostOrderResponse> PostOrderAsync(PostOrderCommand request, CancellationToken cancellationToken = default)
        {
            var response = await _mediator.Send(request.Adapt<PostOrderCommandRequest>(), cancellationToken);

            return response.Adapt<PostOrderResponse>();
        }
    }
}
