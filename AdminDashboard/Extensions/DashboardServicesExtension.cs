using AdminDashboard.Helppers;
using Talabat.Core;
using Talabat.Infrastructure;

namespace AdminDashboard.Extensions
{
    public static class DashboardServicesExtension
    {
        public static IServiceCollection AddDashboardServices(this IServiceCollection services)
        {

            services.AddScoped<IUniteOfWork, UniteOfWork>();

            services.AddAutoMapper(typeof(MappsProfile));
            

            return services;
        }
    }
}
