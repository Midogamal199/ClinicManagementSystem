using System.Reflection;
using ClinicManagementSystem.Application.Common.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ClinicManagementSystem.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();


            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

           
            services.AddAutoMapper(cfg => { }, assembly);
            services.AddValidatorsFromAssembly(assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));



            return services;
        }
    }

}