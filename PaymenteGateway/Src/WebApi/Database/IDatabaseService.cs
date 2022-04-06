using System.Threading.Tasks;

namespace WebApi.Database
{
    public interface IDatabaseService
    {
        Task<string> SavePayment(PaymentDto payment);
        Task<PaymentDto> GetPaymentByID(string paymentId);
    }
}
