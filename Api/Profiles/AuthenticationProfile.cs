using Api.RequestModel.Parameters;
using Application.Dto.Messages;
using AutoMapper;
using Infrastructure.Models;

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