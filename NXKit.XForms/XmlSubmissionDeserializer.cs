using System.ComponentModel.Composition;
using System.IO;
using System.Xml.Linq;

using NXKit.Util;

namespace NXKit.XForms
{

    [Export(typeof(ISubmissionDeserializer))]
    public class XmlsubmissionDeserializer :
        ISubmissionDeserializer
    {

        static readonly MediaRange XML_MEDIARANGE = "application/xml";

        public bool CanDeserialize(MediaRange mediaRange)
        {
            return XML_MEDIARANGE.Matches(mediaRange);
        }

        public XNode Deserialize(TextReader reader)
        {
            return XDocument.Load(reader);
        }

    }

}
