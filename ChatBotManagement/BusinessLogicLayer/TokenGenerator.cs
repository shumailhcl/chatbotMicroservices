using ChatBotManagement.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ChatBotManagement.BusinessLogicLayer
{
    public class TokenGenerator
    {
        private IConfiguration _config;
        public TokenGenerator(IConfiguration config)
        {
            _config = config;
        }
            
        public string GenerateJSONWebToken(mEmployeeDetails data,DateTime sessionExpireTime)
        {
            
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
        new Claim("sapId", data.employeeSapId.ToString()),
        new Claim(JwtRegisteredClaimNames.Email, data.employeeEmailId),
        new Claim("employeeId", data.employeeId.ToString()),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims,
              expires: sessionExpireTime,
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
