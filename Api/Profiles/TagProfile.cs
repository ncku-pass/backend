using Api.RequestModel.Parameters;
using Api.RequestModel.ViewModels;
using Application.Dto.Messages;
using Application.Dto.Responses;
using AutoMapper;
using Infrastructure.Models;

namespace Api.Profiles
{
    public class TagProfile : Profile
    {
        public TagProfile()
        {
            CreateMap<TagManipulateParameter, TagManipulateMessage>();
            CreateMap<TagManipulateMessage, Tag>();
            CreateMap<Tag, TagResponse>();
            CreateMap<TagResponse, TagViewModel>();
        }
    }
}