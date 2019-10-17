using System.Collections.Generic;
using FavoriteStations.Models;
using FavoriteStations.Models.Response;
using System.Threading.Tasks;
using AutoMapper;
using Here;

namespace FavoriteStations.Services {
    public class BusinessLayer: IBusinessLayer {
        private readonly IDataLayer dataLayer;
        private readonly User user;
        private readonly IMapper mapper;
        public BusinessLayer(IDataLayer dataLayer, User user, IMapper mapper) {
            this.dataLayer = dataLayer;
            this.user = user;
            this.mapper = mapper;
        }
        public async Task<List<StationDTO>> GetAllStations() {
            var stations = await this.dataLayer.GetAllStationsForUserAsync(this.user.Sub);
            return this.mapper.Map<List<StationDTO>>(stations);
        }
        public async Task<Either<BusinessOperationFailureReason, StationDTO>> GetStationAsync(int stationId) {
            // Retrieve the station from the database
            var station = await this.dataLayer.GetStationAsync(stationId);
            // Check to see if the station wasn't found or is associated with a different user
            if(station == null) {
                return Either.Left(BusinessOperationFailureReason.ResourceDoesNotExist);
            }
            if(station.UserId != this.user.Sub) {
                return Either.Left(BusinessOperationFailureReason.ResourceDoesNotBelongToCurrentUser);
            }
            // If the station was found and is associated with the current user, then map it and return it
            else {
                return Either.Right(this.mapper.Map<StationDTO>(station));
            }
        }
        public async Task<StationDTO> CreateStationAsync(StationDTO station) {
            // Map the DTO to an entity model and add the current user's id
            var mapped = this.mapper.Map<Station>(station);
            mapped.UserId = this.user.Sub;
            // Create the new station in the DB
            var newStation = await this.dataLayer.CreateStationAsync(mapped);
            // Map the created station to a DTO and return it
            return this.mapper.Map<StationDTO>(newStation);
        }
        public async Task<Either<BusinessOperationFailureReason, StationDTO>> UpdateStationAsync(StationDTO station, int stationId) {
            // Retrieve the station from the database
            var existing = await this.dataLayer.GetStationAsync(stationId);
            // Check to see if the station wasn't found or is associated with a different user
            if(existing == null) {
                return Either.Left(BusinessOperationFailureReason.ResourceDoesNotExist);
            }
            else if(existing.UserId != this.user.Sub) {
                return Either.Left(BusinessOperationFailureReason.ResourceDoesNotBelongToCurrentUser);
            }
            // Apply the values from the provided DTO to the existing entity model
            this.mapper.Map<StationDTO, Station>(station, existing);
            // Persist the updated entity model to the DB
            var updated = await this.dataLayer.UpdateStationAsync(existing);
            // Map and return the updated station model
            return Either.Right(this.mapper.Map<StationDTO>(updated));
        }
        public async Task<Either<BusinessOperationFailureReason, int>> DeleteStationAsync(int stationId) {
            // Retrieve the station from the DB
            var existing = await this.dataLayer.GetStationAsync(stationId);
            // Check to see if the station wasn't found or is associated with a different user
            if(existing == null) {
                return Either.Left(BusinessOperationFailureReason.ResourceDoesNotExist);
            }
            else if(existing.UserId != this.user.Sub) {
                return Either.Left(BusinessOperationFailureReason.ResourceDoesNotBelongToCurrentUser);
            }
            // Delete the station
            await this.dataLayer.DeleteStationAsync(stationId);
            // Return the stationId to signify that it was found and successfully deleted
            return Either.Right(stationId);
        }
    }
}