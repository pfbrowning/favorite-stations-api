using System.Collections.Generic;
using System.Threading.Tasks;
using FavoriteStations.Models;
using FavoriteStations.Models.Response;
using Here;

namespace FavoriteStations.Services {
    public interface IBusinessLayer {
        Task<List<StationDTO>> GetAllStations();
        Task<StationDTO> CreateStationAsync(StationDTO station);
        Task<Either<BusinessOperationFailureReason, StationDTO>> GetStationAsync(int stationId);
        Task<Either<BusinessOperationFailureReason, StationDTO>> UpdateStationAsync(StationDTO station, int stationId);
        Task<Either<BusinessOperationFailureReason, int>> DeleteStationAsync(int stationId);        
    }
}