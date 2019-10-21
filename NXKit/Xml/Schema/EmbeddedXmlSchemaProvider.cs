using System.Collections.Generic;
using System.Xml.Schema;

using NXKit.Composition;

namespace NXKit.Xml.Schema
{

    /// <summary>
    /// Provides some default embedded schemas.
    /// </summary>
    [Export(typeof(IDefaultXmlSchemaProvider))]
    public class EmbeddedXmlSchemaProvider : IDefaultXmlSchemaProvider
    {

        public IEnumerable<XmlSchema> GetSchemas()
        {
            return Xsd.Schemas;
        }

    }

}
