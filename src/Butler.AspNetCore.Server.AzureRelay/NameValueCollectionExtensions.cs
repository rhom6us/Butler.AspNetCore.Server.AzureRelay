using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Butler.AspNetCore.Server.AzureRelay {
    internal static class NameValueCollectionExtensions {
        public static Dictionary<string, string[]> ToDictionary(this NameValueCollection collection) {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            return collection.AllKeys.ToDictionary(p => p, collection.GetValues);
        }
    }
}
