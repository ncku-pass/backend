using Api.RequestModel.Parameters;
using Application.Dto.Messages;
using AutoMapper;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Profiles
{
    public class AuthenticationProfile : Profile
    {
        public AuthenticationProfile()
        {
            CreateMap<AuthenticateLoginParameter, AuthenticateLoginMessage>();
            CreateMap<AuthenticateRegisterParameter, AuthenticateRegisterMessage>();
            CreateMap<AuthenticateRegisterMessage, User>();
        }
    }
}
