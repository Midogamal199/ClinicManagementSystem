using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ClinicManagementSystem.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();


            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

           
            services.AddAutoMapper(cfg => { }, assembly);


            return services;
        }
    }

}