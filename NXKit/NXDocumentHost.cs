using System;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
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
        /// Loads a <see cref="NXDocumentHost"/> from the given <see cref="Uri"/>.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static NXDocumentHost Load(Uri uri)
        {
            Contract.Requires<ArgumentNullException>(uri != null);

            return new NXDocumentHost(
                CompositionUtil.CreateContainer(),
                uri);
        }

        readonly CompositionContainer container;
        readonly Uri uri;
        XDocument xml;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="uri"></param>
        public NXDocumentHost(CompositionContainer container, Uri uri)
            : base()
        {
            Contract.Requires<ArgumentNullException>(container != null);
            Contract.Requires<ArgumentNullException>(uri != null);

            this.container = container;
            this.uri = new Uri(uri.ToString());
            this.Xml = XDocument.Load(uri.ToString(), LoadOptions.SetBaseUri);

            Initialize();
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="state"></param>
        public NXDocumentHost(CompositionContainer container, NXDocumentState state)
            : this(container, state.Uri, state.Xml)
        {
            Contract.Requires<ArgumentNullException>(container != null);
            Contract.Requires<ArgumentNullException>(state != null);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="uri"></param>
        /// <param name="configuration"></param>
        /// <param name="xml"></param>
        /// <param name="nextElementId"></param>
        /// <param name="nodeState"></param>
        NXDocumentHost(CompositionContainer container, Uri uri, string xml)
            : base()
        {
            Contract.Requires<ArgumentNullException>(container != null);
            Contract.Requires<ArgumentNullException>(uri != null);
            Contract.Requires<ArgumentNullException>(xml != null);

            this.container = container;
            this.uri = uri;
            this.xml = XDocument.Parse(xml);

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
            internal set { xml = value; }
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
            using (var i = xml.CreateAnnotationReader())
                writer.WriteNode(i, true);
        }

        /// <summary>
        /// Saves the current state of the <see cref="NXDocumentHost"/> to the specified <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream"></param>
        public void Save(Stream stream)
        {
            using (var wrt = XmlWriter.Create(stream, new XmlWriterSettings() { Encoding = Encoding.UTF8, OmitXmlDeclaration = true }))
                Save(wrt);
        }

    }

}
