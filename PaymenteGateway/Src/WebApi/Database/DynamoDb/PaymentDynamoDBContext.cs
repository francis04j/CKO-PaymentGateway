using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApi.Database.Models;

namespace WebApi.Database.DynamoDb
{
    public class PaymentDynamoDBContext : DynamoDBContext, IPaymentDbContext<PaymentEntity>
    {
        private DynamoDBOperationConfig _config;
        private ILogger<PaymentDynamoDBContext> _logger;

        public PaymentDynamoDBContext(IAmazonDynamoDB client, string tableName, ILogger<PaymentDynamoDBContext> logger) : base(client)
        {
            _config = new DynamoDBOperationConfig()
            {
                OverrideTableName = tableName
            };
            _logger = logger;
        }

        public async Task SaveAsync(PaymentEntity item)
        {
            try
            {
                await base.SaveAsync(item, _config);
            }catch (Exception ex) //TODO: add generic exceptionj handloing
            {
                _logger.LogError("Saving PaymentEntity with Transaction code of {Id} failed with Error message {Message}", item.BankTransactionCode, ex.Message);
                throw;
            }
            
        }

        public async Task<PaymentEntity> GetByIdAsync(string id)
        {
            try
            {
                PaymentEntity enityt = await base.LoadAsync<PaymentEntity>(id, _config);
                return enityt;
            }
            catch (Exception ex) 
            {
                _logger.LogError("GetByIdAsync Failed for payment id of {Id} with error message {Message}", id, ex.Message);
                throw;
            }
        }
    }
}
