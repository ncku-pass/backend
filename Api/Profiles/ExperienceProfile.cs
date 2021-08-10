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
            CreateMap<ExperienceImportParameter, ExperienceCreateMessage>();
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
                    dest => dest.Course,
                    opt => opt.MapFrom(src => src.Where(e => e.ExperienceType == "course").ToList())
                )
                .ForMember(
                    dest => dest.Activity,
                    opt => opt.MapFrom(src => src.Where(e => e.ExperienceType == "activity").ToList())
                )
                .ForMember(
                    dest => dest.Competition,
                    opt => opt.MapFrom(src => src.Where(e => e.ExperienceType == "competition").ToList())
                )
                .ForMember(
                    dest => dest.Work,
                    opt => opt.MapFrom(src => src.Where(e => e.ExperienceType == "work").ToList())
                )
                .ForMember(
                    dest => dest.Certificate,
                    opt => opt.MapFrom(src => src.Where(e => e.ExperienceType == "certificate").ToList())
                )
                .ForMember(
                    dest => dest.Other,
                    opt => opt.MapFrom(src => src.Where(e => e.ExperienceType == "other").ToList())
                );

            CreateMap<ExperienceResponse, ExperienceUpdateParameter>()
                .ForMember(
                    dest => dest.Tags,
                    opt => opt.MapFrom(src => src.Tags.Select(t => t.Id).ToList())
                );
        }
    }
}