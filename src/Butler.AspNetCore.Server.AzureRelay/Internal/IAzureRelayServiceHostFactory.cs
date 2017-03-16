namespace Butler.AspNetCore.Server.AzureRelay.Internal {
    public interface IAzureRelayServiceHostFactory {
        AzureRelayServiceHost CreateServiceHost(AzureRelayServer server);
    }
}
