using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MRK.Emission.GrpcClient.Contracts;
using ProtoBuf.Grpc.Client;
using ProtoBuf.Grpc.ClientFactory;
using System;
using System.Net.Http;

namespace MRK.Emission.GrpcClient
{
    public static class DependencyInjection
    {
        private const string DefaultSection = "emission:url";

        /// <summary>
        /// Adds IEmissionClientService into service collection.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        public static IServiceCollection AddEmissionClient(
            this IServiceCollection services,
            IConfiguration configuration,
            string sectionName = DefaultSection)
        {
            GrpcClientFactory.AllowUnencryptedHttp2 = true;
            var serviceUrl = configuration.GetSection(sectionName);

            return services
                .AddCodeFirstGrpcClient<IEmissionClientService>(opts => ConfigureHttp(opts, serviceUrl.Value)).Services;
        }

        private static void ConfigureHttp(Grpc.Net.ClientFactory.GrpcClientFactoryOptions options, string url)
        {
            options.Address = new Uri(url);
            options.ChannelOptionsActions.Add(channelOpts =>
            {
                channelOpts.HttpClient = new HttpClient(new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback =
                        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                });
            });
        }
    }
}
