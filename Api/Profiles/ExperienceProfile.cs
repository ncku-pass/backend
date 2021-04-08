using Api.RequestModel.Parameters;
using Api.RequestModel.ViewModels;
using Application.Dto.Messages;
using Application.Dto.Responses;
using AutoMapper;
using Infrastructure.Models;
using System.Collections.Generic;
using System.Linq;

namespace Api.Profiles
{
    public class ExperienceProfile : Profile
    {
        public ExperienceProfile()
        {
            CreateMap<ExperienceCreateParameter, ExperienceCreateMessage>();
            CreateMap<ExperienceUpdateParameter, ExperienceUpdateMessage>();

            CreateMap<ExperienceCreateMessage, Experience>();
            CreateMap<ExperienceUpdateMessage, Experience>();

            CreateMap<Experience, ExperienceResponse>()
                .ForMember(
                    dest => dest.ExperienceType,
                    opt => opt.MapFrom(src => src.ExperienceType.ToString())
                );
            CreateMap<ExperienceResponse, ExperienceViewModel>();

            CreateMap<List<ExperienceViewModel>, ExperienceClassifiedViewModel>()
                .ForMember(
                    dest => dest.Courses,
                    opt => opt.MapFrom(src => src.Where(e => e.ExperienceType == "course").ToList())
                )
                .ForMember(
                    dest => dest.Activities,
                    opt => opt.MapFrom(src => src.Where(e => e.ExperienceType == "activity").ToList())
                )
                .ForMember(
                    dest => dest.Competitions,
                    opt => opt.MapFrom(src => src.Where(e => e.ExperienceType == "competition").ToList())
                )
                .ForMember(
                    dest => dest.Works,
                    opt => opt.MapFrom(src => src.Where(e => e.ExperienceType == "work").ToList())
                )
                .ForMember(
                    dest => dest.Certificates,
                    opt => opt.MapFrom(src => src.Where(e => e.ExperienceType == "certificate").ToList())
                )
                .ForMember(
                    dest => dest.Others,
                    opt => opt.MapFrom(src => src.Where(e => e.ExperienceType == "other").ToList())
                );

            CreateMap<ExperienceResponse, ExperienceUpdateParameter>();
        }
    }
}