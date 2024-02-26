using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Payslip.API;
using Payslip.API.Base;
using Payslip.API.Extensions;
using Payslip.Application.Base;
using Payslip.Core.Entities;
using Payslip.Infrastructure.Data;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseKestrel(options =>
{
    options.ListenAnyIP(80); // Use port 80 for HTTP
    options.ListenAnyIP(4030);
});

// Add services to the container.

builder.Services.AddControllers();

var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

AppSettingsModel appSettingsModel = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("CONFIG")) ? config.Get<AppSettingsModel>() :
    JsonConvert.DeserializeObject<AppSettingsModel>(Environment.GetEnvironmentVariable("CONFIG")!)!;

builder.Services.AddDbContext<DataContext>(opt => opt.UseSqlServer(appSettingsModel.ConnectionStrings.Main));

builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.Converters.Add(new PersianDateTimeConverter());
    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

var jwtIssuerOptions = appSettingsModel.JwtIssuerOptions;

SymmetricSecurityKey signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtIssuerOptions.SecretKey));
builder.Services.Configure<JwtIssuerOptionsModel>(options =>
{
    options.Issuer = jwtIssuerOptions.Issuer;
    options.Audience = jwtIssuerOptions.Audience;
    options.SecretKey = jwtIssuerOptions.SecretKey;
    options.ExpireTimeTokenInMinute = jwtIssuerOptions.ExpireTimeTokenInMinute;
    options.ValidTimeInMinute = jwtIssuerOptions.ValidTimeInMinute;
    options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
});
// add identity
var identityBuilder = builder.Services.AddIdentity<User, IdentityRole<Guid>>(o =>
{
    // configure identity options
    o.Password.RequireDigit = true;
    o.Password.RequireLowercase = false;
    o.Password.RequireUppercase = false;
    o.Password.RequireNonAlphanumeric = false;
    o.Password.RequiredLength = 8;
    o.Tokens.ChangePhoneNumberTokenProvider = "Phone";
});

identityBuilder = new IdentityBuilder(identityBuilder.UserType, typeof(IdentityRole<Guid>), builder.Services);

identityBuilder.AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders();

var tokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = true,
    ValidIssuer = jwtIssuerOptions.Issuer,

    ValidateAudience = true,
    ValidAudience = jwtIssuerOptions.Audience,

    ValidateIssuerSigningKey = true,
    IssuerSigningKey = signingKey,

    RequireExpirationTime = false,
    ValidateLifetime = true,
    ClockSkew = TimeSpan.Zero
};

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, configureOptions =>
{
    configureOptions.ClaimsIssuer = jwtIssuerOptions.Issuer;
    configureOptions.TokenValidationParameters = tokenValidationParameters;
    configureOptions.SaveToken = true;
});


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin",
        authBuilder =>
        {
            authBuilder.RequireRole("Admin");
        });

    options.AddPolicy("User",
        authBuilder =>
        {
            authBuilder.RequireRole("User");
        });
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

Registry.Register(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseCors(c =>
{
    c.AllowAnyHeader();
    c.AllowAnyMethod();
    c.AllowAnyOrigin();
});

app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MigrateDatabase<Program>();

app.Run();
