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
                    dest => dest.Categories,
                    opt => opt.MapFrom(src => string.Join(" ", src.Categories))
                );
            CreateMap<ExperienceUpdateParameter, ExperienceUpdateMessage>()
                .ForMember(
                    dest => dest.Categories,
                    opt => opt.MapFrom(src => string.Join(" ", src.Categories))
                );

            CreateMap<ExperienceCreateMessage, Experience>();
            CreateMap<ExperienceUpdateMessage, Experience>();

            CreateMap<Experience, ExperienceResponse>()
                .ForMember(
                    dest => dest.Type,
                    opt => opt.MapFrom(src => src.Type.ToString())
                );
            CreateMap<ExperienceResponse, ExperienceViewModel>()
                .ForMember(
                    dest => dest.Categories,
                    opt => opt.MapFrom(src => src.Categories.Split())
                );

            CreateMap<List<ExperienceViewModel>, ExperienceClassifiedViewModel>()
                .ForMember(
                    dest => dest.Course,
                    opt => opt.MapFrom(src => src.Where(e => e.Type == "course").ToList())
                )
                .ForMember(
                    dest => dest.Activity,
                    opt => opt.MapFrom(src => src.Where(e => e.Type == "activity").ToList())
                )
                .ForMember(
                    dest => dest.Competition,
                    opt => opt.MapFrom(src => src.Where(e => e.Type == "competition").ToList())
                )
                .ForMember(
                    dest => dest.Work,
                    opt => opt.MapFrom(src => src.Where(e => e.Type == "work").ToList())
                )
                .ForMember(
                    dest => dest.Certificate,
                    opt => opt.MapFrom(src => src.Where(e => e.Type == "certificate").ToList())
                )
                .ForMember(
                    dest => dest.Other,
                    opt => opt.MapFrom(src => src.Where(e => e.Type == "other").ToList())
                );

            CreateMap<ExperienceResponse, ExperienceUpdateParameter>()
                .ForMember(
                    dest => dest.Tags,
                    opt => opt.MapFrom(src => src.Tags.Select(t => t.Id).ToList())
                );
        }
    }
}