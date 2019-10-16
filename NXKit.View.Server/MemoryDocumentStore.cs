using System;
using System.Runtime.Caching;

using NXKit.Composition;

namespace NXKit.View.Server
{

    [Export(typeof(IDocumentStore))]
    public class MemoryDocumentStore :
        IDocumentStore
    {

        static readonly string KEY_FORMAT = typeof(MemoryDocumentStore).FullName + ":{0}";

        readonly MemoryCache cache;
        readonly TimeSpan cacheTime = TimeSpan.FromMinutes(15);

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public MemoryDocumentStore()
        {
            this.cache = MemoryCache.Default;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="cacheTime"></param>
        public MemoryDocumentStore(TimeSpan cacheTime)
        {
            if (cacheTime <= TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(cacheTime));

            this.cacheTime = cacheTime;
        }

        public Document Get(string hash)
        {
            return (Document)cache.Remove(string.Format(KEY_FORMAT, hash));
        }

        public void Put(string hash, Document document)
        {
            cache.Set(string.Format(KEY_FORMAT, hash), document, DateTimeOffset.UtcNow + cacheTime);
        }

    }

}
