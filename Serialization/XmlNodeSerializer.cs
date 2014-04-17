using System.ComponentModel.Composition;
using System.IO;
using System.Xml;
using System.Xml.Linq;

using NXKit.Util;

namespace NXKit.Serialization
{

    [Export(typeof(INodeSerializer))]
    public class XmlNodeSerializer :
        INodeSerializer
    {

        static readonly MediaRange XML_MEDIARANGE = "application/xml";

        public Priority CanSerialize(XNode node, MediaRange mediaType)
        {
            return XML_MEDIARANGE.Matches(mediaType) && (node is XDocument || node is XElement) ? Priority.Default : Priority.Ignore;
        }

        public void Serialize(TextWriter writer, XNode node, MediaRange mediaType)
        {
            using (var xml = XmlWriter.Create(writer))
                node.WriteTo(xml);
        }

    }

}
