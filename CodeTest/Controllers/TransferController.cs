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
        public async Task<QuoteResponse> Post([FromBody] QuoteRequest data)
        {
            if (data == null)
            {
                throw new ApplicationException("Missing expected input data");
            }


            var result = await _transferService.ProcessQuote(data);

            return result;
        }
    }
}
