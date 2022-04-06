using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using System.Net.Http.Json;
using FluentAssertions;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace WebApi.IntegrationTests
{
    [Collection("Database")]
    public class MakePaymentTests : IClassFixture<ApiWebApplicationFactory>
    {
        private readonly HttpClient _client;

        private readonly ApiWebApplicationFactory _factory;

        public MakePaymentTests(ApiWebApplicationFactory factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task CreatePayment_should_return_validation_error()
        {
            // Arrange
            var createRequest = new PaymentRequest()
            {
                Amount = 0,
                CardNumber = "1234567890123456",
                CardExpiryMonth = "11",
                CardExpiryDay = "10",
                CardCvv = "123",
                Currency = "USD"
            };

            // Act
            HttpResponseMessage postResponse = await _client
            .PostAsync("/api/MakePayment", JsonContent.Create<PaymentRequest>(createRequest));

            string json = await postResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<CreatePaymentErrorResponse>(json);

            response.status.Should().Be("400");
            response.errors.Should().HaveCount(1);
            response.errors.ContainsKey("Amount").Should().BeTrue();
        }

        [Fact]
        public async Task should_store_payment_request()
        {
            // Arrange
            var createRequest = new PaymentRequest()
            {
                Amount = 100,
                CardNumber = "1234567890123456",
                CardExpiryMonth = "11",
                CardExpiryDay = "10",
                CardCvv = "123",
                Currency = "USD"
            };

            // Act
            HttpResponseMessage postResponse = await _client
            .PostAsync("/api/MakePayment", JsonContent.Create<PaymentRequest>(createRequest));

            string json = await postResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<CreatePaymentResponse>(json);

            Assert.NotNull(response);


            var getResponse = await _client
            .GetAsync($"/api/GetPayment?PaymentId={response.PaymentId}");

            json = await getResponse.Content.ReadAsStringAsync();
            GetPaymentResponse gresponse = JsonConvert.DeserializeObject<GetPaymentResponse>(json);

            gresponse.Amount.Should().Be(createRequest.Amount);
            gresponse.CardExpiryMonth.Should().Be(createRequest.CardExpiryMonth);
            gresponse.CardExpiryDay.Should().Be(createRequest.CardExpiryDay);
            gresponse.Currency.Should().Be(createRequest.Currency);
            gresponse.CardNumber.Should().Be("******");
            gresponse.CardCvv.Should().Be(createRequest.CardCvv);
        }


    }

    public class PaymentRequest
    {
        public string CardNumber { get; set; }

        public string CardExpiryMonth { get; set; }

        public string CardExpiryDay { get; set; }

        public string CardCvv { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }
    }

    public class CreatePaymentResponse
    {
        public string PaymentId { get; set; }
    }

    public class CreatePaymentErrorResponse
    {
        public string type { get; set; }
        public string status { get; set; }
        public Dictionary<string, List<string>> errors { get; set; }

    }

    public class GetPaymentResponse
    {
        public string CardNumber { get; set; }

        public string CardExpiryMonth { get; set; }

        public string CardExpiryDay { get; set; }

        public string CardCvv { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public string BankTransactionCode { get; set; }

        public string BankStatus { get; set; }

        public string BankMessage { get; set; }
    }
}
