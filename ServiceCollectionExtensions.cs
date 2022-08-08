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
            services.AddScoped<ITournoiDAL, TournoiDAL>();
            services.AddScoped<IJoueurDAL, JoueurDAL>();
            services.AddScoped<IMatchDAL,MatchDAL>();
            services.AddScoped<ICompositionDAL, CompositionDAL>();
            services.AddScoped<IScoreDAL, ScoreDAL>();
            services.AddDbContext<RepartitionTournoiContext>(
                options => options.UseSqlServer("name=ConnectionStrings:RepartitionTournoiContext"));

            return services;
        }
    }
}
