using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Architecture.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Architecture.Application.Common
{
    public class Token
    {
        public string Type { get; set; }
        public string Value { get; set; }
        public double? Expires { get; set; }

        public Token(User user)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            var issuer = configuration["AppSettings:Jwt:Issuer"];
            var audience = configuration["AppSettings:Jwt:Audience"];
            var secret = configuration["AppSettings:Jwt:Secret"];
            var expiration = double.TryParse(configuration["AppSettings:Jwt:Expiration"], out var expires) ? expires : 1;

            Type = "Bearer";
            Expires = TimeSpan.FromHours(expiration).TotalMilliseconds;
            Value = new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                },
                expires: DateTime.Now.AddHours(expiration),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)), SecurityAlgorithms.HmacSha256Signature)
            ));
        }
    }
}