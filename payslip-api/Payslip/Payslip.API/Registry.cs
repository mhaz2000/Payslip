using Payslip.API.Helpers;
using Payslip.Application.Helpers;
using Payslip.Application.Services;
using Payslip.Core.Repositories.Base;
using Payslip.Infrastructure.Repositories.Base;

namespace Payslip.API
{
    public class Registry
    {
        public static void Register(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IJwtFactory, JwtFactory>();
            services.AddScoped<ITokenFactory, TokenFactory>();
            services.AddScoped<IPayslipService, PayslipService>();
            services.AddScoped<IJwtTokenHelper, JwtTokenHelper>();
            services.AddScoped<ITokenGenerator, TokenGenerator>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IPayslipExtractorHelpler, PayslipExtractorHelper>();
        }
    }
}
