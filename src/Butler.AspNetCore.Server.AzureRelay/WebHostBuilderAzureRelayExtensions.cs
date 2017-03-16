using System;
using Butler.AspNetCore.Server.AzureRelay;
using Butler.AspNetCore.Server.AzureRelay.Internal;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace Microsoft.AspNetCore.Hosting {
    public static class WebHostBuilderAzureRelayExtensions {
        public static IWebHostBuilder UseAzureRelay(this IWebHostBuilder hostBuilder, AzureRelayServerOptions options) {
            return UseAzureRelay(hostBuilder, options.BaseAddress, options.SharedAccessKey.Name, options.SharedAccessKey.Key);
        }

        public static IWebHostBuilder UseAzureRelay(this IWebHostBuilder hostBuilder, string baseAddress, string sharedAccessKeyName, string sharedAccessKey) {
            return hostBuilder
                .UseAzureRelay(options => {
                    options.BaseAddress = baseAddress;
                    options.SharedAccessKey = new SharedAccessKey {
                        Name = sharedAccessKeyName,
                        Key = sharedAccessKey
                    };
                });
        }


        public static IWebHostBuilder UseAzureRelay(this IWebHostBuilder hostBuilder, Action<AzureRelayServerOptions> options) {
            return hostBuilder.ConfigureServices(services => {
                services.Configure(options);
                services.AddSingleton<IServer, AzureRelayServer>();
                services.AddSingleton<IAzureRelayServiceHostFactory, AzureRelayServiceHostFactory>();
            });
        }
    }
}
