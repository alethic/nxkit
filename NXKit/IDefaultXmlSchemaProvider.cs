using System.Collections.Generic;
using System.Xml.Schema;

namespace NXKit
{

    /// <summary>
    /// Provides a default set of schemas that should be made available.
    /// </summary>
    public interface IDefaultXmlSchemaProvider
    {

        /// <summary>
        /// Provides a default set of schemas that should be made available.
        /// </summary>
        /// <returns></returns>
        IEnumerable<XmlSchema> GetSchemas();

    }

}
