using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace The_Scouts.Services.AuthenticationService
{
    public class TokenService
    {
        private readonly UserManager<IdentityUser> _userManager;

        public TokenService(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<string> CreateToken(IdentityUser user, bool rememberMe = false)
        {
            var expiration = rememberMe
                ? DateTime.UtcNow.AddDays(30)   //  Long session if rememberMe is checked
                : DateTime.UtcNow.AddMinutes(30); //  Default short session

            var roles = await _userManager.GetRolesAsync(user);

            var token = CreateJwtToken(
                CreateClaims(user, roles),
                CreateSigningCredentials(),
                expiration
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        private JwtSecurityToken CreateJwtToken(List<Claim> claims, SigningCredentials credentials, DateTime expiration)
        {
            return new JwtSecurityToken(
                issuer: "Alba",
                audience: "Adili",
                expires: expiration,
                claims: claims,
                signingCredentials: credentials
            );
        }

        private List<Claim> CreateClaims(IdentityUser user, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        private SigningCredentials CreateSigningCredentials()
        {
            return new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes("Mmx44IfURe84A/c4i0g2eY8m/DEhzUzXyyVPwKIo2SU=")
                ),
                SecurityAlgorithms.HmacSha256
            );
        }
    }
}
