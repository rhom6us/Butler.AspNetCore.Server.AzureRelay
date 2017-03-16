using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.ServiceBus;

namespace Butler.AspNetCore.Server.AzureRelay.Internal {
    public class AzureRelayServiceHost : ServiceHost {
        public AzureRelayServiceHost(IOptions<AzureRelayServerOptions> options, AzureRelayServer server, ILoggerFactory loggerFactory) : base(typeof(RelayService), new Uri(options.Value.BaseAddress)) {
            _server = server;
            _loggerFactory = loggerFactory;
            _serverOptions = options.Value;
            _log = loggerFactory.CreateLogger(GetType());
        }


        protected override void OnOpen(TimeSpan timeout) {
            var instancing = Description.Behaviors.Find<DelegateInstanceProviderServiceBehavior>();
            if (instancing == null) {
                instancing = new Func<object>(() => new RelayService(_server.CreateClient(), _loggerFactory));
                Description.Behaviors.Add(instancing);
            }

            var mode = BaseAddresses.Single().Scheme == Uri.UriSchemeHttps ? EndToEndWebHttpSecurityMode.Transport : EndToEndWebHttpSecurityMode.None;
            var binding = new WebHttpRelayBinding(mode, RelayClientAuthenticationType.None);
            //binding.TransferMode = TransferMode.Streamed;

            var endpoint = AddServiceEndpoint(typeof(RelayService), binding, string.Empty);
            endpoint.Behaviors.Add(new WebHttpBehavior());
            endpoint.Behaviors.Add(new TransportClientEndpointBehavior(_serverOptions.SharedAccessKey));

            base.OnOpen(timeout);
        }

        #region Overrides of ServiceHost

        protected override void ApplyConfiguration() {}

        #endregion

        private readonly ILogger _log;
        private readonly ILoggerFactory _loggerFactory;
        private readonly AzureRelayServer _server;
        private readonly AzureRelayServerOptions _serverOptions;
    }
}
