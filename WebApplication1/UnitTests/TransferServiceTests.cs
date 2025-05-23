using CodeTest.Helpers;
using CodeTest.Rates;
using CodeTest.Transfers;
using CodeTest.Transfers.DTO;
using Moq;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class TransferServiceTests
    {
        private TransferService _transferService;
        private Mock<IUniRateService> _mockUniRateService;
        private Mock<ICacheService> _mockCacheService;

        [SetUp]
        public void SetUp()
        {
            _mockUniRateService = new Mock<IUniRateService>();
            _mockCacheService = new Mock<ICacheService>();
            _transferService = new TransferService(_mockUniRateService.Object, _mockCacheService.Object);
        }

        [Test]
        public void GetCachedQuote_WithValidId_ReturnsExpectedQuote()
        {
            //Arrange
            var quoteId = Guid.NewGuid();
            var cachedQuote = new QuoteResponse
            {
                ConvertedAmount = 1234,
                QuoteId = quoteId,
                InverseOfxRate = 0.5m,
                OfxRate = 0.5m
            };

            _mockCacheService.Setup(x => x.GetCachedQuote(quoteId.ToString())).Returns(cachedQuote);

            //Act
            var foundQuote = _transferService.GetQuote(quoteId);

            //Assert
            Assert.That(foundQuote == cachedQuote);
        }

        [Test]
        public void GetCachedQuote_WithInValidId_ThrowsError()
        {
            //Arrange
            var quoteId = Guid.NewGuid();

            //Act
            try
            {
                var foundQuote = _transferService.GetQuote(quoteId);
            }
            catch (Exception ex)
            {
                //Assert
                Assert.That(ex.Message.Contains($"Quote with id {quoteId} not found"));
            }      
        }

        [Test]
        public void GetCachedTransfer_WithValidId_ReturnsExpectedTransfer()
        {
            //Arrange
            var transferId = Guid.NewGuid();
            var cachedTransfer = new TransferResponse
            {
                TransferId = transferId,
                EstimatedDeliveryDate = DateTime.UtcNow.AddDays(1),
                Status = Enum.GetName(TransferStatus.Processing)                
            };

            
            _mockCacheService.Setup(x => x.GetCachedTransfer(transferId.ToString())).Returns(cachedTransfer);

            //Act
            var foundTransfer = _transferService.GetTransfer(transferId);

            //Assert
            Assert.That(foundTransfer == cachedTransfer);
        }

        [Test]
        public void GetCachedTransfer_WithInValidId_ThrowsError()
        {
            //Arrange
            var transferId = Guid.NewGuid();

            //Act
            try
            {
                var foundQuote = _transferService.GetTransfer(transferId);
            }
            catch (Exception ex)
            {
                //Assert
                Assert.That(ex.Message.Contains($"Transfer with id {transferId} not found"));
            }
        }

        [Test]
        public async Task ProcessQuote_WithValidData_ReturnsExpectedRates()
        {
            //Arrange
            var buyCurrency = Currency.USD;
            var sellCurrency = Currency.AUD;
            var ofxRate = 0.25m;
            var inverseRate = 0.75m;
            var amount = 1234.5m;
            var expectedAmount = Decimal.Round(amount * ofxRate, 2);

            var quoteRequest = new QuoteRequest()
            {
                BuyCurrency = CurrencyParser.ToString(buyCurrency),
                SellCurrency = CurrencyParser.ToString(sellCurrency),
                Amount = amount
            };

            _mockUniRateService.Setup(x => x.GetRate(buyCurrency, sellCurrency)).ReturnsAsync(ofxRate);
            _mockUniRateService.Setup(x => x.GetRate(sellCurrency, buyCurrency)).ReturnsAsync(inverseRate);

            //Act
            var result = await _transferService.ProcessQuote(quoteRequest);

            //Assert
            Assert.That(result.OfxRate == ofxRate);
            Assert.That(result.InverseOfxRate == inverseRate);
            Assert.That(result.ConvertedAmount == expectedAmount);
        }

        [Test]
        public async Task CreateTransfer_WithValidData_ReturnsExpectedTransfer()
        {
            //Arrange
            var quoteId = Guid.NewGuid();
            var transferRequest = new TransferRequest()
            {
                QuoteId = quoteId,
                Payer = new Payer
                {
                    Id = Guid.NewGuid(),
                    Name = "Steve",
                    TransferReason = "Invoice"
                },
                Recipient = new Recipient
                {
                    Name = "Jess",
                    AccountNumber = "1234",
                    BankCode = "112",
                    BankName = "St George"
                }
            };

            var cachedQuote = new QuoteResponse
            {
                ConvertedAmount = 1234,
                QuoteId = quoteId,
                InverseOfxRate = 0.5m,
                OfxRate = 0.5m
            };

            _mockCacheService.Setup(x => x.GetCachedQuote(quoteId.ToString())).Returns(cachedQuote);

            //Act
            var result = await _transferService.CreateTransfer(transferRequest);

            //Assert
            Assert.That(result.TransferDetails.Payer == transferRequest.Payer);
            Assert.That(result.TransferDetails.Recipient == transferRequest.Recipient);
            Assert.That(result.Status == "Processing");
        }
    }
}