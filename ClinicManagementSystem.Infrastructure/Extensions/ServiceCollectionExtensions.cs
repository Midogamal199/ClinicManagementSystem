using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Application.Interfaces;
using ClinicManagementSystem.Domain.Interfaces;
using ClinicManagementSystem.Infrastructure.ExternalServices;
using ClinicManagementSystem.Infrastructure.Identity;
using ClinicManagementSystem.Infrastructure.Jobs;
using ClinicManagementSystem.Infrastructure.Persistence;
using ClinicManagementSystem.Infrastructure.Repositories;
using Hangfire;
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
            // Database Context
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Identity Setup
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

            // Hangfire Configuration & Storage
            services.AddHangfire(config => config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection")));

            // Hangfire Server (Background Worker Process)
            services.AddHangfireServer();

            // Repositories & Unit Of Work
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IIdentityService, IdentityService>();

            // External Services (Payment, Mail, Security)
            services.Configure<PaymobOptions>(configuration.GetSection("Paymob"));
            services.AddHttpClient<IPaymentGatewayService, PaymobPaymentGatewayService>();
            services.AddScoped<IWebhookSignatureValidator, PaymobWebhookSignatureValidator>();
            services.AddMemoryCache();
            services.Configure<MailtrapOptions>(configuration.GetSection("Mailtrap"));
            services.AddScoped<IEmailService, MailtrapEmailService>();
            services.AddScoped<IOtpService, OtpService>();
            services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IFileStorageService, LocalFileStorageService>();
            services.AddScoped<IDashboardRepository, DashboardRepository>();

            // Hangfire Background Jobs
            services.AddScoped<IAppointmentReminderJob, AppointmentReminderJob>();
            services.AddScoped<ILabTestNotificationJob, LabTestNotificationJob>();
            services.AddScoped<IDatabaseBackupJob, DatabaseBackupJob>();

            return services;
        }
    }
}