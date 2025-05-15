using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.JsonWebTokens;
using UserService.Application.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace UserService.Infrastructure;

public class JwtTokenGenerator : ITokenGenerator
{
    public async Task<string> GenerateToken(string key, Guid userId)
    {
        await Task.CompletedTask;
        SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new Claim[]
                     {
                         new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                         new Claim("user_id", userId.ToString()),
                     };

        SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor()
                                             {
                                                 IssuedAt = DateTime.UtcNow,
                                                 Expires = DateTime.UtcNow.AddHours(1),
                                                 SigningCredentials = credentials,
                                                 Issuer = "Mehrdad",
                                                 Subject = new ClaimsIdentity(claims),
                                             };
        JsonWebTokenHandler tokenHandler = new JsonWebTokenHandler();
        return tokenHandler.CreateToken(descriptor);
    }

    public async Task<bool> ValidateToken(string token, string key)
    {
        JsonWebTokenHandler tokenHandler = new JsonWebTokenHandler();
        TokenValidationParameters parameters = new TokenValidationParameters()
                                               {
                                                   ValidateLifetime = true,
                                                   ValidateAudience = false,
                                                   ValidateIssuer = true,
                                                   ValidIssuer = "Mehrdad",
                                                   ValidateIssuerSigningKey = true,
                                                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                                                   ClockSkew = TimeSpan.Zero,
                                               };
        return (await tokenHandler.ValidateTokenAsync(token, parameters)).IsValid;
    }
}