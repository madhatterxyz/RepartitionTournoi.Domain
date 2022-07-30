using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RepartitionTournoi.DAL;
using RepartitionTournoi.DAL.Entities;
using RepartitionTournoi.DAL.Interfaces;

namespace RepartitionTournoi.Domain
{
    public static class ServiceCollectionsExtensions
    {
        public static IServiceCollection RegisterDALServices(this IServiceCollection services)
        {
            services.AddScoped<IJeuDAL, JeuDAL>();
            services.AddScoped<IJoueurDAL, JoueurDAL>();
            services.AddDbContext<RepartitionTournoiContext>(
                options => options.UseSqlServer("name=ConnectionStrings:RepartitionTournoiContext"));

            return services;
        }
    }
}
