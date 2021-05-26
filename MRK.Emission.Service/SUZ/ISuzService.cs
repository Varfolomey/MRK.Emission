using MRK.Emission.Domain.Models;
using MRK.Emission.Domain.Models.SUZ;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MRK.Emission.Service.SUZ
{
    public interface ISuzService
    {
        public bool Auth();
        public Task<bool> PingAsync(string clientName, CancellationToken cancellationToken = default);
        public Task<List<OrderDocumentLine>> SendDocLinesAsync(List<OrderDocumentLine> docLines, CancellationToken cancellationToken = default);
        public Task<BufferInfo> GetBufferInfoAsync(OrderDocumentLine docLine, CancellationToken cancellationToken = default);
        public Task<(OrderDocumentLine, List<CisInfo>)> GetCodesAsync(OrderDocumentLine docLine, int codesCount, CancellationToken cancellationToken = default);
        public Task<bool> CloseOrderAsync(OrderDocumentLine docLine, CancellationToken cancellationToken = default);
    }
}
