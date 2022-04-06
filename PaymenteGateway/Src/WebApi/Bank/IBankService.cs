using System.Threading.Tasks;

namespace WebApi.Bank
{
    public interface IBankService
    {
        Task<BankResponse> SubmitMerchantPayment(BankPaymentRequest paymentRequest);
    }
}
