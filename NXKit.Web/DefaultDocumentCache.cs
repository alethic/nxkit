using System;
using System.ComponentModel.Composition;
using System.Runtime.Caching;

namespace NXKit.Web
{

    /// <summary>
    /// Caches document state in a memory cache to later be reconstituted.
    /// </summary>
    [Export(typeof(IDocumentCache))]
    public class DefaultDocumentCache :
        IDocumentCache
    {

        static readonly string KEY_FORMAT = typeof(DefaultDocumentCache).FullName + ":{0}";

        readonly MemoryCache cache;
        readonly TimeSpan cacheTime = TimeSpan.FromMinutes(30);

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        [ImportingConstructor]
        public DefaultDocumentCache()
        {
            this.cache = MemoryCache.Default;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="cacheTime"></param>
        public DefaultDocumentCache(TimeSpan cacheTime)
            : this()
        {
            this.cacheTime = cacheTime;
        }

        public string Get(string key)
        {
            return (string)cache.Get(string.Format(KEY_FORMAT, key));
        }

        public void Set(string key, string save)
        {
            cache.Set(string.Format(KEY_FORMAT, key), save, DateTimeOffset.UtcNow + cacheTime);
        }

    }

}
