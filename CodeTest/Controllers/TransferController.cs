using CodeTest.Transfers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        public QuoteResponse Post([FromBody] string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                throw new ApplicationException("Missing expected input data");
            }

            var inputs = JsonConvert.DeserializeObject<QuoteRequest>(data);

            if (inputs == null)
            {
                throw new ApplicationException("Issue processing inputs");
            }

            var result = _transferService.ProcessQuote(inputs);

            return result;
        }
    }
}
