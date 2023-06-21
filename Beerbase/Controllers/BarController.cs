using Beerbase.Dto;
using Beerbase.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Beerbase.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BarController : ControllerBase
    {
        private readonly BeerbaseContext _beerbaseContext;
        private readonly ILogger<BarController> _logger;

        public BarController(ILogger<BarController> logger, BeerbaseContext beerbaseContext)
        {
            _logger = logger;
            _beerbaseContext = beerbaseContext;
        }

        [HttpGet("{id}", Name = "GetBar")]
        public async Task<ActionResult<BarDto>> GetAsync(int id)
        {
            var bar = await _beerbaseContext.Bars.SingleOrDefaultAsync(b => b.BarId == id);

            if (bar == null)
            {
                return NotFound();
            }

            return Ok(new BarDto(bar));
        }

        [HttpGet(Name = "GetBars")]
        public ActionResult<IEnumerable<BarDto>> Get()
        {
            var bars = _beerbaseContext.Bars.Select(b => new BarDto(b));
            return Ok(bars);
        }

        [HttpPost(Name = "PostBar")]
        public async Task<ActionResult<BarDto>> PostAsync(AddOrUpdateBarDto barDto)
        {
            var bar = new Bar()
            {
                Name = barDto.Name,
                Address = barDto.Address,
                BeersServed = Array.Empty<Beer>()
            };

            await _beerbaseContext.Bars.AddAsync(bar);
            await _beerbaseContext.SaveChangesAsync();
            return Ok(new BarDto(bar));
        }

        [HttpPut("{id}", Name = "PutBar")]
        public async Task<ActionResult<BarDto>> PutAsync(int id, AddOrUpdateBarDto barDto)
        {
            var bar = await _beerbaseContext.Bars.SingleOrDefaultAsync(b => b.BarId == id);

            if (bar == null)
            {
                return NotFound();
            }

            bar.Name = barDto.Name;
            await _beerbaseContext.SaveChangesAsync();
            return Ok(new BarDto(bar));
        }
    }
}