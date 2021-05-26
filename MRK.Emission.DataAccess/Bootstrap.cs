using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MRK.Emission.DataAccess.Repositories;

namespace MRK.Emission.DataAccess
{
    public static class Bootstrap
    {
        public static IServiceCollection AddDataAccess(
           this IServiceCollection services,
           IConfiguration configuration)
        {
            return services
                .AddDbContext<EmissionContext>(config =>
                    config.UseSqlServer(configuration.GetConnectionString("default"))
                )
                .AddTransient<IEmissionRepository, EmissionRepository>();
        }
    }
}
