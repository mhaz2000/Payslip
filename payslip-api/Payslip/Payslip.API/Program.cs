using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Payslip.API;
using Payslip.API.Base;
using Payslip.API.Extensions;
using Payslip.Application.Base;
using Payslip.Core.Entities;
using Payslip.Infrastructure.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<DataContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("Main")));

var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json")
                .Build();

AppSettingsModel appSettingsModel = config.Get<AppSettingsModel>();
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
    o.Password.RequireLowercase = true;
    o.Password.RequireUppercase = false;
    o.Password.RequireNonAlphanumeric = false;
    o.Password.RequiredLength = 6;
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
