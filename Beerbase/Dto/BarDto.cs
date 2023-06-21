using Beerbase.Model;
using System.ComponentModel;

namespace Beerbase.Dto
{
    [DisplayName("Bar")]
    public class BarDto
    {
        public BarDto(Bar bar)
        {
            this.BarId = bar.BarId;
            this.Name = bar.Name;
            this.Address = bar.Address;
        }

        public int BarId { get; private set; }

        public string Name { get; private set; }

        public string Address { get; private set; }
    }
}
