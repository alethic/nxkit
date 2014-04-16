using System.ComponentModel.Composition;
using System.IO;
using System.Xml;
using System.Xml.Linq;

using NXKit.Util;

namespace NXKit.XForms
{

    [Export(typeof(ISubmissionSerializer))]
    public class XmlSubmissionSerializer :
        ISubmissionSerializer
    {

        static readonly MediaRange XML_MEDIARANGE = "application/xml";

        public bool CanSerialize(XNode node, MediaRange mediaRange)
        {
            return XML_MEDIARANGE.Matches(mediaRange) && (node is XDocument || node is XElement);
        }

        public void Serialize(TextWriter writer, XNode node)
        {
            using (var xml = XmlWriter.Create(writer))
                node.WriteTo(xml);
        }

    }

}
