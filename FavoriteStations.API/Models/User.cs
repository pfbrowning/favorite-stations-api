using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace FavoriteStations.Models {
    [ExcludeFromCodeCoverage]
    public class User {
        public User(string sub) {
            this.Sub = sub;
        }
        public User(ClaimsPrincipal claimsPrincipal) {
            this.Sub = claimsPrincipal.FindFirstValue("sub");
        }
        public readonly string Sub;
    }
}