using Beerbase.Controllers;
using Beerbase.Dto;
using Beerbase.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;

namespace Beerbase.Tests.Unit
{
    public class BreweryControllerTests
    {
        private readonly Mock<ILogger<BreweryController>> _loggerMock;

        private readonly Mock<BeerbaseContext> _beerbaseContextMock;

        private readonly BreweryController _sut;

        public BreweryControllerTests()
        {
            _loggerMock = new Mock<ILogger<BreweryController>>();

            _beerbaseContextMock = new Mock<BeerbaseContext>();

            var sampleBreweries = new Brewery[]
            {
                new Brewery() {
                        BreweryId = 1,
                        Name = "Test Brewery One",
                        Beers = Array.Empty<Beer>()
                    },
                new Brewery() {
                        BreweryId = 2,
                        Name = "Test Brewery Two",
                        Beers = Array.Empty<Beer>()
                    }
            };

            _beerbaseContextMock
                .Setup(x => x.Breweries)
                .ReturnsDbSet(sampleBreweries);

            _sut = new BreweryController(_loggerMock.Object, _beerbaseContextMock.Object);
        }

        [Fact]
        public async Task PostMapsCorrectProperties()
        {
            var result = await _sut.PostAsync(new AddOrUpdateBreweryDto()
            {
                Name = "Test Brewery Three",
            });

            var value = result.AssertAndGetOkValue();
            Assert.Equal("Test Brewery Three", value.Name);

            CancellationToken ct = default;
            _beerbaseContextMock.Verify(b => b.SaveChangesAsync(ct), Times.Once()); ;
        }

        [Fact]
        public async Task GetReturnsNotFound()
        {
            _beerbaseContextMock
                .Setup(x => x.Breweries)
                .ReturnsDbSet(new Brewery[] { });

            var result = await _sut.GetAsync(1);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetReturnsById()
        {
            var result = await _sut.GetAsync(1);

            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void GetBreweriesReturnsAll()
        {
            var result = _sut.Get();
            var values = result.AssertAndGetOkValue();

            Assert.Equal(2, values.Count());
            Assert.Equal("Test Brewery One", values.First().Name);
            Assert.Equal("Test Brewery Two", values.Skip(1).First().Name);
        }
    }
}