using CodeTest.Transfers;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CodeTest.Controllers
{
    [ApiController]
    [Route("transfers")]
    [Produces("application/json")]
    public class TransferController : ControllerBase
    {
        private readonly ILogger<TransferController> _logger;
        private readonly ITransferService _transferService;

        public TransferController(
            ILogger<TransferController> logger,
            ITransferService transferService)
        {
            _logger = logger;
            _transferService = transferService;
        }

        [HttpPost("quote")]
        public async Task<ActionResult<QuoteResponse>> PostQuote([FromBody] QuoteRequest data)
        {
            if (data == null)
            {
               return  BadRequest("Missing expected input data");
            }

            try
            {
                var result = await _transferService.ProcessQuote(data);

                return Created("", result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }            
        }

        [HttpGet("quote/{quoteId}")]
        public async Task<ActionResult<QuoteResponse>> GetQuote(Guid quoteId)
        {
            if(quoteId == Guid.Empty)
            {
                return BadRequest("Missing quote id");
            }

            try
            {
                var result = _transferService.GetQuote(quoteId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<TransferResponse>> PostTransfer(TransferRequest request)
        {
            if (request == null)
            {
                return BadRequest("Missing expected input data");
            }

            try
            {
                var result = await _transferService.CreateTransfer(request);

                return Created("", result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
