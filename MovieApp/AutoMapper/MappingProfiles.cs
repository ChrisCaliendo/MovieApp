using AutoMapper;
using MovieApp.Dto;
using MovieApp.Models;

namespace MovieApp.AutoMapper
{
    public class MappingProfiles : Profile
    {

        public MappingProfiles() {
            CreateMap<Show, ShowDto>();
        }
    }
}
