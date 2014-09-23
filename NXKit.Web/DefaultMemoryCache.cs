using System;
using System.ComponentModel.Composition;
using System.Runtime.Caching;

namespace NXKit.Web
{

    [Export(typeof(ICache))]
    public class DefaultMemoryCache :
        ICache
    {

        readonly MemoryCache cache;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        [ImportingConstructor]
        public DefaultMemoryCache()
        {
            this.cache = MemoryCache.Default;
        }

        public T Get<T>(string key, bool remove)
            where T : class
        {
            var o = (T)cache.Get(typeof(DefaultMemoryCache).FullName + ":" + key);
            if (o != null)
                if (remove)
                    cache.Remove(key);

            return o;
        }

        public void Add<T>(string key, T value)
            where T : class
        {
            cache.Set(typeof(DefaultMemoryCache).FullName + ":" + key, value, DateTimeOffset.UtcNow.AddMinutes(5));
        }

    }

}
