using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Architecture.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Architecture.Application.Common
{
    public interface IToken
    {
        string Create(User user);
    }

    public class Token(Settings settings) : IToken
    {
        public string Create(User user)
        {
            return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
                issuer: settings.Jwt.Issuer,
                audience: settings.Jwt.Audience,
                claims: new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                },
                expires: DateTime.Now.AddHours(settings.Jwt.Expiration),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Jwt.Secret)), SecurityAlgorithms.HmacSha256Signature)
            ));
        }
    }
}