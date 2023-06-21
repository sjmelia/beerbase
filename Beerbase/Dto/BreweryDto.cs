using Beerbase.Model;
using System.ComponentModel;

namespace Beerbase.Dto
{
    [DisplayName("Brewery")]
    public class BreweryDto
    {
        public BreweryDto(Brewery brewery)
        {
            this.BreweryId = brewery.BreweryId;
            this.Name = brewery.Name;
        }

        public int BreweryId { get; private set; }

        public string Name { get; private set; }
    }
}
