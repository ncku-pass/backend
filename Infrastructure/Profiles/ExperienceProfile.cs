using Application.Domains;
using Application.Dtos;
using AutoMapper;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Profiles
{
    public class ExperienceProfile : Profile
    {

        public ExperienceProfile()
        {
            CreateMap<Experience, ExperienceDomain>().ReverseMap();
            CreateMap<ExperienceDomain, ExperienceDto>().ReverseMap();

            //CreateMap<TouristRouteForCreationDto, TouristRoute>().
            //    ForMember(
            //        dest => dest.Id,
            //        opt => opt.MapFrom(src => Guid.NewGuid())
            //    );
        }
    }
}
