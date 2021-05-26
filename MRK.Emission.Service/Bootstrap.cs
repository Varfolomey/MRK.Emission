using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MRK.Emission.Service.SUZ;

namespace MRK.Emission.Service
{
    public static class Bootstrap
    {
        private const string DefaultSectionName = "SuzService";

        public static IServiceCollection AddEmissionServices(
            this IServiceCollection services,
            IConfiguration config,
            string sectionName = DefaultSectionName)
        {
            services
                .Configure<SuzServiceSettings>(opts => config.GetSection(sectionName).Bind(opts))
                .AddScoped<ISuzService, SuzService>();

            return services;
        }
    }
}
