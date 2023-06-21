using Microsoft.EntityFrameworkCore;

namespace Beerbase.Model
{
    public class BeerbaseContext : DbContext
    {
        public virtual DbSet<Beer> Beers { get; set; }

        public virtual DbSet<Brewery> Breweries { get; set; }

        public virtual DbSet<Bar> Bars { get; set; }

        private readonly string path;

        public BeerbaseContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            this.path = Path.Join(path, "beerbase.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={this.path}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Beer>()
                .HasKey(e => e.BeerId);

            modelBuilder.Entity<Bar>()
                .HasKey(e => e.BarId);

            modelBuilder.Entity<Brewery>()
                .HasKey(e => e.BreweryId);

            // - Breweries can have many beers
            modelBuilder.Entity<Brewery>()
                .HasMany(e => e.Beers)
                .WithOne(e => e.Brewery);

            // - Bars can serve many types of beers
            modelBuilder.Entity<Bar>()
                .HasMany(e => e.BeersServed)
                .WithMany(e => e.BarsServedAt);

            base.OnModelCreating(modelBuilder);
        }
    }
}
