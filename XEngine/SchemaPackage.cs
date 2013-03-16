using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace XEngine
{

    public abstract class SchemaPackage
    {

        /// <summary>
        /// Gets the namespaces of the schema this package makes available.
        /// </summary>
        public abstract IEnumerable<XNamespace> Namespaces { get; }

        /// <summary>
        /// Resolve the location of the schema's XSD.
        /// </summary>
        /// <param name="ns"></param>
        /// <returns></returns>
        public abstract string ResolveSchema(XNamespace ns);

        /// <summary>
        /// Provides a new stream of the given schema location's XSD data.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public abstract Stream OpenSchema(string location);

    }

}
