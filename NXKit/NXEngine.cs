using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;

using NXKit.Composition;
using NXKit.IO;
using NXKit.Serialization;

namespace NXKit
{

    /// <summary>
    /// Provides access to load NX documents.
    /// </summary>
    [Export(typeof(NXEngine))]
    public class NXEngine
    {

        readonly ICompositionContext context;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public NXEngine(ICompositionContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Loads a <see cref="Document"/> from the given <see cref="XmlReader"/>.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public Document Load(XmlReader reader)
        {
            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            return new Document(host =>
                host.Context.Resolve<AnnotationSerializer>().Deserialize(
                    XDocument.Load(
                        reader,
                        LoadOptions.PreserveWhitespace | LoadOptions.SetBaseUri)),
                context);
        }

        /// <summary>
        /// Loads a <see cref="Document"/> from the given <see cref="TextReader"/>.
        /// </summary>
        /// <param name="reader"></param>
        public Document Load(TextReader reader)
        {
            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            using (var rdr = XmlReader.Create(reader))
                return Load(rdr);
        }

        /// <summary>
        /// Loads a <see cref="Document"/> from the given <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public Document Load(Stream stream)
        {
            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            using (var rdr = new StreamReader(stream))
                return Load(rdr);
        }

        /// <summary>
        /// Loads a <see cref="Document"/> from the given <see cref="Uri"/>.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public Document Load(Uri uri)
        {
            if (uri is null)
                throw new ArgumentNullException(nameof(uri));

            return new Document(host =>
                host.Context.Resolve<AnnotationSerializer>().Deserialize(
                    XDocument.Load(
                        NXKit.Xml.IOXmlReader.Create(
                            host.Context.Resolve<IIOService>(),
                            uri),
                        LoadOptions.PreserveWhitespace | LoadOptions.SetBaseUri)),
                context);
        }

        /// <summary>
        /// Loads a <see cref="Document"/> from the given <see cref="XDocument"/>.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public Document Load(XDocument document)
        {
            if (document is null)
                throw new ArgumentNullException(nameof(document));

            return Load(document.CreateReader());
        }

        /// <summary>
        /// Loads a <see cref="Document"/> by parsing the given input XML.
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public Document Parse(string xml)
        {
            if (xml is null)
                throw new ArgumentNullException(nameof(xml));
            if (string.IsNullOrWhiteSpace(xml))
                throw new ArgumentOutOfRangeException(nameof(xml));

            return Load(XDocument.Parse(xml));
        }

    }

}
