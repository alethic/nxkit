using System;
using System.ComponentModel.Composition;
using System.Runtime.Caching;

namespace NXKit.Web
{

    [Export(typeof(IDocumentStore))]
    public class DefaultDocumentStore :
        IDocumentStore
    {

        static readonly string KEY_FORMAT = typeof(DefaultDocumentStore).FullName + ":{0}";

        readonly MemoryCache cache;
        readonly TimeSpan cacheTime = TimeSpan.FromMinutes(15);

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        [ImportingConstructor]
        public DefaultDocumentStore()
        {
            this.cache = MemoryCache.Default;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="cacheTime"></param>
        public DefaultDocumentStore(TimeSpan cacheTime)
        {
            this.cacheTime = cacheTime;
        }

        public Document Get(string hash)
        {
            return (Document)cache.Remove(string.Format(KEY_FORMAT, hash));
        }

        public void Put(string hash, Document document)
        {
            cache.Set(hash, document, DateTimeOffset.UtcNow + cacheTime);
        }

    }

}
