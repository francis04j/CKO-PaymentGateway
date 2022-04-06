using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using WebApi.Database;
using WebApi.Database.DynamoDb;
using WebApi.Database.Models;
using WebApi.Maskers;
using Xunit;

namespace WebApi.UnitTests
{
    
    public class DatabaseServiceTests
    {
        DatabaseService sut;
        Mock<IValueMasker> valueMaskerMock;
        Mock<IPaymentDbContext<PaymentEntity>> dbContextMock;
        Mock<ILogger<DatabaseService>> loggerMock;

        public DatabaseServiceTests()
        {
            valueMaskerMock = new Mock<IValueMasker>();
            dbContextMock = new Mock<IPaymentDbContext<PaymentEntity>>();
            loggerMock = new Mock<ILogger<DatabaseService>>();
            sut = new DatabaseService(valueMaskerMock.Object, dbContextMock.Object, loggerMock.Object);
        }
        
        [Fact]
        public void should_throw_argument_exception_for_null_dto()
        {
            PaymentDto paymentDto = null;
            
            Func<Task> code  = () => sut.SavePayment(paymentDto);

            Assert.ThrowsAsync<ArgumentNullException>(code);
        }

        [Fact]
        public async Task should_throw_argument_exception_with_message()
        {
            PaymentDto paymentDto = null;

            Func<Task> code = () => sut.SavePayment(paymentDto);

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(code);
            exception.Message.Should().Contain(nameof(paymentDto));
        }

        [Fact]
        public async Task should_use_mask_credit_card_number_before_storage()
        {
            var ardNumber = "123455667";
            PaymentDto paymentDto = new PaymentDto { CardNumber =  ardNumber};
            valueMaskerMock.Setup(x => x.Mask(It.IsAny<string>()));

            await sut.SavePayment(paymentDto);

            valueMaskerMock.Verify(x => x.Mask(It.Is<string>(y => y.Equals(ardNumber))), Times.Once);            
        }

        [Fact]
        public async Task should_use_store_masked_entity_in_dynamo()
        {
            PaymentDto paymentDto = new PaymentDto { CardNumber = "ardNumber" };
            valueMaskerMock.Setup(x => x.Mask(It.IsAny<string>())).Returns("**");
            dbContextMock.Setup(x => x.SaveAsync(It.IsAny<PaymentEntity>()));

            await sut.SavePayment(paymentDto);

            dbContextMock.Verify(x => x.SaveAsync(It.Is<PaymentEntity>(y => y.CardNumber == paymentDto.CardNumber)), Times.Once);
        }

        
        [Fact]
        public void should_throw_argument_exception_for_empty_paymentId()
        {
            string paymnetyId = string.Empty;

            Func<Task> code = () => sut.GetPaymentByID(paymnetyId);

            Assert.ThrowsAsync<ArgumentNullException>(code);
        }

        [Fact]
        public async Task should_get_payment_details_from_database()
        {
            string paymentId = "123";
            var paymentEntity = new PaymentEntity() { BankTransactionCode = "123" };
            dbContextMock.Setup(x => x.GetByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(paymentEntity));
            
            var response = await sut.GetPaymentByID(paymentId);

            response.BankTransactionCode.Should().Be("123");
        }

        [Fact]
        public async Task should_map_payment_details_to_dto()
        {
            string paymentId = "123";
            var paymentEntity = new PaymentEntity() { BankTransactionCode = "123" };
            dbContextMock.Setup(x => x.GetByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(paymentEntity));

            var response = await sut.GetPaymentByID(paymentId);

            response.GetType().Should().Be(typeof(PaymentDto));
        }

    }
}
