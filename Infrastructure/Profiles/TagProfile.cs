using Application.Domains;
using Application.Dtos;
using AutoMapper;
using Infrastructure.Models;
using System.Collections.Generic;

namespace Infrastructure.Profiles
{
    public class TagProfile : Profile
    {
        public TagProfile()
        {
            CreateMap<Tag, TagDomain>().ReverseMap();
            CreateMap<TagDomain, TagDto>().ReverseMap();
        }
    }
}
