using Microsoft.EntityFrameworkCore;
using FavoriteStations.Models.Entity;

namespace FavoriteStations.Data {
    public class FavoriteStationsContext : DbContext {
        public DbSet<Station> Stations { get; set; }
        public FavoriteStationsContext(DbContextOptions<FavoriteStationsContext> options): base(options){ }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Station>()
                .HasIndex(b => b.UserId);
        }
    }
}