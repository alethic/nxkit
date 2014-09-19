using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

using NXKit.IO.Media;

namespace NXKit.XForms.Serialization
{

    [Export(typeof(IModelSerializer))]
    public class XmlModelSerializer :
        IModelSerializer
    {

        static readonly MediaRange[] MEDIA_RANGE = new MediaRange[]
        {
            "application/xml",
            "text/xml",
        };

        public Priority CanSerialize(XNode node, MediaRange mediaType)
        {
            return MEDIA_RANGE.Any(i => i.Matches(mediaType)) && (node is XDocument || node is XElement) ? Priority.Default : Priority.Ignore;
        }

        public void Serialize(TextWriter writer, XNode node, MediaRange mediaType)
        {
            using (var xml = XmlWriter.Create(writer))
                node.WriteTo(xml);
        }

    }

}
