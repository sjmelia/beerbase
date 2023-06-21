using Beerbase.Model;
using System.ComponentModel;

namespace Beerbase.Dto
{
    [DisplayName("BreweryBeer")]
    public class BreweryBeerDto : BreweryDto
    {
        public BreweryBeerDto(Brewery brewery) : base(brewery)
        {
            this.Beers = brewery.Beers == null ? 
                new List<BeerDto>() 
                : brewery.Beers.Select(b => new BeerDto(b));
        }

        public IEnumerable<BeerDto> Beers { get; set; }
    }
}
