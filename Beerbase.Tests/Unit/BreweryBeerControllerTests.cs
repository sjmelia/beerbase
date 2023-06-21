using Beerbase.Controllers;
using Beerbase.Dto;
using Beerbase.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;

namespace Beerbase.Tests.Unit
{
    public class BreweryBeerControllerTests
    {
        private readonly Mock<ILogger<BreweryBeerController>> _loggerMock;

        private readonly Mock<BeerbaseContext> _beerbaseContextMock;

        private readonly BreweryBeerController _sut;

        public BreweryBeerControllerTests()
        {
            _loggerMock = new Mock<ILogger<BreweryBeerController>>();

            _beerbaseContextMock = new Mock<BeerbaseContext>();

            var sampleBeers = new Beer[]
            {
                new Beer() {
                        BeerId = 1,
                        Name = "Test Beer One",
                        PercentageAlcoholByVolume = 10.0m,
                        BarsServedAt = new Bar[] { },
                    },
                new Beer() {
                        BeerId = 2,
                        Name = "Test Beer Two",
                        PercentageAlcoholByVolume = 20.0m,
                        BarsServedAt = new Bar[] { },
                    },
                new Beer() {
                        BeerId = 3,
                        Name = "Test Beer Three",
                        PercentageAlcoholByVolume = 30.0m,
                        BarsServedAt = new Bar[] { },
                    },
            };

            var sampleBreweries = new Brewery[]
            {
                new Brewery() {
                        BreweryId = 1,
                        Name = "Test Brewery One",
                        Beers = new List<Beer>()
                    },
                new Brewery() {
                        BreweryId = 2,
                        Name = "Test Brewery Two",
                        Beers = new List<Beer> { sampleBeers.Skip(1).First() }
                    }
            };

            _beerbaseContextMock
                .Setup(x => x.Breweries)
                .ReturnsDbSet(sampleBreweries);

            _beerbaseContextMock
                .Setup(x => x.Beers)
                .ReturnsDbSet(sampleBeers);

            _sut = new BreweryBeerController(_loggerMock.Object, _beerbaseContextMock.Object);
        }

        [Fact]
        public async Task PostMapsCorrectProperties()
        {
            var result = await _sut.PostAsync(new AddOrUpdateBreweryBeerDto()
            {
                BeerId = 1,
                BreweryId = 1
            });

            var value = result.AssertAndGetOkValue();
            Assert.Equal("Test Brewery One", value.Name);

            CancellationToken ct = default;
            _beerbaseContextMock.Verify(b => b.SaveChangesAsync(ct), Times.Once()); ;
        }

        [Fact]
        public async Task PostReturnsNotFound()
        {
            var result = await _sut.PostAsync(new AddOrUpdateBreweryBeerDto()
            {
                BeerId = 100,
                BreweryId = 100
            });

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetReturnsById()
        {
            var result = await _sut.GetAsync(1);

            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void GetBreweryBeersReturnsAll()
        {
            var result = _sut.Get();
            var values = result.AssertAndGetOkValue();

            Assert.Equal(2, values.Count());
            Assert.Equal("Test Brewery One", values.First().Name);
            Assert.Equal("Test Brewery Two", values.Skip(1).First().Name);
        }
    }
}