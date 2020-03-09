namespace FavoriteStations.Models {
    public class UserStub : IUser {
        private string _UniqueId;
        public UserStub(string id) {
            this._UniqueId = id;
        }
        public string UniqueId {
            get {
                return this._UniqueId;
            }
        }
    }
}