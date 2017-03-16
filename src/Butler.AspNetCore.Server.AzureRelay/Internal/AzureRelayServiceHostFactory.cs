using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Butler.AspNetCore.Server.AzureRelay.Internal {
    internal class AzureRelayServiceHostFactory : IAzureRelayServiceHostFactory {
        public AzureRelayServiceHostFactory(IOptions<AzureRelayServerOptions> options, ILoggerFactory loggerFactory) {
            _options = options;
            _loggerFactory = loggerFactory;
        }


        public AzureRelayServiceHost CreateServiceHost(AzureRelayServer server) {
            return new AzureRelayServiceHost(_options, server, _loggerFactory);
        }

        private readonly ILoggerFactory _loggerFactory;
        private readonly IOptions<AzureRelayServerOptions> _options;
    }
}
