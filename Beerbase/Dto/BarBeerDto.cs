using Beerbase.Model;
using System.ComponentModel;

namespace Beerbase.Dto
{
    [DisplayName("BarBeer")]
    public class BarBeerDto : BarDto
    {
        public BarBeerDto(Bar bar) : base(bar)
        {
            this.BeersServed = bar.BeersServed == null ? 
                new List<BeerDto>() 
                : bar.BeersServed.Select(b => new BeerDto(b));
        }

        public IEnumerable<BeerDto> BeersServed { get; set; }
    }
}
