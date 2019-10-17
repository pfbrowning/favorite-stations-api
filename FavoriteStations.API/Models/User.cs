using System.Security.Claims;

namespace FavoriteStations.Models {
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