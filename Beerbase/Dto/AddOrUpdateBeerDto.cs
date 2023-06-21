namespace Beerbase.Dto
{
    public class AddOrUpdateBeerDto
    {
        public required string Name { get; set; }

        public required decimal PercentageAlcoholByVolume { get; set; }
    }
}
