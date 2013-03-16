using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace XEngine.Forms
{

    [SchemaPackage]
    public class FormSchemaPackage : SchemaPackage
    {
        public override IEnumerable<XNamespace> Namespaces
        {
            get
            {
                yield return FormConstants.XML;
                yield return FormConstants.XMLSchema;
            }
        }

        public override string ResolveSchema(XNamespace ns)
        {
            if (ns == FormConstants.XML)
                return FormConstants.XML_HREF;
            else if (ns == FormConstants.XMLSchema)
                return FormConstants.XMLSchema_HREF;
            else
                return null;
        }

        public override Stream OpenSchema(string location)
        {
            if (location == FormConstants.XML_HREF)
                return typeof(FormSchemaPackage).Assembly.GetManifestResourceStream("ISIS.Forms.xml.xsd");
            else if (location == FormConstants.XMLSchema_HREF)
                return typeof(FormSchemaPackage).Assembly.GetManifestResourceStream("ISIS.Forms.xsdschema.xsd");
            else
                return null;
        }
    }

}
