using CodeTest.Transfers;
using Microsoft.AspNetCore.Mvc;

namespace CodeTest.Controllers
{
    [ApiController]
    [Route("transfers")]
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

        [HttpPost(Name = "quote")]
        public async Task<ActionResult<QuoteResponse>> Post([FromBody] QuoteRequest data)
        {
            if (data == null)
            {
               return  BadRequest("Missing expected input data");
            }

            try
            {
                var result = await _transferService.ProcessQuote(data);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }            
        }

        [HttpGet(Name = "quote/{quoteId}")]
        public async Task<ActionResult<QuoteResponse>> Get(Guid quoteId)
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
    }
}
