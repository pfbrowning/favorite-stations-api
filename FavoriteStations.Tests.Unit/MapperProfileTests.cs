using System.Collections.Generic;
using Xunit;
using FavoriteStations.Mapping;
using FavoriteStations.Models.Dto;
using FavoriteStations.Models.Entity;
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
                new object[] { new StationDto() {
                    StationId = 1,
                    Title = "Station 1",
                    Url = "station.com",
                    IconUrl = "icon.com",
                    Notes = "This is a really cool station"
                }},
                new object[] { new StationDto() {
                    StationId = 2,
                    Title = "Station 2",
                    Url = "anotherstation.com",
                    IconUrl = "anothericon.com",
                    Notes = "This station isn't quite as good"
                }}
            };

        public static IEnumerable<object[]> DummyStationCreateUpdateDtos =>
            new List<object[]> {
                new object[] { new StationCreateUpdateDto() {
                    Title = "Station 1",
                    Url = "station.com",
                    IconUrl = "icon.com",
                    Notes = "This is a really cool station"
                }},
                new object[] { new StationCreateUpdateDto() {
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
        [MemberData(nameof(DummyStationCreateUpdateDtos))]
        public void ShouldMapStationCreateUpdateDtoToStation(StationCreateUpdateDto station) {
            var mapped = this.mapper.Map<Station>(station);
            Assert.Null(mapped.StationId);
            Assert.Equal(station.Title, mapped.Title);
            Assert.Equal(station.Url, mapped.Url);
            Assert.Equal(station.IconUrl, mapped.IconUrl);
            Assert.Equal(station.Notes, mapped.Notes);            
        }

        [Theory]
        [MemberData(nameof(DummyStationEntities))]
        public void ShouldMapStationToStationDto(Station station) {
            var mapped = this.mapper.Map<StationDto>(station);
            Assert.Equal(station.StationId, mapped.StationId);
            Assert.Equal(station.Title, mapped.Title);
            Assert.Equal(station.Url, mapped.Url);
            Assert.Equal(station.IconUrl, mapped.IconUrl);
            Assert.Equal(station.Notes, mapped.Notes);            
        }
    }
}
