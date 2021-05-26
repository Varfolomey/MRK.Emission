using MRK.Emission.GrpcClient.Contracts.Models.Emission.GetCises;
using MRK.Emission.GrpcClient.Contracts.Models.Emission.GetDocuments;
using MRK.Emission.GrpcClient.Contracts.Models.Emission.PostOrder;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;

namespace MRK.Emission.GrpcClient.Contracts
{
    [ServiceContract(Name = "MRK.Emission.Api")]
    public interface IEmissionClientService
    {
        [OperationContract]
        public Task<PostOrderResponse> PostOrderAsync(PostOrderCommand request, CancellationToken cancellationToken = default);
        
        [OperationContract]
        public Task<GetDocumentsResponse> GetDocuments(GetDocumentsRequest request, CancellationToken cancellationToken = default);
        
        [OperationContract]
        public IAsyncEnumerable<CisInfo> GetCises(GetCisesRequest request, CancellationToken cancellationToken = default);
    }
}
