using Beerbase.Dto;
using Beerbase.Model;
using Microsoft.AspNetCore.Connections.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Beerbase.Controllers
{
    [ApiController]
    [Route("/")]
    public class BarBeerController : ControllerBase
    {
        private readonly BeerbaseContext _beerbaseContext;
        private readonly ILogger<BarBeerController> _logger;

        public BarBeerController(ILogger<BarBeerController> logger, BeerbaseContext beerbaseContext)
        {
            _logger = logger;
            _beerbaseContext = beerbaseContext;
        }

        [HttpGet("bar/{id}/beer", Name = "GetBarBeers")]
        public async Task<ActionResult<BarBeerDto>> GetAsync(int id)
        {
            var bar = await _beerbaseContext
                .Bars
                .Include(b => b.BeersServed)
                .SingleOrDefaultAsync(b => b.BarId == id);

            if (bar == null)
            {
                return NotFound();
            }

            return Ok(new BarBeerDto(bar));
        }

        [HttpGet("bar/beer", Name = "GetBarsBeers")]
        public ActionResult<IEnumerable<BarBeerDto>> Get()
        {
            var barBeers = _beerbaseContext
                .Bars
                .Include(b => b.BeersServed)
                .Select(b => new BarBeerDto(b));
            return Ok(barBeers);
        }

        [HttpPost("bar/beer", Name = "PostBarBeer")]
        public async Task<ActionResult<BarBeerDto>> PostAsync(AddOrUpdateBarBeerDto barBeerDto)
        {
            var bar = await _beerbaseContext.Bars.SingleOrDefaultAsync(b => b.BarId == barBeerDto.BarId);
            var beer = await _beerbaseContext.Beers.SingleOrDefaultAsync(b => b.BeerId == barBeerDto.BeerId);

            if (bar == null || beer == null)
            {
                return NotFound();
            }

            if (bar.BeersServed == null)
            {
                bar.BeersServed = new List<Beer>();
            }

            bar.BeersServed.Add(beer);
            await _beerbaseContext.SaveChangesAsync();
            return Ok(new BarBeerDto(bar));
        }
    }
}