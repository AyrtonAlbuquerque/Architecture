using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Architecture.Application.Options;
using Microsoft.IdentityModel.Tokens;

namespace Architecture.Application.Services.Auth.Contracts
{
    public class Token
    {
        public string Type { get; set; }
        public string Value { get; set; }
        public double? Expires { get; set; }

        public Token(Login login, Settings settings)
        {
            Type = "Bearer";
            Expires = TimeSpan.FromHours(settings.Jwt.Expiration).TotalMilliseconds;
            Value = new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
                issuer: settings.Jwt.Issuer,
                audience: settings.Jwt.Audience,
                claims: new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, login.Username),
                },
                expires: DateTime.Now.AddHours(settings.Jwt.Expiration),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Jwt.Secret)), SecurityAlgorithms.HmacSha256Signature)
            ));
        }
    }
}