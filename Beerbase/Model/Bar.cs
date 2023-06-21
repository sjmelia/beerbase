namespace Beerbase.Model
{
    public class Bar
    {
        public int BarId { get; set; }

        public required string Name { get; set; }

        public required string Address { get; set; }

        public required ICollection<Beer> BeersServed { get; set; }
    }
}
