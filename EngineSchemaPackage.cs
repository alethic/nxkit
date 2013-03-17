using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace NXKit
{

    [SchemaPackage]
    public class EngineSchemaPackage : SchemaPackage
    {
        public override IEnumerable<XNamespace> Namespaces
        {
            get
            {
                yield return SchemaConstants.XML;
                yield return SchemaConstants.XMLSchema;
            }
        }

        public override string ResolveSchema(XNamespace ns)
        {
            if (ns == SchemaConstants.XML)
                return SchemaConstants.XML_HREF;
            else if (ns == SchemaConstants.XMLSchema)
                return SchemaConstants.XMLSchema_HREF;
            else
                return null;
        }

        public override Stream OpenSchema(string location)
        {
            if (location == SchemaConstants.XML_HREF)
                return typeof(EngineSchemaPackage).Assembly.GetManifestResourceStream("ISIS.Forms.xml.xsd");
            else if (location == SchemaConstants.XMLSchema_HREF)
                return typeof(EngineSchemaPackage).Assembly.GetManifestResourceStream("ISIS.Forms.xsdschema.xsd");
            else
                return null;
        }
    }

}
