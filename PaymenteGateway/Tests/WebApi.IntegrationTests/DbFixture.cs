using Microsoft.EntityFrameworkCore;
using System;
using WebApi.Database.DynamoDb;
using WebApi.Database.EFCore;

namespace WebApi.IntegrationTests
{
    public class DbFixture : IDisposable
    {
        private readonly PaymentEFCoreDbContext _dbContext;
        public readonly string Payments = $"Payments-{Guid.NewGuid()}";
        public readonly string ConnString;

        private bool _disposed;

        public DbFixture()
        {
            ConnString = $"Server=localhost,1450;Database={Payments};User=sa;Password=2Secure*Password2";

            var builder = new DbContextOptionsBuilder<PaymentEFCoreDbContext>();

            builder.UseSqlServer(ConnString);
            _dbContext = new PaymentEFCoreDbContext(builder.Options, null);

            _dbContext.Database.Migrate();
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    
                    _dbContext.Database.EnsureDeleted();
                }

                _disposed = true;
            }
        }
    }
}
