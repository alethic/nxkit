using System.ComponentModel.Composition;
using System.IO;
using System.Xml.Linq;

using NXKit.Util;

namespace NXKit.Serialization
{

    [Export(typeof(INodeDeserializer))]
    public class XmlNodeDeserializer :
        INodeDeserializer
    {

        static readonly MediaRange XML_MEDIARANGE = "application/xml";

        public Priority CanDeserialize(MediaRange mediaRange)
        {
            return XML_MEDIARANGE.Matches(mediaRange) ? Priority.Default : Priority.Ignore;
        }

        public XNode Deserialize(TextReader reader, MediaRange mediaType)
        {
            return XDocument.Load(reader);
        }

    }

}
