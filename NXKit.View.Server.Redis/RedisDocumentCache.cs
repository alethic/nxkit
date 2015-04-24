using System;
using System.ComponentModel.Composition;

using StackExchange.Redis;

namespace NXKit.View.Server
{

    /// <summary>
    /// Caches document state in a memory cache to later be reconstituted.
    /// </summary>
    [Export(typeof(IDocumentCache))]
    public class RedisDocumentCache :
        IDocumentCache
    {

        static readonly string KEY_FORMAT = typeof(RedisDocumentCache).FullName + ":{0}";

        readonly ConnectionMultiplexer cache;
        readonly TimeSpan cacheTime = TimeSpan.FromHours(4);

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        [ImportingConstructor]
        public RedisDocumentCache()
        {
            this.cache = ConnectionMultiplexer.Connect("");
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="cacheTime"></param>
        public RedisDocumentCache(TimeSpan cacheTime)
            : this()
        {
            this.cacheTime = cacheTime;
        }

        public string Get(string key)
        {
            var db = cache.GetDatabase();
            return (string)db.StringGet(string.Format(KEY_FORMAT, key));
        }

        public void Set(string key, string save)
        {
            var db = cache.GetDatabase();
            db.StringSet(string.Format(KEY_FORMAT, key), save, cacheTime);
        }

    }

}
