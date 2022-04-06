namespace WebApi.Database.Models
{    
    public class PaymentEntity
    {        

        public string PaymentId { get; set; }
        public string BankTransactionCode { get; set; }
        public string  BankStatus { get; set; }

        public string BankMessage { get; set; }

        public string CardNumber { get; set; }

        public string CardExpiryMonth { get; set; }

        public string CardExpiryDay { get; set; }

        public string CardCvv { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }        
    }
}
