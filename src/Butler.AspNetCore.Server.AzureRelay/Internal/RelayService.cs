using System.IO;
using System.Linq;
using System.Net.Http;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Butler.AspNetCore.Server.AzureRelay.Internal {
    [ServiceContract, ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class RelayService {
        public RelayService(HttpClient client, ILoggerFactory loggerFactory) {
            _client = client;
            _log = loggerFactory.CreateLogger(GetType());
        }


        [OperationContract, WebInvoke(UriTemplate = "*", Method = "*")]
        public async Task<Stream> Relay(Stream stream) {
            var request = new HttpRequestMessage {
                RequestUri = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.RequestUri,
                Method = new HttpMethod(WebOperationContext.Current.IncomingRequest.Method),
                Content = stream != null ? new StreamContent(stream) : null
            };
            foreach (var header in WebOperationContext.Current.IncomingRequest.Headers.ToDictionary()) {
                if (request.Headers.TryAddWithoutValidation(header.Key, header.Value))
                    continue;
                if ((request.Content != null) && request.Content.Headers.TryAddWithoutValidation(header.Key, header.Value))
                    continue;
                _log.LogWarning("Skipping invalid header {0}", header.Key);
            }

            var response = await _client.SendAsync(request);

            foreach (var header in response.Headers.Union(response.Content.Headers).SelectMany(h => h.Value.ToDictionary(val => h.Key))) {
                WebOperationContext.Current.OutgoingResponse.Headers.Add(header.Key, header.Value);
            }
            WebOperationContext.Current.OutgoingResponse.StatusCode = response.StatusCode;
            return await response.Content.ReadAsStreamAsync();
        }

        private readonly HttpClient _client;
        private readonly ILogger _log;
    }
}
