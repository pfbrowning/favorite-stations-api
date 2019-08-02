using Microsoft.EntityFrameworkCore;
using FavoriteStations.Models;

namespace FavoriteStations.Data {
    public class FavoriteStationsContext : DbContext {
        public DbSet<Station> Stations { get; set; }
        public FavoriteStationsContext(DbContextOptions<FavoriteStationsContext> options): base(options){ }
    }
}