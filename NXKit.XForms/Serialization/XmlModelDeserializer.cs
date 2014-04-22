using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Xml.Linq;

using NXKit.Serialization;
using NXKit.Util;

namespace NXKit.XForms.Serialization
{

    [Export(typeof(IModelDeserializer))]
    public class XmlModelDeserializer :
        IModelDeserializer
    {

        static readonly MediaRange[] MEDIA_RANGE = new MediaRange[]
        {
            "application/xml",
            "text/xml",
        };

        public Priority CanDeserialize(MediaRange mediaType)
        {
            return MEDIA_RANGE.Any(i => i.Matches(mediaType)) ? Priority.Default : Priority.Ignore;
        }

        public XDocument Deserialize(TextReader reader, MediaRange mediaType)
        {
            return XNodeAnnotationSerializer.Deserialize(XDocument.Load(reader));
        }

    }

}
