﻿using AutoMapper;
using MovieApp.Dto;
using MovieApp.Models;

namespace MovieApp.AutoMapper
{
    public class MappingProfiles : Profile
    {

        public MappingProfiles() {
            CreateMap<Show, ShowDto>();
            CreateMap<ShowDto, Show>();
            CreateMap<Tag, TagDto>();
            CreateMap<TagDto, Tag>();
            CreateMap<Binge, BingeDto>();
            CreateMap<Binge, BingeExtDto>();
            CreateMap<BingeDto, Binge>();
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
            CreateMap<User, LoginDto>();
            CreateMap<LoginDto, User>();
        }
    }
}
