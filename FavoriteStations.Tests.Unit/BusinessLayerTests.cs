using System.Collections.Generic;
using Xunit;
using FavoriteStations.Mapping;
using FavoriteStations.Models;
using FavoriteStations.Models.Dto;
using FavoriteStations.Models.Entity;
using FavoriteStations.Services;
using FavoriteStations.Models.Response;
using NSubstitute;
using AutoMapper;
using DeepEqual.Syntax;

namespace FavoriteStations.Tests.Unit {
    public class BusinessLayerTests {
        private User user;
        private IBusinessLayer businessLayer;
        private readonly IMapper mapper;
        private readonly IDataLayer dataLayer;
        public BusinessLayerTests() {
            this.user = new User("Dummy Test User");
            var mappingConfig = new MapperConfiguration(mc => {
                mc.AddProfile(new MappingProfile());
            });
            this.mapper = mappingConfig.CreateMapper();
            this.dataLayer = Substitute.For<IDataLayer>();
            this.businessLayer = new BusinessLayer(this.dataLayer, this.user, this.mapper);
        }
        public static IEnumerable<object[]> DummyTestUsers =>
            new List<object[]> {
                new object[] { new User("Test User 1") },
                new object[] { new User("Test User 2") },
                new object[] { new User("Another Test User") }
            };
        public static IEnumerable<object[]> DummyUserMatchEntries =>
            new List<object[]> {
                new object[] { "different user", "current user", false },
                new object[] { "user", "USER", false },
                new object[] { "user", "user", true },
                new object[] { "user     ", "user", false }
            };

        // public static IEnumerable<object[]> DummyNewStationDtos =>
        //     new List<object[]> {
        //         new object[] { new StationDTO() {
        //             Title = "Station 1",
        //             Url = "station.com",
        //             IconUrl = "icon.com",
        //             Notes = "This is a really cool station"
        //         }},
        //         new object[] { new StationDTO() {
        //             Title = "Station 2",
        //             Url = "anotherstation.com",
        //             IconUrl = "anothericon.com",
        //             Notes = "This station isn't quite as good"
        //         }}
        //     };
            
        [Theory]
        [MemberData(nameof(DummyTestUsers))]
        public async void SouldGetAllStationsForCurrentUser(User user) {
            // Arrange: Init the business layer with the provided user
            this.businessLayer = new BusinessLayer(this.dataLayer, user, this.mapper);
            // Act: Perform GetAllStations
            await this.businessLayer.GetAllStations();
            // Assert that the proper user id was passed to GetAllStationsForUserAsync
            await this.dataLayer.Received(1).GetAllStationsForUserAsync(user.Sub);
        }

            
        [Theory]
        [MemberData(nameof(DummyTestUsers))]
        public async void ShouldApplyTheCurrentUserIdToNewStations(User user) {
            // Arrange: Init the businesslayer with the provided user & create a dummy StationDTO
            this.businessLayer = new BusinessLayer(this.dataLayer, user, this.mapper);
            var newStation = new StationCreateUpdateDto() { Title = "New Station" };
            // Act: Create the station
            await this.businessLayer.CreateStationAsync(newStation);
            // Assert that the station was created with the provided UserId
            await this.dataLayer.Received(1).CreateStationAsync(Arg.Is<Station>(s => s.UserId == user.Sub));
        }

        [Fact]
        [MemberData(nameof(DummyTestUsers))]
        public async void GetStationAsyncShouldReturnResourceDoesNotExistWhenResourceNotFound() {
            // Arrange: Configure GetStationAsync to return null
            this.dataLayer.GetStationAsync(Arg.Any<int>()).Returns((Station)null);

            // Act: Run GetStationAsync
            var response = await this.businessLayer.GetStationAsync(1);

            // Assert that the response is ResourceDoesNotExist
            Assert.True(response.IsLeft);
            Assert.Equal(BusinessOperationFailureReason.ResourceDoesNotExist, response.LeftValue);
        }

