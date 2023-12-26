using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Payslip.API.Helpers;
using Payslip.Application.Base;
using Payslip.Application.Helpers.TokenHelpers;
using Payslip.Application.Services;
using Payslip.Core.Repositories.Base;
using Payslip.Infrastructure.Repositories.Base;


namespace Payslip.API
{
    public static class Registry
    {
        public static void Register(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IJwtFactory, JwtFactory>();
            services.AddScoped<ITokenFactory, TokenFactory>();
            services.AddScoped<IPayslipService, PayslipService>();
            services.AddScoped<IJwtTokenHelper, JwtTokenHelper>();
            services.AddScoped<ITokenGenerator, TokenGenerator>();
            services.AddScoped<IPayslipService, PayslipService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IExcelHelpler, ExcelHelpler>();
        }
    }
}
