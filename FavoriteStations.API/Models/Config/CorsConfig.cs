using System.Diagnostics.CodeAnalysis;

namespace FavoriteStations.Config {
    [ExcludeFromCodeCoverage]
    public class CorsConfig {
        public string[] AllowedCorsOrigins { get; set; }
    }
}