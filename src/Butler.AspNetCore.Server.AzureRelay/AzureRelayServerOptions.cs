using Microsoft.ServiceBus;

namespace Butler.AspNetCore.Server.AzureRelay {
    public class AzureRelayServerOptions {
        public string BaseAddress { get; set; }
        public SharedAccessKey SharedAccessKey { get; set; }

        public AzureRelayServerOptions() {
            SharedAccessKey = new SharedAccessKey();
        }
    }

    public class SharedAccessKey {
        public string Name { get; set; }
        public string Key { get; set; }
        public SharedAccessKey() {}

        public SharedAccessKey(string key, string name) {
            Key = key;
            Name = name;
        }

        public static implicit operator TokenProvider(SharedAccessKey key) {
            return TokenProvider.CreateSharedAccessSignatureTokenProvider(key.Name, key.Key);
        }
    }
}
