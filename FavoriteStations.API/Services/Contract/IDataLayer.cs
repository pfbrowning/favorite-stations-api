using System.Collections.Generic;
using FavoriteStations.Models.Entity;
using System.Threading.Tasks;

namespace FavoriteStations.Services {
    public interface IDataLayer {
        Task<List<Station>> GetAllStationsForUserAsync(string userId);
        Task<Station> GetStationAsync(int stationId);
        Task<Station> CreateStationAsync(Station station);
        Task<Station> UpdateStationAsync(Station station);
        Task<bool> DeleteStationAsync(int stationId);        
    }
}