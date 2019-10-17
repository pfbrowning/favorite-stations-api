using System.Collections.Generic;
using Xunit;
using FavoriteStations.Mapping;
using FavoriteStations.Models;
using FavoriteStations.Services;
using NSubstitute;
using AutoMapper;

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
            var newStation = new StationDTO() { Title = "New Station" };
            // Act: Create the station
            await this.businessLayer.CreateStationAsync(newStation);
            // Assert that the station was created with the provided UserId
            await this.dataLayer.Received(1).CreateStationAsync(Arg.Is<Station>(s => s.UserId == user.Sub));
        }
    }
}
