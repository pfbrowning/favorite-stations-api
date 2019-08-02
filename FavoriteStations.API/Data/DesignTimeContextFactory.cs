using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Design;

namespace FavoriteStations.Data {
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<FavoriteStationsContext> {
        public FavoriteStationsContext CreateDbContext(string[] args) {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<FavoriteStationsContext>();
            builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            return new FavoriteStationsContext(builder.Options);
        }
    }
}