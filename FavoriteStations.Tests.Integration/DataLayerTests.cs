using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Microsoft.EntityFrameworkCore;
using FavoriteStations.Data;
using FavoriteStations.Models;
using FavoriteStations.Services;

namespace FavoriteStations.Tests.Integration {
    public class DataLayerTests {
        private readonly DataLayerService dataLayer;
        private readonly FavoriteStationsContext dbContext;
        public DataLayerTests() {
            var builder = new DbContextOptionsBuilder<FavoriteStationsContext>();
            builder.UseSqlServer(@"Server=localhost\SQLEXPRESS;Database=favorite-stations;Trusted_Connection=True;");
            this.dbContext = new FavoriteStationsContext(builder.Options);
            this.dataLayer = new DataLayerService(dbContext);
        }
        [Fact]
        public async void ShouldCreateRetrieveAndDeleteAStation() {
            Station station = new Station() {
                UserId = "Dummy User",
                Title = "Station Title",
                Url = "station.com",
                IconUrl = "icon.com",
                Notes = "Dummy notes"
            };
            // Create a dummy station to ensure that create is working
            Station createdStation = await this.dataLayer.CreateStationAsync(station);
            // Retrieve the created station to check that both the create and retrieve are working
            Assert.NotNull(await this.dataLayer.GetStationAsync(createdStation.StationId.Value));
            // Delete the station for cleanup
            Assert.True(await this.dataLayer.DeleteStationAsync(createdStation.StationId.Value));
        }

        [Fact]
        public async void ShouldUpdateAStation() {
            // Arrange: Create a dummy station and then retrieve it to update
            Station station = new Station() {
                UserId = "Dummy User",
                Title = "Station Title",
                Url = "station.com",
                IconUrl = "icon.com",
                Notes = "Dummy notes"
            };
            Station created = await this.dataLayer.CreateStationAsync(station);
            Station retrieved = await this.dataLayer.GetStationAsync(created.StationId.Value);
            Assert.Equal("Station Title", retrieved.Title);
            // Act: Update the retrieved station and then retrieve the updated entry
            retrieved.Title = "Updated Title";
            retrieved.Url = "Updated Url";
            retrieved.IconUrl = "Updated Icon";
            retrieved.Notes = "Updated Notes";
            Station updated = await this.dataLayer.UpdateStationAsync(station);
            Station updateRetrieved = await this.dataLayer.GetStationAsync(created.StationId.Value);

            // Assert that the retrieved station matches the updated values
            Assert.Equal("Updated Title", updateRetrieved.Title);
            Assert.Equal("Updated Url", updateRetrieved.Url);
            Assert.Equal("Updated Icon", updateRetrieved.IconUrl);
            Assert.Equal("Updated Notes", updateRetrieved.Notes);

            // Cleanup
            await this.dataLayer.DeleteStationAsync(created.StationId.Value);
        }

        [Fact]
        public async void ShouldGetAllStationsForUser() {
            /* Arrange: Create 10 stations for the dummy test user and
            one station for another user */
            List<Station> userStationsCreated = new List<Station>();
            string testUserId = "ShouldGetAllStationsForUserTest";
            for (int i = 0; i < 10; i++) {
                Station station = new Station() {
                    UserId = testUserId,
                    Url = $"station{i}.com"
                };
                userStationsCreated.Add(await this.dataLayer.CreateStationAsync(station));
            }
            Station otherStation = new Station() {
                UserId = "another user",
                Url = "another url"
            };
            await this.dataLayer.CreateStationAsync(otherStation);

            // Act: Get all stations for the dummy test user
            var stationsForUser = await this.dataLayer.GetAllStationsForUserAsync(testUserId);

            /* Assert that the stations returned by GetAllStationsForUserAsync match 
            the stations created for the dummy test user */
            var stationsForUserIds = stationsForUser.Select(station => station.StationId.Value).ToList();
            var createdUserStationIds = userStationsCreated.Select(station => station.StationId.Value).ToList();
            Assert.Equal<List<int>>(createdUserStationIds, stationsForUserIds);
            Assert.Equal(10, stationsForUserIds.Count);

            // Cleanup
            this.dbContext.Remove(otherStation);
            this.dbContext.RemoveRange(userStationsCreated);
            this.dbContext.SaveChanges();
        }

        [Fact]
        public async void ShouldFailWhenCreatingAStationWithIdProvided() {
            // Arrange: Create a dummy station with an id
            Station station = new Station() {
                StationId = 8675309,
                UserId = "Dummy ID",
                Url = "Dummy URL"
            };

            // Act & Assert: Attempt to create the Station and expect that it fails
            await Assert.ThrowsAsync<Microsoft.EntityFrameworkCore.DbUpdateException>(async () => await this.dataLayer.CreateStationAsync(station));
        }

        [Fact]
        public async void ShouldFailWhenUpdatingAStationWithNoId() {
            // Arrange: Create a dummy station with no id
            Station station = new Station() {
                UserId = "Dummy ID",
                Url = "Dummy URL"
            };
            Assert.Null(station.StationId);

            // Act: Pass the station to UpdateStationAsync
            await this.dataLayer.UpdateStationAsync(station);

            // Assert that a new station was created
            Assert.NotNull(station.StationId);

            // Cleanup
            await this.dataLayer.DeleteStationAsync(station.StationId.Value);
        }
    }
}
