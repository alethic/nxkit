using System;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

using NXKit.Serialization;
using NXKit.Util;

namespace NXKit
{

    /// <summary>
    /// Hosts an NXKit document. Provides access to the visual tree for a renderer or other processor.
    /// </summary>
    public class NXDocumentHost
    {

        /// <summary>
        /// Initializes the static instance.
        /// </summary>
        static NXDocumentHost()
        {
            ResourceUriParser.Register();
            ResourceWebRequestFactory.Register();
        }

        /// <summary>
        /// Loads a <see cref="NXDocumentHost"/> from the given <see cref="XmlReader"/>.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static NXDocumentHost Load(XmlReader reader)
        {
            Contract.Requires<ArgumentNullException>(reader != null);

            return new NXDocumentHost(
                CompositionUtil.CreateContainer(),
                XNodeAnnotationSerializer.Deserialize(
                    XDocument.Load(
                        reader,
                        LoadOptions.PreserveWhitespace | LoadOptions.SetBaseUri)));
        }

        /// <summary>
        /// Loads a <see cref="NXDocumentHost"/> from the given <see cref="TextReader"/>.
        /// </summary>
        /// <param name="reader"></param>
        public static NXDocumentHost Load(TextReader reader)
        {
            Contract.Requires<ArgumentNullException>(reader != null);

            using (var rdr = XmlReader.Create(reader))
                return Load(rdr);
        }

        /// <summary>
        /// Loads a <see cref="NXDocumentHost"/> from the given <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream"></param>
        public static NXDocumentHost Load(Stream stream)
        {
            Contract.Requires<ArgumentNullException>(stream != null);

            using (var rdr = new StreamReader(stream))
                return Load(rdr);
        }

        /// <summary>
        /// Loads a <see cref="NXDocumentHost"/> from the given <see cref="Uri"/>.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static NXDocumentHost Load(Uri uri)
        {
            Contract.Requires<ArgumentNullException>(uri != null);

            return Load(XmlReader.Create(uri.ToString()));
        }

        readonly CompositionContainer container;
        XDocument xml;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="uri"></param>
        /// <param name="xml"></param>
        NXDocumentHost(CompositionContainer container, XDocument xml)
            : base()
        {
            Contract.Requires<ArgumentNullException>(container != null);
            Contract.Requires<ArgumentNullException>(xml != null);

            this.container = container;
            this.xml = xml;

            Initialize();
        }

        /// <summary>
        /// Initializes modules
        /// </summary>
        void Initialize()
        {
            // ensures the document is in the container
            Container.WithExport<NXDocumentHost>(this);

            // ensure XML document has access to document host
            xml.AddAnnotation(this);

            // ensure document has been invoked at least once
            Invoke();
        }

        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> used to resolve services.
        /// </summary>
        public CompositionContainer Container
        {
            get { return container; }
        }

        /// <summary>
        /// Gets a reference to the current <see cref="Xml"/> being handled.
        /// </summary>
        public XDocument Xml
        {
            get { return xml; }
        }

        /// <summary>
        /// Invokes any outstanding actions.
        /// </summary>
        public void Invoke()
        {
            bool run;
            do
            {
                var invokes = Xml.DescendantsAndSelf()
                    .SelectMany(i => i.Interfaces<IOnInvoke>())
                    .ToList();

                run = false;
                foreach (var invoke in invokes)
                    run |= invoke.Invoke();
            }
            while (run);
        }

        /// <summary>
        /// Gets a reference to the root <see cref="XElement"/> instance for navigating the visual tree.
        /// </summary>
        public XElement Root
        {
            get { return xml.Root; }
        }

        /// <summary>
        /// Saves the current state of the <see cref="NXDocumentHost"/> to the specified <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="writer"></param>
        /// <returns></returns>
        public void Save(XmlWriter writer)
        {
            Contract.Requires<ArgumentNullException>(writer != null);

            XNodeAnnotationSerializer.Serialize(xml).Save(writer);
        }

        /// <summary>
        /// Saves the current state of the <see cref="NXDocumentHost"/> to the specified <see cref="TextWriter"/>.
        /// </summary>
        /// <param name="writer"></param>
        public void Save(TextWriter writer)
        {
            Contract.Requires<ArgumentNullException>(writer != null);

            var settings = new XmlWriterSettings()
            {
                Encoding = Encoding.UTF8,
                OmitXmlDeclaration = true,
                NamespaceHandling = NamespaceHandling.OmitDuplicates,
            };

            using (var wrt = XmlWriter.Create(writer, settings))
                Save(wrt);
        }

        /// <summary>
        /// Saves the current state of the <see cref="NXDocumentHost"/> to the specified <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream"></param>
        public void Save(Stream stream)
        {
            Contract.Requires<ArgumentNullException>(stream != null);

            using (var wrt = new StreamWriter(stream, Encoding.UTF8))
                Save(wrt);
        }

    }

}
