using Application.Dto.Messages;
using Application.Dto.Responses;
using Application.Services.Interface;
using AutoMapper;
using Infrastructure.Infrastructure;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AuthenticateService : IAuthenticateService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AuthenticateService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IConfiguration configuration,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager
            )
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
            this._configuration = configuration;
            this._userManager = userManager;
            this._signInManager = signInManager;
        }

        public async Task<AuthenticateLoginResponse> Login(AuthenticateLoginMessage loginMessage)
        {
            //1 驗證帳密
            var loginResult = await _signInManager.PasswordSignInAsync(
                                                loginMessage.StudentId,
                                                loginMessage.Password,
                                                false,
                                                false
                                                );
            if (!loginResult.Succeeded)
            {
                return new AuthenticateLoginResponse() { Succeeded = false };
            }
            var user = await this._userManager.FindByNameAsync(loginMessage.StudentId);
            var userInfo = await this._unitOfWork.User.FirstOrDefaultAsync(u => u.AspNetId == Guid.Parse(user.Id));

            //2 創建JWT
            //header
            var signingAlgorithm = SecurityAlgorithms.HmacSha256;
            //payload
            var claims = new List<Claim>
            {
                //sub
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.Id.ToString()),
            };
            var roleNames = await this._userManager.GetRolesAsync(user);
            foreach (var roleName in roleNames)
            {
                var roleClaim = new Claim(ClaimTypes.Role, roleName);
                claims.Add(roleClaim);
            }
            //signiture
            var serectByte = Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]);
            var signatureKey = new SymmetricSecurityKey(serectByte);
            var singingCredentials = new SigningCredentials(signatureKey, signingAlgorithm);

            var token = new JwtSecurityToken(
                issuer: _configuration["Authentication:Issuer"], // 發布者
                audience: _configuration["Authentication:Audience"], // 要求使用者
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddDays(1),
                singingCredentials
            );

            var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

            return new AuthenticateLoginResponse()
            {
                Succeeded = true,
                TokenStr = tokenStr
            };
        }

        public async Task<AuthenticateRegisterResponse> Register(AuthenticateRegisterMessage registerParameter)
        {
            // 1 使用用戶名創建對象
            var user = new IdentityUser
            {
                UserName = registerParameter.StudentId,
                Email = registerParameter.Email
            };

            // 2 Hash密碼,保存用戶
            var result = await _userManager.CreateAsync(user, registerParameter.Password);
            if (!result.Succeeded)
            {
                return new AuthenticateRegisterResponse() { Succeeded = false };
            }

            // 3 在UserTable保存用戶 return
            var userModel = this._mapper.Map<User>(registerParameter);
            userModel.AspNetId = Guid.Parse(user.Id);
            this._unitOfWork.User.Add(userModel);
            await this._unitOfWork.SaveChangeAsync();
            return new AuthenticateRegisterResponse() { Succeeded = true };
        }
    }
}