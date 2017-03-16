using System;
using System.Linq;
using System.Net.Http;
using System.ServiceModel;
using Butler.AspNetCore.Server.AzureRelay.Internal;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Butler.AspNetCore.Server.AzureRelay {
    public class AzureRelayServer : IServer {
        public AzureRelayServer(IOptions<AzureRelayServerOptions> options, ILoggerFactory loggerFactory, IAzureRelayServiceHostFactory serviceHostFactory) {
            _serverOptions = options.Value;
            _log = loggerFactory.CreateLogger(GetType());
            _host = serviceHostFactory.CreateServiceHost(this);
        }

        public HttpClient CreateClient() {
            var handler = new ClientHandler(PathString.FromUriComponent(new Uri(_serverOptions.BaseAddress)), _application);
            return new HttpClient(handler) {BaseAddress = new Uri(_serverOptions.BaseAddress)};
        }

        public void Dispose() {
            _host?.Close();
            _host = null;
        }


        void IServer.Start<TContext>(IHttpApplication<TContext> application) {
            _log.LogInformation("HttpApplication Start");
            _application = (IHttpApplication<HostingApplication.Context>) application;


            _host.Open();
            _log.LogDebug($"ServiceHost is open at {_host.BaseAddresses.Single()}");
        }


        IFeatureCollection IServer.Features { get; } = new FeatureCollection();
        private readonly ILogger _log;
        private readonly AzureRelayServerOptions _serverOptions;
        private IHttpApplication<HostingApplication.Context> _application;
        private ServiceHost _host;
    }
}
