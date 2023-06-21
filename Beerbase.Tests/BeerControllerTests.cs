using Beerbase.Controllers;
using Beerbase.Dto;
using Beerbase.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;

namespace Beerbase.Tests
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
            _beerbaseContextMock
                .Setup<DbSet<Beer>>(x => x.Beers)
                .ReturnsDbSet(new Beer[] { });

            _sut = new BeerController(_loggerMock.Object, _beerbaseContextMock.Object);
        }

        [Fact]
        public async Task PostMapsCorrectProperties()
        {
            var result = await _sut.PostAsync(new BeerDto()
            {
                Name = "Test Beer",
                PercentageAlcoholByVolume = 100.1m
            });

            var okResult = result.Result as OkObjectResult;
            Assert.NotNull(okResult);
            var value = okResult.Value as Beer;
            Assert.NotNull(value);
            Assert.Equal("Test Beer", value.Name);
            Assert.Equal(100.1m, value.PercentageAlcoholByVolume);

            CancellationToken ct = default;
            _beerbaseContextMock.Verify(b => b.SaveChangesAsync(ct), Times.Once()); ;
        }

        [Fact]
        public async Task GetReturnsNotFound()
        {
            _beerbaseContextMock
                .Setup<DbSet<Beer>>(x => x.Beers)
                .ReturnsDbSet(new Beer[] { });

            var result = await _sut.GetAsync(1);

            Assert.IsType<NotFoundResult>(result.Result);
        }
    }
}