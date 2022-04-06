using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApi.Bank;
using WebApi.Models;
using Mapster;
using System.Threading.Tasks;
using WebApi.Database;
using System;

namespace WebApi.Controllers
{
	[Route("api/[action]")]
	[ApiController]
	public class PayController : ControllerBase
	{
		private readonly ILogger<PayController> _logger;
		private readonly IBankService _bankService;
		private readonly IDatabaseService _db;

		public PayController(ILogger<PayController> logger, IBankService bankService, IDatabaseService databaseService)
		{
			_logger = logger;
			_bankService = bankService;
			_db = databaseService;
		}

		[HttpPost]
		[ProducesResponseType(typeof(PayResponse), StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> MakePayment([FromBody] PayRequest body)
		{
            try
            {
				BankPaymentRequest request = body.Adapt<BankPaymentRequest>();
				BankResponse bankResponse = await _bankService.SubmitMerchantPayment(request);

				PaymentDto paymentEntity = body.Adapt<PaymentDto>();
				paymentEntity.BankTransactionCode = bankResponse.TransactionCode;
				paymentEntity.BankStatus =	bankResponse.Status;
				paymentEntity.BankMessage = bankResponse.Message;

				string paymentId = await _db.SavePayment(paymentEntity);
				
				return Ok(new PayResponse { PaymentId = paymentId });
			}
			catch(Exception ex)
            {
				_logger.LogError("Error:", ex); //TODO: clean up
 				return StatusCode(500); //TODO: better error
			}
			
		}

		[HttpGet]
		[ProducesResponseType(typeof(PaymentDetailsResponse), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetPayment([FromQuery] string paymentId)
		{
            try
            {
				PaymentDto response = await _db.GetPaymentByID(paymentId);
				PaymentDetailsResponse res = response.Adapt<PaymentDetailsResponse>();
				
				if (res == null) return NotFound();
				
				return Ok(res);
			}
			catch(Exception ex)
            {
				_logger.LogError("ServerError:", ex);
				return StatusCode(500);
			}
			
		}
	}
}
