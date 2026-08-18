using edumis.Common;
using edumis.Models.Users.DTO;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace edumisbackend.Helpers;

public sealed class TokenHelper(IConfiguration configuration)
{
    public string CreateToken(UserDTO user)
    {
        if (user == null) return string.Empty;

        var AuthClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, Utilities.EncryptString(user.UserId.ToString())),
                new Claim("Branch", string.IsNullOrEmpty(user.BranchId) ? string.Empty: user.BranchId),
                new Claim("UserType", user.UserType.ToString()),
                new Claim("UserRole", user.UserRole.HasValue ? user.UserRole.ToString() : string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

        var AuthSignInKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["JWTAuth:Secret"]));

        var token = new JwtSecurityToken(
            issuer: configuration["JWTAuth:ValidIssuer"].ToString(),
            audience: configuration["JWTAuth:ValidAudience"],
            claims: AuthClaims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(configuration["JWTAuth:AccessTokenExpirationMinutes"])),
            signingCredentials: new SigningCredentials(AuthSignInKey, SecurityAlgorithms.HmacSha256)
            );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string CreateTempToken(string UserId)
    {
        if (string.IsNullOrEmpty(UserId)) return string.Empty;

        var AuthClaims = new List<Claim>
            {               
                new Claim(ClaimTypes.NameIdentifier, Utilities.EncryptString(UserId)),  
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

        var AuthSignInKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["JWTAuth:Secret"]));

        var token = new JwtSecurityToken(
            issuer: configuration["JWTAuth:ValidIssuer"].ToString(),
            audience: configuration["JWTAuth:ValidAudience"],
            claims: AuthClaims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(configuration["JWTAuth:AccessTokenExpirationMinutes"])),
            signingCredentials: new SigningCredentials(AuthSignInKey, SecurityAlgorithms.HmacSha256)
            );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
    public string GenerateCsrfToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
    

    public string GenerateAccessToken(IDictionary<string, string> data) {
        var payload = new List<Claim>();
        foreach (var (key, value) in data) {
            payload.Add( new Claim(key, value));
        }

        var signinKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["JWTAuth:Secret"]??""));

        var token = new JwtSecurityToken(
            issuer: configuration["JWTAuth:ValidIssuer"],
            audience: configuration["JWTAuth:ValidAudience"],
            claims: payload,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(configuration["JWTAuth:AccessTokenExpirationMinutes"])),
            signingCredentials: new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
