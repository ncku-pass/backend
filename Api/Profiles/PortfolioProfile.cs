using Api.RequestModel.Parameters;
using Api.RequestModel.ViewModels;
using Application.Dto.Messages;
using Application.Dto.Responses;
using AutoMapper;
using Infrastructure.Models;

namespace Api.Profiles
{
    public class PortfolioProfile : Profile
    {
        public PortfolioProfile()
        {
            CreateMap<Topic, TopicResponse>();
            CreateMap<TopicResponse, TopicViewModel>();

            CreateMap<Resume, ResumeResponse>();
            CreateMap<ResumeResponse, ResumeViewModel>();

            CreateMap<PortfolioSaveParameter, PortfolioSaveMessage>();
            CreateMap<PortfolioSaveMessage, Resume>();
            CreateMap<TopicSaveParameter, TopicSaveMessage>();
            CreateMap<TopicSaveMessage, Topic>();

        }
    }
}
