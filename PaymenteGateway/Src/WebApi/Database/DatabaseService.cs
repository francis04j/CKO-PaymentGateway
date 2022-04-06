using Mapster;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using WebApi.Database.DynamoDb;
using WebApi.Database;
using WebApi.Maskers;
using WebApi.Database.Models;

namespace WebApi.Database
{

    public class DatabaseService :  IDatabaseService
    {
        readonly ILogger<DatabaseService> _logger;
        readonly IValueMasker _valueMasker;
        readonly IPaymentDbContext<PaymentEntity> _dbContext;

        public DatabaseService(IValueMasker valueMasker, IPaymentDbContext<PaymentEntity> dbContext, ILogger<DatabaseService> logger)
        {
            _valueMasker = valueMasker;
            _dbContext = dbContext;
            _logger = logger;
        }

        
        public async Task<string> SavePayment(PaymentDto paymentDto)
        {
            if (paymentDto == null)
            {
                _logger.LogError("ArgumentNullException: {arg}", nameof(paymentDto)); //TODO: make this better
                throw new ArgumentNullException(nameof(paymentDto));               
            }

            try
            {              
                paymentDto.CardNumber = _valueMasker.Mask(paymentDto.CardNumber);
                PaymentEntity entity = paymentDto.Adapt<PaymentEntity>();
                entity.PaymentId = Guid.NewGuid().ToString();

                await _dbContext.SaveAsync(entity);
          

                return entity.PaymentId;
            }
            catch (Exception ex)
            {
                _logger.LogError("SavePayment failed for BankTransactionCode {code} with Message: {Msg}", paymentDto.BankTransactionCode, ex.Message); 
                throw;
            }
            
        }

        public async Task<PaymentDto> GetPaymentByID(string paymentId)
        {
            if (string.IsNullOrEmpty(paymentId))
            {
                _logger.LogError("ArgumentNullException: {arg}", nameof(paymentId));
                throw new ArgumentNullException(nameof(paymentId));
            }

            try
            {
                 var entity = await _dbContext.GetByIdAsync(paymentId);
              //  using (var db = new PaymentEFCoreDbContext())
              //  {
                   // var entity  =await db.Payments.FindAsync(paymentId);
                    PaymentDto dto = entity.Adapt<PaymentDto>();
                    return dto;
              //  }
                    

                
            }catch (Exception ex)
            {
                _logger.LogError("GetPaymentByID for PaymentId: {id} failed with Message: {Msg}", paymentId, ex.Message);
                throw;
            }
            
        }

    }
}
