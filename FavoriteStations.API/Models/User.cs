using System.Security.Claims;

namespace FavoriteStations.Models {
    public class User : IUser {
        public User(ClaimsPrincipal claimsPrincipal) {
            this.Subject = claimsPrincipal.FindFirstValue("sub");
            this.Issuer = claimsPrincipal.FindFirstValue("iss");
        }
        public readonly string Subject;
        public readonly string Issuer;

        // https://openid.net/specs/openid-connect-core-1_0.html#ClaimStability
        public string UniqueId {
            get {
                return $"{this.Subject}|{this.Issuer}";
            }
        }
    }
}