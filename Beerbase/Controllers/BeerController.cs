using Beerbase.Dto;
using Beerbase.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Beerbase.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BeerController : ControllerBase
    {
        private readonly BeerbaseContext _beerbaseContext;
        private readonly ILogger<BeerController> _logger;

        public BeerController(ILogger<BeerController> logger, BeerbaseContext beerbaseContext)
        {
            _logger = logger;
            _beerbaseContext = beerbaseContext;
        }

        [HttpGet("{id}", Name = "GetBeer")]
        public async Task<ActionResult<BeerDto>> GetAsync(int id)
        {
            var beer = await _beerbaseContext.Beers.SingleOrDefaultAsync(b => b.BeerId == id);

            if (beer == null)
            {
                return NotFound();
            }

            return Ok(new BeerDto(beer));
        }

        [HttpGet(Name = "GetBeers")]
        public ActionResult<IEnumerable<BeerDto>> Get(decimal? gtAlcoholByVolume, decimal? ltAlcoholByVolume)
        {
            IQueryable<Beer> beers = _beerbaseContext.Beers;

            if (gtAlcoholByVolume != null)
            {
                beers = beers.Where(b => b.PercentageAlcoholByVolume > gtAlcoholByVolume);
            }

            if (ltAlcoholByVolume != null)
            {
                beers = beers.Where(b => b.PercentageAlcoholByVolume < ltAlcoholByVolume);
            }

            var beerDtos = beers.Select(b => new BeerDto(b));

            return Ok(beerDtos);
        }

        [HttpPost(Name = "PostBeer")]
        public async Task<ActionResult<BeerDto>> PostAsync(AddOrUpdateBeerDto beerDto)
        {
            // Check if beer exists with same name? See README
            var beerExistsWithSameName = _beerbaseContext.Beers.Any(b => b.Name == beerDto.Name);
            if (beerExistsWithSameName)
            {
                return Conflict();
            }

            var beer = new Beer()
            {
                Name = beerDto.Name,
                PercentageAlcoholByVolume = beerDto.PercentageAlcoholByVolume,
                Brewery = null,
                BarsServedAt = Array.Empty<Bar>()
            };

            await _beerbaseContext.Beers.AddAsync(beer);
            await _beerbaseContext.SaveChangesAsync();
            return Ok(new BeerDto(beer));
        }

        [HttpPut("{id}", Name = "PutBeer")]
        public async Task<ActionResult<BeerDto>> PutAsync(int id, AddOrUpdateBeerDto beerDto)
        {
            var beer = await _beerbaseContext.Beers.SingleOrDefaultAsync(b => b.BeerId == id);

            if (beer == null)
            {
                return NotFound();
            }

            beer.Name = beerDto.Name;
            beer.PercentageAlcoholByVolume = beerDto.PercentageAlcoholByVolume;
            await _beerbaseContext.SaveChangesAsync();
            return Ok(new BeerDto(beer));
        }
    }
}