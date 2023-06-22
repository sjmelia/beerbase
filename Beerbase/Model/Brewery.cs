namespace Beerbase.Model
{
    public class Brewery
    {
        public int BreweryId { get; set; }

        public string Name { get; set; }

        public ICollection<Beer> Beers { get; set; }
    }
}
