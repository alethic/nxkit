using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
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

        readonly AnnotationSerializer serializer;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="serializer"></param>
        [ImportingConstructor]
        public XmlModelDeserializer(AnnotationSerializer serializer)
        {
            Contract.Requires<ArgumentNullException>(serializer != null);

            this.serializer = serializer;
        }

        public Priority CanDeserialize(MediaRange mediaType)
        {
            return MEDIA_RANGE.Any(i => i.Matches(mediaType)) ? Priority.Default : Priority.Ignore;
        }

        public XDocument Deserialize(TextReader reader, MediaRange mediaType)
        {
            return serializer.Deserialize(XDocument.Load(reader));
        }

    }

}
