using Api.RequestModel.Parameters;
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
    public class NCKUPortalProfile : Profile
    {
        public NCKUPortalProfile()
        {
            CreateMap<NCKUPortalTokenParameter, NCKUPortalTokenMessage>();
            CreateMap<NCKUPortalTokenVerifyResponse, NCKUPortalRegisterMessage>();
            CreateMap<NCKUPortalRegisterMessage, User>();
        }
    }
}
