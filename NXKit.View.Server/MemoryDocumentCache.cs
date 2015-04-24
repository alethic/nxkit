using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Runtime.Caching;

namespace NXKit.View.Server
{

    /// <summary>
    /// Caches document state in a memory cache to later be reconstituted.
    /// </summary>
    [Export(typeof(IDocumentCache))]
    public class MemoryDocumentCache :
        IDocumentCache
    {

        static readonly string KEY_FORMAT = typeof(MemoryDocumentCache).FullName + ":{0}";

        readonly MemoryCache cache;
        readonly TimeSpan cacheTime = TimeSpan.FromMinutes(30);

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        [ImportingConstructor]
        public MemoryDocumentCache()
        {
            this.cache = MemoryCache.Default;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="cacheTime"></param>
        public MemoryDocumentCache(TimeSpan cacheTime)
            : this()
        {
            Contract.Requires<ArgumentOutOfRangeException>(cacheTime > TimeSpan.Zero);

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
