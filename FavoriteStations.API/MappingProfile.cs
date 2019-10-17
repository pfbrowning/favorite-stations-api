using FavoriteStations.Models;
using AutoMapper;

namespace FavoriteStations.Mapping {
    public class MappingProfile : Profile {
        public MappingProfile() {
            CreateMap<Station, StationDTO>();
            CreateMap<StationDTO, Station>();
        }
    }
}