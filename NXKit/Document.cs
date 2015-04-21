using System;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

using NXKit.Composition;
using NXKit.Diagnostics;
using NXKit.IO;
using NXKit.Serialization;
using NXKit.Util;
using NXKit.Xml;

namespace NXKit
{

    /// <summary>
    /// Hosts an NXKit document. Provides access to the visual tree for a renderer or other processor.
    /// </summary>
    public class Document :
        IDisposable
    {

        /// <summary>
        /// Initializes the static instance.
        /// </summary>
        static Document()
        {
            if (!FrameworkUtil.IsCompatibleWithFramework())
                throw new Exception("NXKit is only compatible with Frameworks targeting .NET 4.5 or above.");
        }

        /// <summary>
        /// Loads a <see cref="Document"/> from the given <see cref="XmlReader"/>.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="catalog"></param>
        /// <param name="exports"></param>
        /// <returns></returns>
        public static Document Load(XmlReader reader, ComposablePartCatalog catalog = null, ExportProvider exports = null)
        {
            Contract.Requires<ArgumentNullException>(reader != null);

            return new Document(host =>
                host.Container.GetExportedValue<AnnotationSerializer>().Deserialize(
                    XDocument.Load(
                        reader,
                        LoadOptions.PreserveWhitespace | LoadOptions.SetBaseUri)),
                catalog,
                exports);
        }

        /// <summary>
        /// Loads a <see cref="Document"/> from the given <see cref="XmlReader"/>.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static Document Load(XmlReader reader)
        {
            Contract.Requires<ArgumentNullException>(reader != null);

            return Load(reader, null, null);
        }

        /// <summary>
        /// Loads a <see cref="Document"/> from the given <see cref="TextReader"/>.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="catalog"></param>
        /// <param name="exports"></param>
        public static Document Load(TextReader reader, ComposablePartCatalog catalog = null, ExportProvider exports = null)
        {
            Contract.Requires<ArgumentNullException>(reader != null);

            using (var rdr = XmlReader.Create(reader))
                return Load(rdr, catalog, exports);
        }

        /// <summary>
        /// Loads a <see cref="Document"/> from the given <see cref="TextReader"/>.
        /// </summary>
        /// <param name="reader"></param>
        public static Document Load(TextReader reader)
        {
            Contract.Requires<ArgumentNullException>(reader != null);

            return Load(reader, null, null);
        }

        /// <summary>
        /// Loads a <see cref="Document"/> from the given <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="catalog"></param>
        /// <param name="exports"></param>
        /// <returns></returns>
        public static Document Load(Stream stream, ComposablePartCatalog catalog = null, ExportProvider exports = null)
        {
            Contract.Requires<ArgumentNullException>(stream != null);

            using (var rdr = new StreamReader(stream))
                return Load(rdr, catalog, exports);
        }

        /// <summary>
        /// Loads a <see cref="Document"/> from the given <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream"></param>
        public static Document Load(Stream stream)
        {
            Contract.Requires<ArgumentNullException>(stream != null);

            return Load(stream, null, null);
        }

        /// <summary>
        /// Loads a <see cref="Document"/> from the given <see cref="Uri"/>.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="catalog"></param>
        /// <param name="exports"></param>
        /// <returns></returns>
        public static Document Load(Uri uri, ComposablePartCatalog catalog = null, ExportProvider exports = null)
        {
            Contract.Requires<ArgumentNullException>(uri != null);

            return new Document(host =>
                host.Container.GetExportedValue<AnnotationSerializer>().Deserialize(
                    XDocument.Load(
                        NXKit.Xml.IOXmlReader.Create(
                            host.Container.GetExportedValue<IIOService>(),
                            uri),
                        LoadOptions.PreserveWhitespace | LoadOptions.SetBaseUri)),
                catalog,
                exports);
        }

        /// <summary>
        /// Loads a <see cref="Document"/> from the given <see cref="Uri"/>.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static Document Load(Uri uri)
        {
            Contract.Requires<ArgumentNullException>(uri != null);

            return Load(uri, null, null);
        }

        /// <summary>
        /// Loads a <see cref="Document"/> from the given <see cref="XDocument"/>.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="catalog"></param>
        /// <param name="exports"></param>
        /// <returns></returns>
        public static Document Load(XDocument document, ComposablePartCatalog catalog = null, ExportProvider exports = null)
        {
            Contract.Requires<ArgumentNullException>(document != null);

            return Load(document.CreateReader(), catalog, exports);
        }

        /// <summary>
        /// Loads a <see cref="Document"/> from the given <see cref="XDocument"/>.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static Document Load(XDocument document)
        {
            Contract.Requires<ArgumentNullException>(document != null);

            return Load(document, null, null);
        }

        /// <summary>
        /// Loads a <see cref="Document"/> by parsing the given input XML.
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static Document Parse(string xml)
        {
            Contract.Requires<ArgumentNullException>(xml != null);
            Contract.Requires<ArgumentOutOfRangeException>(!string.IsNullOrWhiteSpace(xml));

            return Load(XDocument.Parse(xml));
        }

        /// <summary>
        /// Gets the <see cref="CompositionConfiguration"/> for the given input.
        /// </summary>
        /// <param name="catalog"></param>
        /// <param name="exports"></param>
        /// <returns></returns>
        static CompositionConfiguration GetConfiguration(ComposablePartCatalog catalog, ExportProvider exports)
        {
            // default configuration
            if (catalog == null &&
                exports == null)
                return CompositionConfiguration.Default;

            // default catalogs
            if (catalog == null)
                return new CompositionConfiguration(
                    CompositionUtil.DefaultGlobalCatalog,
                    CompositionUtil.DefaultHostCatalog,
                    CompositionUtil.DefaultObjectCatalog,
                    exports ?? new CompositionContainer());

            // otherwise, generate new
            return new CompositionConfiguration(
                catalog,
                exports ?? new CompositionContainer());
        }

        readonly CompositionConfiguration configuration;
        readonly CompositionContainer container;
        readonly IInvoker invoker;
        readonly ITraceService trace;
        readonly XDocument xml;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="catalog"></param>
        /// <param name="exports"></param>
        Document(
            Func<Document, XDocument> xml,
            ComposablePartCatalog catalog,
            ExportProvider exports)
        {
            Contract.Requires<ArgumentNullException>(xml != null);

            // configure composition
            this.configuration = GetConfiguration(catalog, exports);
            this.container = new CompositionContainer(configuration.HostCatalog, true, new CompositionContainer(configuration.GlobalCatalog, true, configuration.Exports));
            this.container.GetExportedValue<DocumentEnvironment>().SetHost(this);

            // required services
            this.invoker = container.GetExportedValue<IInvoker>();
            this.trace = container.GetExportedValue<ITraceService>();

            // initialize xml
            this.xml = xml(this);
            this.xml.AddAnnotation(this);

            // parallel initialization of common interfaces
            Parallel.ForEach(this.xml.DescendantNodesAndSelf(), i =>
            {
                Enumerable.Empty<object>()
                    .Concat(i.Interfaces<IOnInit>())
                    .Concat(i.Interfaces<IOnLoad>())
                    .ToLinkedList();
            });

            // initial invocation entry
            this.invoker.Invoke(() => { });
        }

        /// <summary>
        /// Gets the <see cref="CompositionConfiguration"/> for the host.
        /// </summary>
        public CompositionConfiguration Configuration
        {
            get { return configuration; }
        }

        /// <summary>
        /// Gets the host configured <see cref="CompositionContainer"/>.
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
        /// Gets a reference to the root <see cref="XElement"/> instance for navigating the visual tree.
        /// </summary>
        public XElement Root
        {
            get { return xml.Root; }
        }

        /// <summary>
        /// Saves the current state of the <see cref="Document"/> to the specified <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="writer"></param>
        /// <returns></returns>
        public void Save(XmlWriter writer)
        {
            Contract.Requires<ArgumentNullException>(writer != null);

            // instruct any interfaces to save their state
            var saves = Xml.DescendantsAndSelf()
                .SelectMany(i => i.Interfaces<IOnSave>())
                .ToLinkedList();
            foreach (var save in saves)
                save.Save();

            // serialize document to writer
            container.GetExportedValue<AnnotationSerializer>().Serialize(xml).Save(writer);
        }

        /// <summary>
        /// Saves the current state of the <see cref="Document"/> to the specified <see cref="TextWriter"/>.
        /// </summary>
        /// <param name="writer"></param>
        public void Save(TextWriter writer)
        {
            Contract.Requires<ArgumentNullException>(writer != null);

            var settings = new XmlWriterSettings()
            {
                Encoding = Encoding.UTF8,
                OmitXmlDeclaration = true,
                NamespaceHandling = NamespaceHandling.Default,
            };

            using (var wrt = XmlWriter.Create(writer, settings))
                Save(wrt);
        }

        /// <summary>
        /// Saves the current state of the <see cref="Document"/> to the specified <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream"></param>
        public void Save(Stream stream)
        {
            Contract.Requires<ArgumentNullException>(stream != null);

            using (var wrt = new StreamWriter(stream, Encoding.UTF8))
                Save(wrt);
        }

        /// <summary>
        /// Disposes of the <see cref="Document"/>.
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);

            if (xml != null)
            {
                // dispose of any annotations that support it
                var disposable = xml
                    .DescendantNodesAndSelf()
                    .SelectMany(i => i.Annotations<IDisposable>());

                foreach (var dispose in disposable)
                    if (dispose != this)
                        dispose.Dispose();
            }

            // dispose of host container
            if (container != null)
                container.Dispose();
        }

        /// <summary>
        /// Finalizes the instance.
        /// </summary>
        ~Document()
        {
            Dispose();
        }

    }

}
