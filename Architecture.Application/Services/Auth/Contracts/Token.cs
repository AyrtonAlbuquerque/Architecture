using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Architecture.Application.Services.Auth.Contracts
{
    public class Token
    {
        public string Type { get; set; }
        public string Value { get; set; }
        public double? Expires { get; set; }

        public Token(Login login, string secret, double session = 24)
        {
            ArgumentException.ThrowIfNullOrEmpty(secret);

            Type = "Bearer";
            Expires = TimeSpan.FromHours(session).TotalMilliseconds;
            Value = new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
                issuer: "Architecture",
                audience: "Architecture",
                claims: new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, login.Username),
                },
                expires: DateTime.UtcNow.AddHours(session),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)), SecurityAlgorithms.HmacSha256Signature)
            ));
        }
    }

}