using System;
using System.Collections.Generic;
using System.IO;

using NXKit.Util;

namespace NXKit
{

    /// <summary>
    /// Resolver implementation.
    /// </summary>
    public class DefaultResolver :
        IResolver
    {

        readonly Dictionary<string, IResolver> resolvers = new Dictionary<string, IResolver>()
        {
            { "http", new HttpResolver() },
        };

        /// <summary>
        /// Executes the function against appropriate resolver for the given URL.
        /// </summary>
        /// <param name="href"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        Stream Resolve(Uri href, Func<IResolver, Stream> func)
        {
            var resolver = resolvers.GetOrDefault(href.Scheme);
            if (resolver != null)
                return func(resolver);

            return null;
        }

        public virtual Stream Get(Uri href)
        {
            return Resolve(href, i => i.Get(href));
        }

        public virtual Stream Put(Uri href, Stream stream)
        {
            return Resolve(href, i => i.Put(href, stream));
        }

    }

}
