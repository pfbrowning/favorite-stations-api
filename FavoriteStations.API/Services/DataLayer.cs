using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using FavoriteStations.Data;
using FavoriteStations.Models.Entity;
using System.Threading.Tasks;

namespace FavoriteStations.Services {
    public class DataLayer: IDataLayer {
        private readonly FavoriteStationsContext dbContext;
        public DataLayer(FavoriteStationsContext dbContext) {
            this.dbContext = dbContext;
        }
        public async Task<List<Station>> GetAllStationsForUserAsync(string userId) {
            return await this.dbContext.Stations
                .Where(station => station.UserId == userId)
                .ToListAsync();
        }
        public async Task<Station> GetStationAsync(int stationId) {
            return await this.dbContext.Stations
                .Where(station => station.StationId == stationId)
                .SingleOrDefaultAsync();
        }
        public async Task<Station> CreateStationAsync(Station station) {
            this.dbContext.Stations.Add(station);
            await this.dbContext.SaveChangesAsync();
            return station;
        }
        public async Task<Station> UpdateStationAsync(Station station) {
            var change = this.dbContext.Update(station);
            await this.dbContext.SaveChangesAsync();
            return station;
        }
        public async Task<bool> DeleteStationAsync(int stationId) {
            Station station = await this.GetStationAsync(stationId);
            if(station != null) {
                this.dbContext.Stations.Remove(station);
                await this.dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}