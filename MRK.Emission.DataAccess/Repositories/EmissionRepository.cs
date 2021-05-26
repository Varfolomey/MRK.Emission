using Microsoft.EntityFrameworkCore;
using MRK.Emission.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace MRK.Emission.DataAccess.Repositories
{
    public class EmissionRepository : IEmissionRepository
    {
        private readonly EmissionContext _context;
        public EmissionRepository(EmissionContext context)
        {
            _context = context;
        }

        public async Task<List<OrderDocument>> GetOrderDocumentsAsync(Expression<Func<OrderDocument, bool>> expression, CancellationToken cancellationToken = default)
            => await _context.OrderDocuments.Where(expression).ToListAsync(cancellationToken);

        public async Task<List<OrderDocumentLine>> GetOrderDocumentLinesAsync(Expression<Func<OrderDocumentLine, bool>> expression, CancellationToken cancellationToken = default)
            => await _context.OrderDocumentLines.Where(expression).ToListAsync(cancellationToken);

        public async Task<List<T>> AddRangeAsync<T>(List<T> items, CancellationToken cancellationToken = default)
        {
            items.ForEach(async i => await _context.AddAsync(i));
            await _context.SaveChangesAsync(cancellationToken);

            return items;
        }

        public async Task<T> UpdateAsync<T>(T obj, CancellationToken cancellationToken = default)
            where T : new()
        {
            _context.Update(obj);
            await _context.SaveChangesAsync(cancellationToken);

            return obj;
        }

        public Task<int> CountCisInfosAsync(Expression<Func<CisInfo, bool>> expression, CancellationToken cancellationToken = default)
            => _context.CisInfos.Where(expression).CountAsync(cancellationToken);

        public Task<List<CisInfo>> GetCisInfosAsync(Expression<Func<CisInfo, bool>> expression, CancellationToken cancellationToken = default)
            => _context.CisInfos.Where(expression).ToListAsync(cancellationToken);
    }
}
