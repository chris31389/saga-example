using Invoices.Worker.Sagas;
using Microsoft.EntityFrameworkCore;

namespace Invoices.Worker.Shared.Persistence;

public class InvoiceDbContext : DbContext
{
    public InvoiceDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrderSagaData>().HasKey(x => x.CorrelationId);
    }

    public DbSet<OrderSagaData> SagaData { get; set; }
}