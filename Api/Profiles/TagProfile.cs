using Api.RequestModel.Parameters;
using Api.RequestModel.ViewModels;
using Application.Dto.Messages;
using Application.Dto.Responses;
using AutoMapper;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Profiles
{
    public class TagProfile : Profile
    {
        public TagProfile()
        {
            CreateMap<TagCreateParameter, TagCreateMessage>();
            CreateMap<TagCreateMessage, Tag>();
            CreateMap<Tag, TagResponse>();
            CreateMap<TagResponse, TagViewModel>();
        }
    }
}
