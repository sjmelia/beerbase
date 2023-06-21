using Beerbase.Dto;
using Beerbase.Model;
using Microsoft.AspNetCore.Connections.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Beerbase.Controllers
{
    [ApiController]
    [Route("/")]
    public class BreweryBeerController : ControllerBase
    {
        private readonly BeerbaseContext _beerbaseContext;
        private readonly ILogger<BreweryBeerController> _logger;

        public BreweryBeerController(ILogger<BreweryBeerController> logger, BeerbaseContext beerbaseContext)
        {
            _logger = logger;
            _beerbaseContext = beerbaseContext;
        }

        [HttpGet("brewery/{id}/beer", Name = "GetBreweryBeers")]
        public async Task<ActionResult<BreweryBeerDto>> GetAsync(int id)
        {
            var brewery = await _beerbaseContext
                .Breweries
                .Include(b => b.Beers)
                .SingleOrDefaultAsync(b => b.BreweryId == id);

            if (brewery == null)
            {
                return NotFound();
            }

            return Ok(new BreweryBeerDto(brewery));
        }

        [HttpGet("brewery/beer", Name = "GetBreweriesBeers")]
        public ActionResult<IEnumerable<BreweryBeerDto>> Get()
        {
            var breweryBeers = _beerbaseContext
                .Breweries
                .Include(b => b.Beers)
                .Select(b => new BreweryBeerDto(b));
            return Ok(breweryBeers);
        }

        [HttpPost("brewery/beer", Name = "PostBreweryBeer")]
        public async Task<ActionResult<BreweryBeerDto>> PostAsync(AddOrUpdateBreweryBeerDto breweryBeerDto)
        {
            var brewery = await _beerbaseContext.Breweries.SingleOrDefaultAsync(b => b.BreweryId == breweryBeerDto.BreweryId);
            var beer = await _beerbaseContext.Beers.SingleOrDefaultAsync(b => b.BeerId == breweryBeerDto.BeerId);

            if (brewery == null || beer == null)
            {
                return NotFound();
            }

            if (brewery.Beers == null)
            {
                brewery.Beers = new List<Beer>();
            }

            brewery.Beers.Add(beer);
            await _beerbaseContext.SaveChangesAsync();
            return Ok(new BreweryBeerDto(brewery));
        }
    }
}