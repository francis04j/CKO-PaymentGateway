
namespace WebApi.Models
{
    public class PayRequest
    {      
        public string CardNumber { get; set; }

        public string CardExpiryMonth { get; set; }

        public string CardExpiryDay { get; set; }

        public string CardCvv { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }
    }
}
    