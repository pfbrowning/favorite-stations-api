using System.Diagnostics.CodeAnalysis;

namespace FavoriteStations.Config {
    [ExcludeFromCodeCoverage]
    public class AuthConfig {
        public string Authority { get; set; }
        public string Audience { get; set; }
    }
}