using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace NXKit.XForms
{

    [SchemaPackage]
    public sealed class SchemaPackage : NXKit.SchemaPackage
    {

        public override IEnumerable<XNamespace> Namespaces
        {
            get
            {
                yield return Constants.XForms_1_0;
            }
        }

        public override string ResolveSchema(XNamespace ns)
        {
            if (ns == Constants.XForms_1_0)
                return Constants.XForms_1_0_HREF;
            else
                return null;
        }

        public override Stream OpenSchema(string location)
        {
            if (location == Constants.XForms_1_0_HREF)
                return typeof(XFormsModule).Assembly.GetManifestResourceStream("ISIS.Forms.XForms.XForms-Schema.xsd");
            else
                return null;
        }

    }

}
