using Application.Dtos;
using AutoMapper;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Profiles
{
    public class ExperienceProfile : Profile
    {
        public ExperienceProfile()
        {
            CreateMap<Experience, ExperienceDto>().ReverseMap();
            CreateMap<ExperienceDtoCreate, Experience>().ReverseMap();
            CreateMap<List<Tag>, Experience>().ReverseMap();

            //CreateMap<TouristRouteForCreationDto, TouristRoute>().
            //    ForMember(
            //        dest => dest.Id,
            //        opt => opt.MapFrom(src => Guid.NewGuid())
            //    );
        }
    }
}
