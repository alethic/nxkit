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
                yield return EngineConstants.XML;
                yield return EngineConstants.XMLSchema;
            }
        }

        public override string ResolveSchema(XNamespace ns)
        {
            if (ns == EngineConstants.XML)
                return EngineConstants.XML_HREF;
            else if (ns == EngineConstants.XMLSchema)
                return EngineConstants.XMLSchema_HREF;
            else
                return null;
        }

        public override Stream OpenSchema(string location)
        {
            if (location == EngineConstants.XML_HREF)
                return typeof(EngineSchemaPackage).Assembly.GetManifestResourceStream("ISIS.Forms.xml.xsd");
            else if (location == EngineConstants.XMLSchema_HREF)
                return typeof(EngineSchemaPackage).Assembly.GetManifestResourceStream("ISIS.Forms.xsdschema.xsd");
            else
                return null;
        }
    }

}
