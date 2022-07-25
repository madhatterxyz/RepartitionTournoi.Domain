using Microsoft.Extensions.DependencyInjection;
using RepartitionTournoi.DAL;
using RepartitionTournoi.DAL.Interfaces;

namespace RepartitionTournoi.Domain
{
    public static class ServiceCollectionsExtensions
    {
        public static IServiceCollection RegisterDALServices(this IServiceCollection services)
        {
            services.AddScoped<IJeuDAL, JeuDAL>();
            services.AddScoped<IJoueurDAL, JoueurDAL>();

            return services;
        }
    }
}
