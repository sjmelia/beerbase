using Beerbase.Model;
using System.ComponentModel;

namespace Beerbase.Dto
{
    [DisplayName("Beer")]
    public class BeerDto
    {
        public BeerDto(Beer beer)
        {
            this.BeerId = beer.BeerId;
            this.Name = beer.Name;
            this.PercentageAlcoholByVolume = beer.PercentageAlcoholByVolume;
        }

        public int BeerId { get; private set; }

        public string Name { get; private set; }

        public decimal PercentageAlcoholByVolume { get; private set; }
    }
}
