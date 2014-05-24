using System;
using System.ComponentModel.Composition;
using System.Runtime.Caching;

namespace NXKit.Web
{

    [Export(typeof(ICache))]
    public class DefaultMemoryCache :
        ICache
    {

        public object Get(string key)
        {
            return MemoryCache.Default.Get(typeof(DefaultMemoryCache).FullName + ":" + key);
        }

        public void Add(string key, object value)
        {
            MemoryCache.Default.Set(typeof(DefaultMemoryCache).FullName + ":" + key, value, DateTimeOffset.UtcNow.AddMinutes(5));
        }

    }

}