        [Theory]
        [MemberData(nameof(DummyUserMatchEntries))]
        public async void GetStationAsyncShouldReturnResourceDoesNotBelongToCurrentUserWhenNecessesary(string returnedUser, string currentUser, bool shouldMatch) {
            // Arrange
            // Configure GetStationAsync to return the provided returnedUser
            this.dataLayer.GetStationAsync(Arg.Any<int>()).Returns(new Station() { UserId = returnedUser });
            // Create the provided currentUser
            this.user = new User(currentUser);
            // Re-initialize the business layer so that it takes the new user object
            this.businessLayer = new BusinessLayer(this.dataLayer, this.user, this.mapper);

            // Act: Run GetStationAsync
            var response = await this.businessLayer.GetStationAsync(1);

            /* Assert that the we're returning either ResourceDoesNotBelongToCurrentUser or a Station
            based on the shouldMatch input. */
            if(!shouldMatch) {
                Assert.True(response.IsLeft);
                Assert.Equal(BusinessOperationFailureReason.ResourceDoesNotBelongToCurrentUser, response.LeftValue);
            } else {
                Assert.True(response.IsRight);
            }
        }

        [Fact]
        public async void GetStationAsyncShouldReturnTheStationWhenAllIsWell() {
            // Arrange
            // Configure GetStationAsync to return a Station for user "user"
            var station = new Station() { UserId = "user" };
            this.dataLayer.GetStationAsync(Arg.Any<int>()).Returns(station);
            // Set the current user with sub of "user"
            this.user = new User("user");
            // Re-initialize the business layer so that it takes the new user object
            this.businessLayer = new BusinessLayer(this.dataLayer, this.user, this.mapper);

            // Act: Run GetStationAsync
            var response = await this.businessLayer.GetStationAsync(1);

            // Assert that the station is successfully mapped and returned 
            Assert.True(response.IsRight);
            response.RightValue.ShouldDeepEqual(this.mapper.Map<StationDto>(station));
        }

        [Fact]
        public async void UpdateStationAsyncShouldReturnResourceDoesNotExistWhenResourceNotFound() {
            // Arrange: Configure GetStationAsync to return null
            this.dataLayer.GetStationAsync(Arg.Any<int>()).Returns((Station)null);

            // Act: Run UpdateStationAsync
            var response = await this.businessLayer.UpdateStationAsync(new StationCreateUpdateDto(), 1);

            // Assert that the response is ResourceDoesNotExist
            Assert.True(response.IsLeft);
            Assert.Equal(BusinessOperationFailureReason.ResourceDoesNotExist, response.LeftValue);
        }

        [Theory]
        [MemberData(nameof(DummyUserMatchEntries))]
        public async void UpdateStationAsyncShouldReturnResourceDoesNotBelongToCurrentUserWhenNecessesary(string returnedUser, string currentUser, bool shouldMatch) {
            // Arrange
            // Configure GetStationAsync to return the provided returnedUser
            this.dataLayer.GetStationAsync(Arg.Any<int>()).Returns(new Station() { UserId = returnedUser });
            // Create the provided currentUser
            this.user = new User(currentUser);
            // Re-initialize the business layer so that it takes the new user object
            this.businessLayer = new BusinessLayer(this.dataLayer, this.user, this.mapper);

            // Act: Run UpdateStationAsync
            var response = await this.businessLayer.GetStationAsync(1);

            /* Assert that the we're returning either ResourceDoesNotBelongToCurrentUser or a Station
            based on the shouldMatch input. */
            if(!shouldMatch) {
                Assert.True(response.IsLeft);
                Assert.Equal(BusinessOperationFailureReason.ResourceDoesNotBelongToCurrentUser, response.LeftValue);
            } else {
                Assert.True(response.IsRight);
            }
        }

        [Fact]
        public async void UpdateStationAsyncShouldProperlyUpdateTheStation() {
            // Arrange
            // Init a dummy original station and return it when GetStationAsync is called
            Station originalStation = new Station() {
                StationId = 1,
                UserId = "Dummy Test User",
                Title = "Original Title",
                Url = "Original URL",
                IconUrl = "Original Icon URL",
                Notes = "Original Notes"
            };
            this.dataLayer.GetStationAsync(1).Returns(originalStation);
            // Init a dummy station update
            StationCreateUpdateDto newStation = new StationCreateUpdateDto() {
                Title = "New Title",
                Url = "New URL",
                IconUrl = "New Icon URL",
                Notes = "New Notes"
            };

            // Act: Update the station
            var result = this.businessLayer.UpdateStationAsync(newStation, 1);

            /* Assert that the updated station model passed to the data layer
            has the original, unmodified StationId and UserId, along with the new
            values for all of the other properties. */
            await this.dataLayer.Received(1).UpdateStationAsync(Arg.Is<Station>(u =>
                u.StationId == originalStation.StationId && 
                u.UserId == originalStation.UserId &&
                u.Title == newStation.Title &&
                u.Url == newStation.Url &&
                u.IconUrl == newStation.IconUrl &&
                u.Notes == newStation.Notes
            ));
        }

