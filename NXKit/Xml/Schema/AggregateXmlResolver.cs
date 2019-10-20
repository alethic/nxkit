using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace NXKit.Xml.Schema
{

    /// <summary>
    /// Attempts to resolve XML references from a set of resolvers.
    /// </summary>
    public class AggregateXmlResolver :
        XmlUrlResolver
    {

        readonly IEnumerable<XmlResolver> resolvers;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolvers"></param>
        public AggregateXmlResolver(IEnumerable<XmlResolver> resolvers)
        {
            this.resolvers = resolvers ?? throw new ArgumentNullException(nameof(resolvers));
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolvers"></param>
        public AggregateXmlResolver(params XmlResolver[] resolvers) :
            this(resolvers?.AsEnumerable())
        {

        }

        public override Uri ResolveUri(Uri baseUri, string relativeUri)
        {
            foreach (var i in resolvers)
            {
                var result = i.ResolveUri(baseUri, relativeUri);
                if (result != null)
                    return result;
            }

            return null;
        }

        public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
        {
            foreach (var i in resolvers)
            {
                var result = i.GetEntity(absoluteUri, role, ofObjectToReturn);
                if (result != null)
                    return result;
            }

            return null;
        }

        public override async Task<object> GetEntityAsync(Uri absoluteUri, string role, Type ofObjectToReturn)
        {
            foreach (var i in resolvers)
            {
                var result = await i.GetEntityAsync(absoluteUri, role, ofObjectToReturn);
                if (result != null)
                    return result;
            }

            return null;
        }

    }

}
