using Api.RequestModel.Parameters;
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
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AuthenticateController(
            IConfiguration configuration,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager
            )
        {
            this._configuration = configuration;
            this._userManager = userManager;
            this._signInManager = signInManager;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginParameter loginParameter)
        {
            //1 驗證帳密
            var loginResult = await _signInManager.PasswordSignInAsync(
                                                loginParameter.Email,
                                                loginParameter.PassWord,
                                                false,
                                                false
                                                );
            if (!loginResult.Succeeded)
            {
                return this.BadRequest();
            }
            var user = await this._userManager.FindByNameAsync(loginParameter.Email);

            //2 創建JWT
            //header
            var signingAlgorithm = SecurityAlgorithms.HmacSha256;
            //payload
            var claims = new List<Claim>
            {
                //sub
                new Claim(JwtRegisteredClaimNames.Sub, "1"),
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

            //3 回傳200ok + JWT
            return this.Ok(tokenStr);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterParameter registerParameter)
        {
            // 1 使用用戶名創建對象
            var user = new IdentityUser
            {
                UserName = registerParameter.Email,
                Email = registerParameter.Email
            };

            // 2 Hash密碼,保存用戶
            var result = await _userManager.CreateAsync(user, registerParameter.Password);
            if (!result.Succeeded)
            {
                return this.BadRequest();
            }
            // 3 return
            return this.Ok();


        }
    }
}
