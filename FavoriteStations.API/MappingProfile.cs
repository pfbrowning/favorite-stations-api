using FavoriteStations.Models.Dto;
using FavoriteStations.Models.Entity;
using AutoMapper;

namespace FavoriteStations.Mapping {
    public class MappingProfile : Profile {
        public MappingProfile() {
            CreateMap<Station, StationDto>();
            CreateMap<StationCreateUpdateDto, Station>();
        }
    }
}