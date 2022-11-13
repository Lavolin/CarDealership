using CarDealership.Core.Contracts;
using CarDealership.Core.Services;
using CarDealership.Infrastructure.Data.Common;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CarDealershipServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IRepository, Repository>();           
            services.AddScoped<ICarService, CarService>();           

            return services;
        }
    }
}
