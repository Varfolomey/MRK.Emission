using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MRK.Emission.Api.HostedServices;
using MRK.Emission.Business;
using MRK.Emission.DataAccess;
using MRK.Emission.GrpcClient;
using MRK.Emission.Service;
using ProtoBuf.Grpc.Server;

namespace MRK.Emission.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddOptions()
                .AddMediatR(typeof(Startup), typeof(Business.Bootstrap))
                .AddDataAccess(_configuration)
                .AddBusiness(_configuration)
                .AddEmissionServices(_configuration)
                .AddEmissionClient(_configuration)
                .AddHostedService<SuzHostedService>()
                .AddCodeFirstGrpc();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            TypeAdapterConfig.GlobalSettings.Scan(typeof(Startup).Assembly, typeof(Business.Bootstrap).Assembly);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<EmissionService>();
            });
        }
    }
}
