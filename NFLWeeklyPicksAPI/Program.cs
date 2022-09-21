using EmailService;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NFLWeeklyPicksAPI;
using NFLWeeklyPicksAPI.ActionFilters;
using NFLWeeklyPicksAPI.Behaviors;
using NFLWeeklyPicksAPI.Behaviors.CustomTokenProviders;
using NFLWeeklyPicksAPI.Behaviors.Interfaces;
using NFLWeeklyPicksAPI.Extensions;
using NFLWeeklyPicksAPI.Jobs;
using NFLWeeklyPicksAPI.Options;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(typeof(Program));
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddCors(c => { c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin()); });
var configuration = builder.Configuration;
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("Default")));
builder.Services.AddAuthentication();
builder.Services.ConfigureIdentity();
builder.Services.ConfigureJWT(builder.Configuration);
builder.Services.AddJwtConfiguration(configuration);
builder.Services.AddScoped<IServiceManager, ServiceManager>();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<ValidationFilterAttribute>();
var emailConfig = configuration
    .GetSection("EmailConfiguration")
    .Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.Configure<DataProtectionTokenProviderOptions>(opt => opt.TokenLifespan = TimeSpan.FromHours(2));
builder.Services.Configure<EmailConfirmationTokenProviderOptions>(opt => opt.TokenLifespan = TimeSpan.FromDays(3));
builder.Services.AddHttpContextAccessor();
var clientAppConfig = configuration.GetSection("ClientAppOptions");
builder.Services.Configure<ClientAppOptions>(clientAppConfig);
//TODO: uncomment once auth is configured for external providers
//var googleConfig = configuration.GetSection("GoogleAuthentication").Get<GoogleAuthenticationOptioins>();
//builder.Services.AddAuthentication().AddGoogle(options =>
//{
//    options.ClientId = googleConfig.ClientId;
//    options.ClientSecret = googleConfig.ClientSecret;
//});


//Quartz TODO: Implement later
// builder.Services.AddSingleton<IJobFactory, PicksJobFactory>();
// builder.Services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
// builder.Services.AddSingleton<WeeklyWinnerEmailJob>();
// builder.Services.AddSingleton(new JobSchedule(typeof(WeeklyWinnerEmailJob), "0 0/5 * * * ?"));
// builder.Services.AddQuartz(q =>
// {
//     //Job for weekly winner email
//     q.ScheduleJob<WeeklyWinnerEmailJob>(trigger => trigger
//         .WithIdentity("Weekly Winner Email Trigger")
//         .StartNow()
//         .WithDescription("Test job"));
//     //Job to sync weeks
//
//     //Job to sync competitions
// });

// builder.Services.AddQuartzHostedService(options =>
// {
//     //Shut down jobs gracefully.
//     options.WaitForJobsToComplete = true;
// });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(cors => cors.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();