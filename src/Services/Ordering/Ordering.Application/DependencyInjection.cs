using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Ordering.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>    // questo deve esser registrato prima dei servizi di infrastruttura
            {                             // e poi tramite il mediatr ho gli interceptor funzionanti
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });
            return services;
        }
    }
}
                                //mediatr ----> interceptor ----> altri servizi