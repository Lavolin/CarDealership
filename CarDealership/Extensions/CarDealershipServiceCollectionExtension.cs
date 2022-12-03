using CarDealership.Core.Contracts;
using CarDealership.Core.Contracts.Admin;
using CarDealership.Core.Services;
using CarDealership.Core.Services.Admin;
using CarDealership.Infrastructure.Data.Common;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CarDealershipServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IRepository, Repository>();           
            services.AddScoped<ICarService, CarService>();           
            services.AddScoped<IDealerService, DealerService>();
            services.AddScoped<IMotorService, MotorService>();
            services.AddScoped<ITruckService, TruckService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