        [Fact]
        public async void UpdateStationAsyncShouldReturnTheUpdatedStation() {
            // Arrange
            // Init a dummy original station and return it when GetStationAsync is called
            Station originalStation = new Station() {
                StationId = 1,
                UserId = "Dummy Test User",
                Title = "Original Title",
                Url = "Original URL",
                IconUrl = "Original Icon URL",
                Notes = "Original Notes"
            };
            this.dataLayer.GetStationAsync(1).Returns(originalStation);
            // Init a dummy "updated" station and return it from UpdateStationAsync
            Station updatedStation = new Station() {
                StationId = 2,
                Title = "Updated Title",
                Url = "Updated URL",
                IconUrl = "Updated Icon URL",
                Notes = "Updated Notes"
            };
            this.dataLayer.UpdateStationAsync(Arg.Any<Station>()).Returns(updatedStation);

            // Act: Update the station
            var result = await this.businessLayer.UpdateStationAsync(new StationCreateUpdateDto(), 1);

            // Assert that the returned station object is the mapped updatedStation
            Assert.True(result.IsRight);
            this.mapper.Map<StationDto>(updatedStation).ShouldDeepEqual(result.RightValue);
        }

        [Fact]
        [MemberData(nameof(DummyTestUsers))]
        public async void DeleteStationAsyncShouldReturnResourceDoesNotExistWhenResourceNotFound() {
            // Arrange: Configure GetStationAsync to return null
            this.dataLayer.GetStationAsync(Arg.Any<int>()).Returns((Station)null);

            // Act: Run DeleteStationAsync
            var response = await this.businessLayer.DeleteStationAsync(1);

            // Assert that the response is ResourceDoesNotExist
            Assert.True(response.IsLeft);
            Assert.Equal(BusinessOperationFailureReason.ResourceDoesNotExist, response.LeftValue);
        }

        [Theory]
        [MemberData(nameof(DummyUserMatchEntries))]
        public async void DeleteStationAsyncShouldReturnResourceDoesNotBelongToCurrentUserWhenNecessesary(string returnedUser, string currentUser, bool shouldMatch) {
            // Arrange
            // Configure GetStationAsync to return the provided returnedUser
            this.dataLayer.GetStationAsync(Arg.Any<int>()).Returns(new Station() { UserId = returnedUser });
            // Create the provided currentUser
            this.user = new User(currentUser);
            // Re-initialize the business layer so that it takes the new user object
            this.businessLayer = new BusinessLayer(this.dataLayer, this.user, this.mapper);

            // Act: Run DeleteStationAsync
            var response = await this.businessLayer.DeleteStationAsync(1);

            /* Assert that the we're returning either ResourceDoesNotBelongToCurrentUser or a Station Id
            based on the shouldMatch input. */
            if(!shouldMatch) {
                Assert.True(response.IsLeft);
                Assert.Equal(BusinessOperationFailureReason.ResourceDoesNotBelongToCurrentUser, response.LeftValue);
            } else {
                Assert.True(response.IsRight);
            }
        }

        [Fact]
        public async void DeleteStationAsyncShouldReturnTheStationIdWhenAllIsWell() {
            // Arrange
            // Configure GetStationAsync to return a Station for user "user"
            var station = new Station() { UserId = "user" };
            this.dataLayer.GetStationAsync(Arg.Any<int>()).Returns(station);
            // Set the current user with sub of "user"
            this.user = new User("user");
            // Re-initialize the business layer so that it takes the new user object
            this.businessLayer = new BusinessLayer(this.dataLayer, this.user, this.mapper);

            // Act: Run DeleteStationAsync
            var response = await this.businessLayer.DeleteStationAsync(8675309);

            // Assert that the station id is returned, denoting a successful deletion
            Assert.True(response.IsRight);
            Assert.Equal(8675309, response.RightValue);
        }
    }
}
