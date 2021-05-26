using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MRK.Emission.Business
{
    public static class Bootstrap
    {
        private const string DefaultSectionName = "Business";
     
        public static IServiceCollection AddBusiness(
            this IServiceCollection services,
            IConfiguration config,
            string sectionName = DefaultSectionName)
        {

            return services;
        }
    }
}
