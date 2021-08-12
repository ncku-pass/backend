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
            // TODO:改名為Category
            CreateMap<ExperienceCreateParameter, ExperienceCreateMessage>()
                .ForMember(
                    dest => dest.Category,
                    opt => opt.MapFrom(src => string.Join(" ", src.Type))
                );
            CreateMap<ExperienceUpdateParameter, ExperienceUpdateMessage>()
                .ForMember(
                    dest => dest.Category,
                    opt => opt.MapFrom(src => string.Join(" ", src.Type))
                );

            CreateMap<ExperienceCreateMessage, Experience>();
            CreateMap<ExperienceUpdateMessage, Experience>();

            CreateMap<Experience, ExperienceResponse>()
                .ForMember(
                    dest => dest.ExperienceType,
                    opt => opt.MapFrom(src => src.ExperienceType.ToString())
                );
            CreateMap<ExperienceResponse, ExperienceViewModel>()
                .ForMember(
                    dest => dest.Type,
                    opt => opt.MapFrom(src => src.Category.Split())
                );

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