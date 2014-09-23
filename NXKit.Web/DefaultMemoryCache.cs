using System;
using System.ComponentModel.Composition;
using System.Runtime.Caching;

namespace NXKit.Web
{

    [Export(typeof(IDocumentCache))]
    public class DefaultMemoryCache :
        IDocumentCache
    {

        readonly MemoryCache cache;
        readonly TimeSpan cacheTime = TimeSpan.FromMinutes(5);

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        [ImportingConstructor]
        public DefaultMemoryCache()
        {
            this.cache = MemoryCache.Default;
        }

        public string Get(string key)
        {
            return (string)cache.Get(typeof(DefaultMemoryCache).FullName + ":" + key);
        }

        public void Set(string key, string save)
        {
            cache.Set(typeof(DefaultMemoryCache).FullName + ":" + key, save, DateTimeOffset.UtcNow + cacheTime);
        }

    }

}
