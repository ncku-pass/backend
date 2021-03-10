using Api.RequestModel.Parameters;
using Api.RequestModel.ViewModels;
using Application.Dto.Messages;
using Application.Dto.Responses;
using AutoMapper;
using Infrastructure.Models;

namespace Api.Profiles
{
    public class ExperienceProfile : Profile
    {
        public ExperienceProfile()
        {
            CreateMap<ExperienceCreateParameter, ExperienceCreateMessage>();
            CreateMap<ExperienceCreateMessage, Experience>();
            CreateMap<Experience, ExperienceResponse>();
            CreateMap<ExperienceResponse, ExperienceViewModel>();

            //CreateMap<TouristRouteForCreationDto, TouristRoute>().
            //    ForMember(
            //        dest => dest.Id,
            //        opt => opt.MapFrom(src => Guid.NewGuid())
            //    );
        }
    }
}