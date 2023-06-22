namespace Beerbase.Model
{
    public class Bar
    {
        public int BarId { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public ICollection<Beer> BeersServed { get; set; }
    }
}
