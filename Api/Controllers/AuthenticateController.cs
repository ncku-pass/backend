using Api.RequestModel.Parameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        public AuthenticateController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginParameter loginParameter)
        {
            //1 驗證帳密

            //2 創建JWT
            //header
            var signingAlgorithm = SecurityAlgorithms.HmacSha256;
            //payload
            var claims = new[]
            {
                //sub
                new Claim(JwtRegisteredClaimNames.Sub, "1"),
                // TODO:之後要處裡後台權限
                //new Claim(ClaimTypes.Role, "Admin")
            };
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
    }
}
