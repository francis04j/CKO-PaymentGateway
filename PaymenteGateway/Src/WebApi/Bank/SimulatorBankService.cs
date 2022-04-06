using System;
using System.Threading.Tasks;

namespace WebApi.Bank
{
    public class SimulatorBankService : IBankService
    {
        public Task<BankResponse> SubmitMerchantPayment(BankPaymentRequest paymentRequest)
        {
            if (paymentRequest.CardNumber.StartsWith("0000"))
                return Task.FromResult(new BankResponse() { TransactionCode = Guid.NewGuid().ToString(), Status = "Rejected", Message = "Possible Fraud" });
            if (paymentRequest.CardNumber.StartsWith("1000"))
                return Task.FromResult(new BankResponse() { TransactionCode = Guid.NewGuid().ToString(), Status = "Rejected", Message = "" });
         

            return Task.FromResult(new BankResponse() { TransactionCode = Guid.NewGuid().ToString(), Status = "Success", Message = "" });
        }
    }
}
