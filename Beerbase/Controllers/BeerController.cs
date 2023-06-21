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
        public async Task<ActionResult<Beer>> GetAsync(int id)
        {
            var beer = await _beerbaseContext.Beers.SingleOrDefaultAsync(b => b.BeerId == id);

            if (beer == null)
            {
                return NotFound();
            }

            return beer;
        }

        [HttpGet(Name = "GetBeers")]
        public async Task<ActionResult<IEnumerable<Beer>>> GetAsync(decimal? gtAlcoholByVolume, decimal? ltAlcoholByVolume)
        {
            var beersQueryable = _beerbaseContext.Beers.AsQueryable();

            if (gtAlcoholByVolume != null)
            {
                beersQueryable = beersQueryable.Where(b => b.PercentageAlcoholByVolume > gtAlcoholByVolume);
            }

            if (ltAlcoholByVolume != null)
            {
                beersQueryable = beersQueryable.Where(b => b.PercentageAlcoholByVolume < ltAlcoholByVolume);
            }

            var beers = await beersQueryable.ToArrayAsync();
            return beers;
        }

        [HttpPost(Name = "PostBeer")]
        public async Task<ActionResult<Beer>> PostAsync(BeerDto beerDto)
        {
            var beer = new Beer()
            {
                Name = beerDto.Name,
                PercentageAlcoholByVolume = beerDto.PercentageAlcoholByVolume,
                Brewery = null,
                BarsServedAt = new Bar[] { }
            };

            await _beerbaseContext.Beers.AddAsync(beer);
            await _beerbaseContext.SaveChangesAsync();
            return Ok(beer);
        }

        [HttpPut("{id}", Name = "PutBeer")]
        public async Task<ActionResult<Beer>> PutAsync(int id, BeerDto beerDto)
        {
            var beer = await _beerbaseContext.Beers.SingleOrDefaultAsync(b => b.BeerId == id);

            if (beer == null)
            {
                return NotFound();
            }

            beer.Name = beerDto.Name;
            beer.PercentageAlcoholByVolume = beerDto.PercentageAlcoholByVolume;
            await _beerbaseContext.SaveChangesAsync();
            return beer;
        }
    }
}