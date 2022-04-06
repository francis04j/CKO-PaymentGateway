using FluentValidation;
using WebApi.Models;

namespace WebApi.ModelValidators
{
    public class PayRequestValidator : AbstractValidator<PayRequest>
    {
        public readonly int EXPECTED_CARD_NUMBER_LENGTH = 16;
        public readonly int EXPECTED_CARD_CVV_LENGTH = 3;

        public PayRequestValidator()
        {            
            RuleFor(p => p.CardNumber).NotEmpty().Length(EXPECTED_CARD_NUMBER_LENGTH); 
            RuleFor(p => p.CardExpiryMonth).NotEmpty().MaximumLength(2);
            RuleFor(p => p.CardExpiryDay).NotEmpty().MaximumLength(2);
            RuleFor(p => p.CardCvv).NotEmpty().Length(EXPECTED_CARD_CVV_LENGTH);
            RuleFor(p => p.Amount).NotEmpty();
            RuleFor(p => p.Currency).NotEmpty();
        }
        
    }
}
