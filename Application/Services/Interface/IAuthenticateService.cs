using Application.Dto.Messages;
using Application.Dto.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interface
{
    public interface IAuthenticateService
    {
        Task<AuthenticateLoginResponse> Login(AuthenticateLoginMessage loginMessage);
        Task<AuthenticateRegisterResponse> Register(AuthenticateRegisterMessage registerParameter);
    }
}
