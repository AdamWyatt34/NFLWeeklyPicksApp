using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NFLWeeklyPicksAPI.Models.Authorization;
using NFLWeeklyPicksAPI.Models.Entities;
using NFLWeeklyPicksAPI.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace NFLWeeklyPicksAPI.Commands.Authentication
{
    public class CreateToken : IRequest<TokenDto>
    {
        public User User { get; set; }

        public bool PopulateExpiration { get; set; }

        public class Handler : IRequestHandler<CreateToken, TokenDto>
        {
            private readonly UserManager<User> _userManager;
            private readonly JwtOptions _jwtOptions;

            public Handler(UserManager<User> userManager, IOptions<JwtOptions> jwtOptions)
            {
                _userManager = userManager;
                _jwtOptions = jwtOptions.Value;
            }

            public async Task<TokenDto> Handle(CreateToken request, CancellationToken cancellationToken)
            {
                var signingCredentials = GetSigningCredentials();

                var claims = await GetClaims(request.User);

                var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

                var refreshToken = GenerateRefreshToken();

                request.User.RefreshToken = refreshToken;

                if (request.PopulateExpiration)
                {
                    request.User.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
                }

                await _userManager.UpdateAsync(request.User);

                var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

                return new TokenDto(accessToken, refreshToken);
            }

            private SigningCredentials GetSigningCredentials()
            {
                var key = Encoding.UTF8.GetBytes(_jwtOptions.SecretKey);
                var secret = new SymmetricSecurityKey(key);
                return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
            }

            private async Task<List<Claim>> GetClaims(User user)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName)
                };
                var roles = await _userManager.GetRolesAsync(user);
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                return claims;
            }

            private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
            {
                var tokenOptions = new JwtSecurityToken
                (
                    issuer: _jwtOptions.ValidIssuer,
                    audience: _jwtOptions.ValidAudience,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtOptions.Expires)),
                    signingCredentials: signingCredentials
                );
                return tokenOptions;
            }

            private string GenerateRefreshToken()
            {
                var randomNumber = new byte[32];

                using var rng = RandomNumberGenerator.Create();
                rng.GetBytes(randomNumber);

                return Convert.ToBase64String(randomNumber);
            }

            public static ClaimsPrincipal GetPrincipalFromExpiredToken(string token, JwtOptions jwtOptions)
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
                    ValidateLifetime = true,
                    ValidIssuer = jwtOptions.ValidIssuer,
                    ValidAudience = jwtOptions.ValidAudience
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                SecurityToken securityToken;

                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);

                var jwtSecurityToken = securityToken as JwtSecurityToken;

                if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                        StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new SecurityTokenException("Invalid token");
                }

                return principal;
            }
        }
    }
}