using Beerbase.Dto;
using Beerbase.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Beerbase.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BreweryController : ControllerBase
    {
        private readonly BeerbaseContext _beerbaseContext;
        private readonly ILogger<BreweryController> _logger;

        public BreweryController(ILogger<BreweryController> logger, BeerbaseContext beerbaseContext)
        {
            _logger = logger;
            _beerbaseContext = beerbaseContext;
        }

        [HttpGet("{id}", Name = "GetBrewery")]
        public async Task<ActionResult<BreweryDto>> GetAsync(int id)
        {
            var brewery = await _beerbaseContext.Breweries.SingleOrDefaultAsync(b => b.BreweryId == id);

            if (brewery == null)
            {
                return NotFound();
            }

            return Ok(new BreweryDto(brewery));
        }

        [HttpGet(Name = "GetBreweries")]
        public ActionResult<IEnumerable<BreweryDto>> Get()
        {
            var breweries = _beerbaseContext.Breweries.Select(b => new BreweryDto(b));
            return Ok(breweries);
        }

        [HttpPost(Name = "PostBrewery")]
        public async Task<ActionResult<BreweryDto>> PostAsync(AddOrUpdateBreweryDto breweryDto)
        {
            var brewery = new Brewery()
            {
                Name = breweryDto.Name,
                Beers = Array.Empty<Beer>()
            };

            await _beerbaseContext.Breweries.AddAsync(brewery);
            await _beerbaseContext.SaveChangesAsync();
            return Ok(new BreweryDto(brewery));
        }

        [HttpPut("{id}", Name = "PutBrewery")]
        public async Task<ActionResult<BreweryDto>> PutAsync(int id, AddOrUpdateBreweryDto breweryDto)
        {
            var brewery = await _beerbaseContext.Breweries.SingleOrDefaultAsync(b => b.BreweryId == id);

            if (brewery == null)
            {
                return NotFound();
            }

            brewery.Name = breweryDto.Name;
            await _beerbaseContext.SaveChangesAsync();
            return Ok(new BreweryDto(brewery));
        }
    }
}