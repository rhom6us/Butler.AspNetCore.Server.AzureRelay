using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Butler.AspNetCore.Server.AzureRelay.Internal {
    public class DelegateInstanceProviderServiceBehavior : IServiceBehavior {
        public DelegateInstanceProviderServiceBehavior(Func<object> factory) {
            _factory = factory;
        }

        public static implicit operator DelegateInstanceProviderServiceBehavior(Func<object> factory) {
            return new DelegateInstanceProviderServiceBehavior(factory);
        }


        void IServiceBehavior.Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase) {}

        void IServiceBehavior.AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters) {}

        void IServiceBehavior.ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase) {
            foreach (var channelDispatcher in serviceHostBase.ChannelDispatchers.OfType<ChannelDispatcher>()) {
                foreach (var endpointDispatcher in channelDispatcher.Endpoints) {
                    endpointDispatcher.DispatchRuntime.InstanceProvider = new DelegateInstanceProvider(_factory);
                }
            }
        }

        private readonly Func<object> _factory;
    }
}
