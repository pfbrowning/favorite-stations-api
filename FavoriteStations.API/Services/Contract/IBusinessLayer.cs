using System.Collections.Generic;
using System.Threading.Tasks;
using FavoriteStations.Models.Dto;
using FavoriteStations.Models.Response;
using Here;

namespace FavoriteStations.Services {
    public interface IBusinessLayer {
        Task<List<StationDto>> GetAllStations();
        Task<StationDto> CreateStationAsync(StationCreateUpdateDto station);
        Task<Either<BusinessOperationFailureReason, StationDto>> GetStationAsync(int stationId);
        Task<Either<BusinessOperationFailureReason, StationDto>> UpdateStationAsync(StationCreateUpdateDto station, int stationId);
        Task<Either<BusinessOperationFailureReason, int>> DeleteStationAsync(int stationId);        
    }
}