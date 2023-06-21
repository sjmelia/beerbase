namespace Beerbase.Model
{
    public class Brewery
    {
        public int BreweryId { get; set; }

        public required string Name { get; set; }

        public required ICollection<Beer> Beers { get; set; }
    }
}
