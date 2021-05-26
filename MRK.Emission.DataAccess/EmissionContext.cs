using Microsoft.EntityFrameworkCore;
using MRK.Emission.Domain.Models;

namespace MRK.Emission.DataAccess
{
    public class EmissionContext : DbContext
    {
        public DbSet<CisInfo> CisInfos { get; set; }
        public DbSet<OrderDocument> OrderDocuments { get; set; }
        public DbSet<OrderDocumentLine> OrderDocumentLines { get; set; }


        public EmissionContext(DbContextOptions<EmissionContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<OrderDocument>()
               .HasKey(o => new { o.documentId, o.clientName });

            modelBuilder.Entity<OrderDocumentLine>().HasKey(ol => new { ol.documentId, ol.documentLineNum, ol.clientName });

            modelBuilder.Entity<CisInfo>().HasKey(u => new { u.Id });

            modelBuilder
                .Entity<OrderDocument>()
                .HasMany(o => o.documentLines)
                .WithOne(l => l.orderDocument);
        }
    }
}
