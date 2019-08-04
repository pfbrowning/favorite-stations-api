using System.Security.Claims;

namespace FavoriteStations.Models {
    public class User {
        public User(ClaimsPrincipal claimsPrincipal) {
            this.Sub = claimsPrincipal.FindFirstValue("sub");
        }
        public readonly string Sub;
    }
}