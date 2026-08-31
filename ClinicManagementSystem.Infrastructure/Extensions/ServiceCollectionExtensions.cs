using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.Interfaces;
using ClinicManagementSystem.Domain.Interfaces;
using ClinicManagementSystem.Infrastructure.ExternalServices;
using ClinicManagementSystem.Infrastructure.Identity;
using ClinicManagementSystem.Infrastructure.Persistence;
using ClinicManagementSystem.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClinicManagementSystem.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(
          this IServiceCollection services,
          IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.Configure<PaymobOptions>(configuration.GetSection("Paymob"));
            services.AddHttpClient<IPaymentGatewayService, PaymobPaymentGatewayService>();
            services.AddScoped<IWebhookSignatureValidator, PaymobWebhookSignatureValidator>();
            services.AddMemoryCache();
            services.Configure<MailtrapOptions>(configuration.GetSection("Mailtrap"));
            services.AddScoped<IEmailService, MailtrapEmailService>();
            services.AddScoped<IOtpService, OtpService>();
            return services;
        }
    }
}