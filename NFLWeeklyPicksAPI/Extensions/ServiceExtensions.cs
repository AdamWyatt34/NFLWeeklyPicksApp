using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using NFLWeeklyPicksAPI.Behaviors.CustomTokenProviders;
using NFLWeeklyPicksAPI.Models.Entities;
using NFLWeeklyPicksAPI.Options;
using System.Text;

namespace NFLWeeklyPicksAPI.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentity<User, IdentityRole>(o =>
                {
                    o.Password.RequireDigit = true;
                    o.Password.RequireLowercase = false;
                    o.Password.RequireUppercase = false;
                    o.Password.RequireNonAlphanumeric = false;
                    o.Password.RequiredLength = 10;
                    o.User.RequireUniqueEmail = true;
                    o.SignIn.RequireConfirmedEmail = true;
                    o.Tokens.EmailConfirmationTokenProvider = "emailconfirmation";
                    o.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                    o.Lockout.MaxFailedAccessAttempts = 3;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddTokenProvider<EmailConfirmationTokenProvider<User>>("emailconfirmation");
        }

        public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtConfiguration = new JwtOptions();
            configuration.Bind(jwtConfiguration.Section, jwtConfiguration);

            services.AddAuthentication(opt =>
                {
                    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = jwtConfiguration.ValidIssuer,
                        ValidAudience = jwtConfiguration.ValidAudience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration.SecretKey)),
                    };
                });
        }

        public static void AddJwtConfiguration(this IServiceCollection services, IConfiguration configuration) =>
            services.Configure<JwtOptions>(configuration.GetSection("JwtSettings"));
    }
}