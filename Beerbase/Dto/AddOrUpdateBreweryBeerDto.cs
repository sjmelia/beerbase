namespace Beerbase.Dto
{
    public class AddOrUpdateBreweryBeerDto
    {
        public required int BeerId { get; set; }

        public required int BreweryId { get; set; }
    }
}
