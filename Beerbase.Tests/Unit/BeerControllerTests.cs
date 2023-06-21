using Beerbase.Controllers;
using Beerbase.Dto;
using Beerbase.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;

namespace Beerbase.Tests.Unit
{
    public class BeerControllerTests
    {
        private readonly Mock<ILogger<BeerController>> _loggerMock;

        private readonly Mock<BeerbaseContext> _beerbaseContextMock;

        private readonly BeerController _sut;

        public BeerControllerTests()
        {
            _loggerMock = new Mock<ILogger<BeerController>>();

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

            _beerbaseContextMock
                .Setup(x => x.Beers)
                .ReturnsDbSet(sampleBeers);

            _sut = new BeerController(_loggerMock.Object, _beerbaseContextMock.Object);
        }

        [Fact]
        public async Task PostMapsCorrectProperties()
        {
            var result = await _sut.PostAsync(new AddOrUpdateBeerDto()
            {
                Name = "Test Beer",
                PercentageAlcoholByVolume = 100.1m
            });

            var value = result.AssertAndGetOkValue();
            Assert.Equal("Test Beer", value.Name);
            Assert.Equal(100.1m, value.PercentageAlcoholByVolume);

            CancellationToken ct = default;
            _beerbaseContextMock.Verify(b => b.SaveChangesAsync(ct), Times.Once()); ;
        }

        [Fact]
        public async Task PutMapsCorrectProperties()
        {
            var result = await _sut.PutAsync(1, new AddOrUpdateBeerDto()
            {
                Name = "Test Beer One B",
                PercentageAlcoholByVolume = 100.1m
            });

            var value = result.AssertAndGetOkValue();
            Assert.Equal("Test Beer One B", value.Name);
            Assert.Equal(100.1m, value.PercentageAlcoholByVolume);

            CancellationToken ct = default;
            _beerbaseContextMock.Verify(b => b.SaveChangesAsync(ct), Times.Once()); ;
        }

        [Fact]
        public async Task GetReturnsNotFound()
        {
            _beerbaseContextMock
                .Setup(x => x.Beers)
                .ReturnsDbSet(new Beer[] { });

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
        public void GetFiltersByPercentageAlcoholVolumeGt()
        {
            var result = _sut.Get(10, null);
            var values = result.AssertAndGetOkValue();

            Assert.Equal(2, values.Count());
            Assert.Equal("Test Beer Two", values.First().Name);
            Assert.Equal("Test Beer Three", values.Skip(1).First().Name);
        }

        [Fact]
        public void GetFiltersByPercentageAlcoholVolumeLt()
        {
            var result = _sut.Get(null, 20);
            var values = result.AssertAndGetOkValue();

            Assert.Single(values);
            Assert.Equal("Test Beer One", values.First().Name);
        }
    }
}