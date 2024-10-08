﻿using Application.Dto.Messages;
using Application.Dto.Responses;
using Application.Services.Interface;
using AutoMapper;
using Infrastructure.Infrastructure;
using Infrastructure.Models;
using Infrastructure.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
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
        private readonly INCKUPortalService _NCKUPortalService;
        private readonly IAESCryptAPI _AESCryptAPI;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AuthenticateService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IConfiguration configuration,
            INCKUPortalService nCKUPortalService,
            IAESCryptAPI aESCryptAPI,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager
            )
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
            this._configuration = configuration;
            this._NCKUPortalService = nCKUPortalService;
            this._AESCryptAPI = aESCryptAPI;
            this._userManager = userManager;
            this._signInManager = signInManager;
        }

        private async Task<string> GenerateJWTToken(IdentityUser identityUser, User userModel)
        {
            //header
            var signingAlgorithm = SecurityAlgorithms.HmacSha256;

            //payload
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userModel.Id.ToString()),
                new Claim("studentId", userModel.StudentId)
            };
            var roleNames = await this._userManager.GetRolesAsync(identityUser);
            foreach (var roleName in roleNames)
            {
                claims.Add(new Claim("roles", roleName));
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
                expires: DateTime.UtcNow.AddDays(7),
                singingCredentials
            );

            var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenStr;
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
            var identityUser = await this._userManager.FindByNameAsync(loginMessage.StudentId);
            var userModel = await this._unitOfWork.User.FirstOrDefaultAsync(u => u.AspNetId == Guid.Parse(identityUser.Id));

            //2 創建JWT
            //payload
            var tokenStr = await this.GenerateJWTToken(identityUser, userModel);

            //3 創建ImageToken
            
            var imageToken = this._AESCryptAPI.Encrypt(userModel.StudentId);


            return new AuthenticateLoginResponse()
            {
                Succeeded = true,
                TokenStr = tokenStr,
                ImageToken = imageToken
            };
        }

        public async Task<AuthenticateLoginResponse> LoginByNCKUPortal(NCKUPortalTokenMessage message)
        {
            //1 驗證Token是否為真計中Token
            var loginResult = await this._NCKUPortalService.TokenVerify(message);
            if (!loginResult.Succeeded)
            {
                return new AuthenticateLoginResponse() { Succeeded = false, ErrorMessage = loginResult.ErrorMessage };
            }

            //2 若為第一次登入則註冊
            var identityUser = await this._userManager.FindByNameAsync(loginResult.StudentId);
            if (identityUser == null)
            {
                var registerMessage = this._mapper.Map<NCKUPortalRegisterMessage>(loginResult);
                var registerResponse = await this.RegisterWithNCKUPortal(registerMessage);
                if (!registerResponse.Succeeded)
                {
                    return new AuthenticateLoginResponse() { Succeeded = false, ErrorMessage = registerResponse.ErrorMessage[0] };
                }
                identityUser = await this._userManager.FindByNameAsync(loginResult.StudentId);
            }
            var userModel = await this._unitOfWork.User.FirstOrDefaultAsync(u => u.AspNetId == Guid.Parse(identityUser.Id));

            //2 創建JWT
            //payload
            var tokenStr = await this.GenerateJWTToken(identityUser, userModel);

            //3 創建ImageToken

            var imageToken = this._AESCryptAPI.Encrypt(userModel.StudentId);

            return new AuthenticateLoginResponse()
            {
                Succeeded = true,
                TokenStr = tokenStr,
                ImageToken = imageToken
            };
        }

        public async Task<AuthenticateRegisterResponse> Register(AuthenticateRegisterMessage registerMessage)
        {
            // 1 使用用戶名創建對象
            var user = new IdentityUser
            {
                UserName = registerMessage.StudentId,
                Email = registerMessage.Email
            };

            // 2 Hash密碼,保存用戶
            var result = await _userManager.CreateAsync(user, registerMessage.Password);
            if (!result.Succeeded)
            {
                var errorMessage = result.Errors.Select(e => e.Code + ": " + e.Description).ToList();
                return new AuthenticateRegisterResponse() { Succeeded = false, ErrorMessage = errorMessage };
            }

            // 3 在UserTable保存用戶 return
            var userModel = this._mapper.Map<User>(registerMessage);
            userModel.AspNetId = Guid.Parse(user.Id);
            userModel.DepartmentId = (await this._unitOfWork.Department.FirstOrDefaultAsync(d => d.Prefix == registerMessage.Major)).Id;
            this._unitOfWork.User.Add(userModel);
            await this._unitOfWork.SaveChangeAsync();
            return new AuthenticateRegisterResponse() { Succeeded = true };
        }

        private async Task<AuthenticateRegisterResponse> RegisterWithNCKUPortal(NCKUPortalRegisterMessage registerMessage)
        {
            // 1 使用用戶名創建對象
            var user = new IdentityUser
            {
                UserName = registerMessage.StudentId,
                Email = registerMessage.Email
            };

            // 2 保存用戶
            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                var errorMessage = result.Errors.Select(e => e.Code + ": " + e.Description).ToList();
                return new AuthenticateRegisterResponse() { Succeeded = false, ErrorMessage = errorMessage };
            }

            // 3 在UserTable保存用戶 return
            var userModel = this._mapper.Map<User>(registerMessage);
            userModel.AspNetId = Guid.Parse(user.Id);
            userModel.DepartmentId = (await this._unitOfWork.Department.FirstOrDefaultAsync(d => d.Prefix == registerMessage.Major)).Id;
            this._unitOfWork.User.Add(userModel);
            await this._unitOfWork.SaveChangeAsync();
            return new AuthenticateRegisterResponse() { Succeeded = true };
        }
    }
}