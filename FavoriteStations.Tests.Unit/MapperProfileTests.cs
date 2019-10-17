using System.Collections.Generic;
using Xunit;
using FavoriteStations.Mapping;
using FavoriteStations.Models;
using AutoMapper;

namespace FavoriteStations.Tests.Unit {
    public class MapperProfileTests {
        private readonly IMapper mapper;
        public MapperProfileTests() {
            var mappingConfig = new MapperConfiguration(mc => {
                mc.AddProfile(new MappingProfile());
            });
            this.mapper = mappingConfig.CreateMapper();
        }

        public static IEnumerable<object[]> DummyStationDtos =>
            new List<object[]> {
                new object[] { new StationDTO() {
                    StationId = 1,
                    Title = "Station 1",
                    Url = "station.com",
                    IconUrl = "icon.com",
                    Notes = "This is a really cool station"
                }},
                new object[] { new StationDTO() {
                    StationId = 2,
                    Title = "Station 2",
                    Url = "anotherstation.com",
                    IconUrl = "anothericon.com",
                    Notes = "This station isn't quite as good"
                }}
            };

        public static IEnumerable<object[]> DummyStationEntities =>
            new List<object[]> {
                new object[] { new Station() {
                    StationId = 1,
                    Title = "Station 1",
                    Url = "station.com",
                    IconUrl = "icon.com",
                    Notes = "This is a really cool station"
                }},
                new object[] { new Station() {
                    StationId = 2,
                    Title = "Station 2",
                    Url = "anotherstation.com",
                    IconUrl = "anothericon.com",
                    Notes = "This station isn't quite as good"
                }}
            };

        [Theory]
        [MemberData(nameof(DummyStationDtos))]
        public void ShouldMapStationDTOToStation(StationDTO station) {
            var mapped = this.mapper.Map<Station>(station);
            Assert.Equal(station.StationId, mapped.StationId);
            Assert.Equal(station.Title, mapped.Title);
            Assert.Equal(station.Url, mapped.Url);
            Assert.Equal(station.IconUrl, mapped.IconUrl);
            Assert.Equal(station.Notes, mapped.Notes);            
        }

        [Theory]
        [MemberData(nameof(DummyStationEntities))]
        public void ShouldMapStationToStationDTO(Station station) {
            var mapped = this.mapper.Map<StationDTO>(station);
            Assert.Equal(station.StationId, mapped.StationId);
            Assert.Equal(station.Title, mapped.Title);
            Assert.Equal(station.Url, mapped.Url);
            Assert.Equal(station.IconUrl, mapped.IconUrl);
            Assert.Equal(station.Notes, mapped.Notes);            
        }
    }
}
