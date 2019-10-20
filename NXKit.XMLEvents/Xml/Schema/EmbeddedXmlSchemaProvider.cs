using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;

using NXKit.Composition;

namespace NXKit.XMLEvents.Xml.Schema
{

    /// <summary>
    /// Provides some default embedded schema.
    /// </summary>
    [Export(typeof(IDefaultXmlSchemaProvider))]
    public class EmbeddedXmlSchemaProvider : IDefaultXmlSchemaProvider
    {

        public IEnumerable<XmlSchema> GetSchemas()
        {
            return Xsd.SchemaSet.Schemas().Cast<XmlSchema>();
        }

    }

}
