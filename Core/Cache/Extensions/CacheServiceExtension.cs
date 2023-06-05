using Cache.Services;
using Hazelcast;
using Microsoft.Extensions.DependencyInjection;

namespace Cache.Extensions
{
    public static class CacheServiceExtension
    {
        public static void AddCacheService(this IServiceCollection services, HazelcastOptions hazelcastOptions)
        {
            services.AddSingleton<ICitiesService, CitiesService>(service =>
                new CitiesService(hazelcastOptions)
            );
            services.AddSingleton<ICountriesService, CountriesService>(service =>
                new CountriesService(hazelcastOptions)
            );

            services.AddSingleton<IOrderService, OrderService>(service =>
                new OrderService(hazelcastOptions)
            );
        }
    }
}
