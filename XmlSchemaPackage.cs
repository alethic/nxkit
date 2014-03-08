using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace NXKit
{

    /// <summary>
    /// Schema package for core-NXKit schemas.
    /// </summary>
    public class XmlSchemaPackage : SchemaPackage
    {

        public override IEnumerable<XNamespace> Namespaces
        {
            get
            {
                yield return XmlSchemaConstants.XML;
                yield return XmlSchemaConstants.XMLSchema;
            }
        }

        public override string ResolveSchema(XNamespace ns)
        {
            if (ns == XmlSchemaConstants.XML)
                return XmlSchemaConstants.XML_HREF;
            else if (ns == XmlSchemaConstants.XMLSchema)
                return XmlSchemaConstants.XMLSchema_HREF;
            else
                return null;
        }

        public override Stream OpenSchema(string location)
        {
            return null;
        }
    }

}
