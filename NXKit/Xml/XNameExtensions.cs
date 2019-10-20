using System;
using System.Xml;
using System.Xml.Linq;

namespace NXKit.Xml
{

    /// <summary>
    /// Provides various extension methods for working with <see cref="XName"/> instances.
    /// </summary>
    public static class XNameExtensions
    {

        /// <summary>
        /// Returns the <see cref="XmlQualifiedName"/> as a <see cref="XName"/>.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static XmlQualifiedName ToXmlQualifiedName(this XName self)
        {
            if (self == null)
                throw new ArgumentNullException(nameof(self));

            return new XmlQualifiedName(self.LocalName, self.NamespaceName);
        }

    }

}
