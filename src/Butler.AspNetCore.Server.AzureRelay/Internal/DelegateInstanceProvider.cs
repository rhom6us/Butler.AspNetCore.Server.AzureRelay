using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace Butler.AspNetCore.Server.AzureRelay.Internal {
    public class DelegateInstanceProvider : IInstanceProvider {
        public DelegateInstanceProvider(Func<object> factory) {
            _factory = factory;
        }

        object IInstanceProvider.GetInstance(InstanceContext instanceContext) {
            return _factory();
        }

        object IInstanceProvider.GetInstance(InstanceContext instanceContext, Message message) {
            return _factory();
        }

        void IInstanceProvider.ReleaseInstance(InstanceContext instanceContext, object instance) {}
        private readonly Func<object> _factory;
    }
}
