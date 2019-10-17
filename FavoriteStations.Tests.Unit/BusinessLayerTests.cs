using Xunit;
using FavoriteStations.Mapping;
using FavoriteStations.Models;
using FavoriteStations.Services;
using NSubstitute;
using AutoMapper;

namespace FavoriteStations.Tests.Unit {
    public class BusinessLayerTests {
        private readonly User user;
        private readonly IMapper mapper;
        private readonly IDataLayer dataLayer;
        private readonly IBusinessLayer businessLayer;
        public BusinessLayerTests() {
            this.user = new User("Dummy Test User");
            var mappingConfig = new MapperConfiguration(mc => {
                mc.AddProfile(new MappingProfile());
            });
            this.mapper = mappingConfig.CreateMapper();
            this.dataLayer = Substitute.For<IDataLayer>();
            this.businessLayer = new BusinessLayer(this.dataLayer, this.user, this.mapper);
        }
        [Fact]
        public void SouldGetAllStationsForCurrentUser() {
            this.businessLayer.GetAllStations();
            this.dataLayer.Received(1).GetAllStationsForUserAsync(this.user.Sub);
        }
    }
}
