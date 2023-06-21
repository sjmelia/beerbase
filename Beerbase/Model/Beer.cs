namespace Beerbase.Model
{
    public class Beer
    {
        public int BeerId { get; set; }

        public required string Name { get; set; }

        public decimal PercentageAlcoholByVolume { get; set; }

        /// <summary>
        /// The associated brewery.
        /// </summary>
        /// <remarks>
        /// Spec implies is nullable, since a beer can be linked with a brewery non-atomically.
        /// </remarks>
        public Brewery? Brewery { get; set; }

        public required ICollection<Bar> BarsServedAt { get; set; }
    }
}
