using System;
using WebApi.ModelValidators;
using Xunit;
using FluentAssertions;
using WebApi.Models;
using FluentValidation.Results;

namespace WebApi.UnitTests
{
    public class PayRequestValidatorTests
    {
        readonly PayRequestValidator sut;

        public PayRequestValidatorTests()
        {
            sut = new PayRequestValidator();
        }

        [Fact]
        public void should_return_false_for_empty_card_expiry_month()
        {
            var request = new PayRequest() { CardExpiryMonth = String.Empty };
            
            ValidationResult response = sut.Validate(request);

            response.IsValid.Should().BeFalse();
        }

        [Fact]
        public void should_return_false_for_card_number_not_sixteen()
        {
            var request = new PayRequest() { CardNumber = "123" };

            ValidationResult response = sut.Validate(request);

            response.IsValid.Should().BeFalse();
        }
    }
}
