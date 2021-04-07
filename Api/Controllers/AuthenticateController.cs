using Api.RequestModel.Parameters;
using Application.Dto.Messages;
using Application.Services.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthenticateService _authenticateService;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AuthenticateController(
            IConfiguration configuration,
            IAuthenticateService authenticateService,
            IMapper mapper,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager
            )
        {
            this._configuration = configuration;
            this._authenticateService = authenticateService;
            this._mapper = mapper;
            this._userManager = userManager;
            this._signInManager = signInManager;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] AuthenticateLoginParameter loginParameter)
        {
            var loginMessage = this._mapper.Map<AuthenticateLoginMessage>(loginParameter);
            var loginResponse = await this._authenticateService.Login(loginMessage);
            if (!loginResponse.Succeeded)
            {
                return this.BadRequest();
            }
            return this.Ok(loginResponse);

        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AuthenticateRegisterParameter registerParameter)
        {
            var registerMessage = this._mapper.Map<AuthenticateRegisterMessage>(registerParameter);
            var registerResponse = await this._authenticateService.Register(registerMessage);
            if (!registerResponse.Succeeded)
            {
                return this.BadRequest();
            }
            return this.Ok();
        }
    }
}
