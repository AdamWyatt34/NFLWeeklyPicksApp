using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NFLWeeklyPicksAPI.Exceptions;
using NFLWeeklyPicksAPI.Models.Authorization;
using NFLWeeklyPicksAPI.Models.Entities;
using NFLWeeklyPicksAPI.Options;

namespace NFLWeeklyPicksAPI.Commands.Authentication
{
    public class RefreshToken : IRequest<TokenDto>
    {
        public TokenDto Token { get; set; }

        public class Handler : IRequestHandler<RefreshToken, TokenDto>
        {
            private readonly UserManager<User> _userManager;
            private readonly JwtOptions _jwtOptions;
            private readonly IMediator _dispatcher;

            public Handler(UserManager<User> userManager, IOptions<JwtOptions> jwtOptions, IMediator dispatcher)
            {
                _userManager = userManager;
                _jwtOptions = jwtOptions.Value;
                _dispatcher = dispatcher;
            }

            public async Task<TokenDto> Handle(RefreshToken request, CancellationToken cancellationToken)
            {
                var principal = GetPrincipalFromExpiredToken(request.Token.AccessToken, _jwtOptions);

                var user = await _userManager.FindByNameAsync(principal.Identity.Name);

                if (user == null || user.RefreshToken != request.Token.RefreshToken ||
                    (user.RefreshTokenExpiryTime != default && user.RefreshTokenExpiryTime <= DateTime.Now))
                    throw new RefreshTokenBadRequest();

                var result = await _dispatcher.Send(new CreateToken() { User = user, PopulateExpiration = false },
                    cancellationToken);

                return result;
            }

            private static ClaimsPrincipal GetPrincipalFromExpiredToken(string token, JwtOptions jwtOptions)
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
                    ValidateLifetime = false,
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