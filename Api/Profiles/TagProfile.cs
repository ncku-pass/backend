using Application.Domains;
using Application.Dtos;
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
            CreateMap<Tag, TagDomain>().ReverseMap();
            CreateMap<TagDomain, TagDto>().ReverseMap();
        }
    }
}
