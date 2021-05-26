using MRK.Emission.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace MRK.Emission.DataAccess.Repositories
{
    public interface IEmissionRepository
    {
        public Task<List<OrderDocument>> GetOrderDocumentsAsync(Expression<Func<OrderDocument, bool>> expression, CancellationToken cancellationToken = default);
        public Task<List<OrderDocumentLine>> GetOrderDocumentLinesAsync(Expression<Func<OrderDocumentLine, bool>> expression, CancellationToken cancellationToken = default);
        public Task<List<T>> AddRangeAsync<T>(List<T> items, CancellationToken cancellationToken = default);
        public Task<T> UpdateAsync<T>(T obj, CancellationToken cancellationToken = default) where T : new();
        public Task<int> CountCisInfosAsync(Expression<Func<CisInfo, bool>> expression, CancellationToken cancellationToken = default);
        public Task<List<CisInfo>> GetCisInfosAsync(Expression<Func<CisInfo, bool>> expression, CancellationToken cancellationToken = default);
    }
}
