using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace XEngine.Layout
{

    [SchemaPackage]
    public sealed class SchemaPackage : XEngine.SchemaPackage
    {

        public override IEnumerable<XNamespace> Namespaces
        {
            get
            {
                yield return Constants.Layout_1_0;
            }
        }

        public override string ResolveSchema(XNamespace ns)
        {
            if (ns == Constants.Layout_1_0)
                return Constants.Layout_1_0_HREF;
            else
                return null;
        }

        public override Stream OpenSchema(string location)
        {
            if (location == Constants.Layout_1_0_HREF)
                return typeof(LayoutModule).Assembly.GetManifestResourceStream("ISIS.Forms.Layout.Layout-1.0.xsd");
            else
                return null;
        }

    }

}
