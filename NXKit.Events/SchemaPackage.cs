using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace NXKit.Events
{

    /// <summary>
    /// Provides XML-events schema.
    /// </summary>
    public sealed class SchemaPackage : NXKit.SchemaPackage
    {

        public override IEnumerable<XNamespace> Namespaces
        {
            get
            {
                yield return SchemaConstants.Events_1_0;
            }
        }

        public override string ResolveSchema(XNamespace ns)
        {
            if (ns == SchemaConstants.Events_1_0)
                return SchemaConstants.Events_1_0_HREF;
            else
                return null;
        }

        public override Stream OpenSchema(string location)
        {
            if (location == SchemaConstants.Events_1_0_HREF)
                return typeof(EventsModule).Assembly.GetManifestResourceStream("NXKit.Events.xml-events-attribs-1.xsd");
            else
                return null;
        }

    }

}
