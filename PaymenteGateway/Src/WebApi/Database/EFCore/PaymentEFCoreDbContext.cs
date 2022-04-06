using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using WebApi.Database;
using WebApi.Database.Models;

namespace WebApi.Database.EFCore
{

    public class Payment
    {
        public string PaymentId { get; set; }
        public string BankTransactionCode { get; set; }
        public string BankStatus { get; set; }

        public string BankMessage { get; set; }

        public string CardNumber { get; set; }

        public string CardExpiryMonth { get; set; }

        public string CardExpiryDay { get; set; }

        public string CardCvv { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }
    }

    public class PaymentEFCoreDbContext : DbContext, IPaymentDbContext<PaymentEntity>
    {
        private ILogger<PaymentEFCoreDbContext> _logger;
        public DbSet<PaymentEntity> Payments { get; set; }

        public PaymentEFCoreDbContext(DbContextOptions<PaymentEFCoreDbContext> options, ILogger<PaymentEFCoreDbContext> logger) : base(options)
        {
            _logger = logger;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PaymentEntity>()
                .Property(x => x.Amount)
                .HasColumnType("decimal(19,4)");
             modelBuilder.Entity<PaymentEntity>().HasKey(x => x.PaymentId);
           
        }

        public async Task<PaymentEntity> GetByIdAsync(string id)
        {
           
            try
            {
                PaymentEntity entity = await base.FindAsync<PaymentEntity>(id);
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError("GetByIdAsync Failed for payment id of {Id} with error message {Message}", id, ex.Message);
                throw;
            }
        }

        public async Task SaveAsync(PaymentEntity item)
        {
            try { 
                Payments.Add(item);
                await base.SaveChangesAsync();
            }
            catch (Exception ex) 
            {
                _logger.LogError("SaveAsync Failed for BankTransactionCode  {Id} with error message {Message}", item.BankTransactionCode, ex.Message);
                throw;
            }

        }
    }
}
