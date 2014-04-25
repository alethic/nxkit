using System;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Xml;
using System.Xml.Linq;

using NXKit.Composition;
using NXKit.Diagnostics;
using NXKit.IO;
using NXKit.Serialization;
using NXKit.Xml;

namespace NXKit
{

    /// <summary>
    /// Hosts an NXKit document. Provides access to the visual tree for a renderer or other processor.
    /// </summary>
    public class NXDocumentHost
    {

        /// <summary>
        /// Loads a <see cref="NXDocumentHost"/> from the given <see cref="XmlReader"/>.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="catalog"></param>
        /// <param name="exports"></param>
        /// <returns></returns>
        public static NXDocumentHost Load(XmlReader reader, ComposablePartCatalog catalog = null, ExportProvider exports = null)
        {
            Contract.Requires<ArgumentNullException>(reader != null);

            catalog = catalog ?? CompositionUtil.DefaultCatalog;
            exports = exports ?? CompositionUtil.CreateContainer(catalog);

            return new NXDocumentHost(
                XNodeAnnotationSerializer.Deserialize(
                    XDocument.Load(
                        reader,
                        LoadOptions.PreserveWhitespace | LoadOptions.SetBaseUri)),
                catalog,
                exports);
        }

        /// <summary>
        /// Loads a <see cref="NXDocumentHost"/> from the given <see cref="XmlReader"/>.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static NXDocumentHost Load(XmlReader reader)
        {
            Contract.Requires<ArgumentNullException>(reader != null);

            return Load(reader, null, null);
        }

        /// <summary>
        /// Loads a <see cref="NXDocumentHost"/> from the given <see cref="TextReader"/>.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="catalog"></param>
        /// <param name="exports"></param>
        public static NXDocumentHost Load(TextReader reader, ComposablePartCatalog catalog = null, ExportProvider exports = null)
        {
            Contract.Requires<ArgumentNullException>(reader != null);

            catalog = catalog ?? CompositionUtil.DefaultCatalog;
            exports = exports ?? CompositionUtil.CreateContainer(catalog);

            using (var rdr = XmlReader.Create(reader))
                return Load(rdr, catalog, exports);
        }

        /// <summary>
        /// Loads a <see cref="NXDocumentHost"/> from the given <see cref="TextReader"/>.
        /// </summary>
        /// <param name="reader"></param>
        public static NXDocumentHost Load(TextReader reader)
        {
            Contract.Requires<ArgumentNullException>(reader != null);

            return Load(reader, null, null);
        }

        /// <summary>
        /// Loads a <see cref="NXDocumentHost"/> from the given <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="catalog"></param>
        /// <param name="exports"></param>
        /// <returns></returns>
        public static NXDocumentHost Load(Stream stream, ComposablePartCatalog catalog = null, ExportProvider exports = null)
        {
            Contract.Requires<ArgumentNullException>(stream != null);

            catalog = catalog ?? CompositionUtil.DefaultCatalog;
            exports = exports ?? CompositionUtil.CreateContainer(catalog);

            using (var rdr = new StreamReader(stream))
                return Load(rdr, catalog, exports);
        }

        /// <summary>
        /// Loads a <see cref="NXDocumentHost"/> from the given <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream"></param>
        public static NXDocumentHost Load(Stream stream)
        {
            Contract.Requires<ArgumentNullException>(stream != null);

            return Load(stream, null, null);
        }

        /// <summary>
        /// Loads a <see cref="NXDocumentHost"/> from the given <see cref="Uri"/>.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="catalog"></param>
        /// <param name="exports"></param>
        /// <returns></returns>
        public static NXDocumentHost Load(Uri uri, ComposablePartCatalog catalog = null, ExportProvider exports = null)
        {
            Contract.Requires<ArgumentNullException>(uri != null);

            catalog = catalog ?? CompositionUtil.DefaultCatalog;
            exports = exports ?? CompositionUtil.CreateContainer(catalog);

            return Load(
                NXKit.Xml.IOXmlReader.Create(
                    exports.GetExportedValue<IIOService>(),
                    uri));
        }

        /// <summary>
        /// Loads a <see cref="NXDocumentHost"/> from the given <see cref="Uri"/>.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static NXDocumentHost Load(Uri uri)
        {
            Contract.Requires<ArgumentNullException>(uri != null);

            return Load(uri, null, null);
        }

        /// <summary>
        /// Loads a <see cref="NXDocumentHost"/> from the given <see cref="XDocument"/>.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="catalog"></param>
        /// <param name="exports"></param>
        /// <returns></returns>
        public static NXDocumentHost Load(XDocument document, ComposablePartCatalog catalog = null, ExportProvider exports = null)
        {
            Contract.Requires<ArgumentNullException>(document != null);

            catalog = catalog ?? CompositionUtil.DefaultCatalog;
            exports = exports ?? CompositionUtil.CreateContainer(catalog);

            return Load(document.CreateReader(), catalog, exports);
        }

        /// <summary>
        /// Loads a <see cref="NXDocumentHost"/> from the given <see cref="XDocument"/>.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static NXDocumentHost Load(XDocument document)
        {
            Contract.Requires<ArgumentNullException>(document != null);

            return Load(document, null, null);
        }

        readonly ComposablePartCatalog catalog;
        readonly CompositionContainer global;
        readonly CompositionContainer host;
        readonly ITraceService trace;
        XDocument xml;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="catalog"></param>
        /// <param name="exports"></param>
        NXDocumentHost(XDocument xml, ComposablePartCatalog catalog = null, ExportProvider exports = null)
            : base()
        {
            Contract.Requires<ArgumentNullException>(xml != null);

            catalog = catalog ?? CompositionUtil.DefaultCatalog;
            exports = exports ?? CompositionUtil.CreateContainer(catalog);

            this.catalog = catalog;

            // global container, contains all exports that are global in nature
            this.global = new CompositionContainer(
                new ScopeExportProvider(exports, Scope.Global));

            // host container, contains all exports that are host scoped, and catalog of host scoped parts
            this.host = new CompositionContainer(
                new ScopeCatalog(this.catalog, Scope.Host), 
                this.global);

            // ensures the document is in the container
            this.host.WithExport<NXDocumentHost>(this);
            this.host.WithExport<ExportProvider>(host);

            // initialize document configuration
            this.trace = host.GetExportedValue<ITraceService>();
            this.xml = xml;

            // ensure XML document has access to document host
            this.xml.AddAnnotation(this);
            this.xml.AddAnnotation(host);

            Initialize();
        }

        /// <summary>
        /// Initializes modules
        /// </summary>
        void Initialize()
        {
            // start up document
            InvokeInit();
            InvokeLoad();
            Invoke();
        }

        /// <summary>
        /// Handles an exception by dispatching it to the root <see cref="IExceptionHandler"/>.
        /// </summary>
        /// <param name="exception"></param>
        void HandleException(Exception exception)
        {
            Contract.Requires<ArgumentNullException>(exception != null);
            trace.Warning(exception);

            bool rethrow = true;

            // search for exception handlers
            foreach (var handler in Xml.Interfaces<IExceptionHandler>())
                rethrow |= !handler.HandleException(exception);

            // should we rethrow the exception?
            if (rethrow)
                ExceptionDispatchInfo.Capture(exception).Throw();
        }

        /// <summary>
        /// Invokes the given <see cref="Action"/>, protecting the caller against exceptions.
        /// </summary>
        /// <param name="action"></param>
        void Invoke(Action action)
        {
            Contract.Requires<ArgumentNullException>(action != null);

            try
            {
                action();
            }
            catch (Exception e)
            {
                HandleException(e);
            }
        }

        /// <summary>
        /// Invokes the given <see cref="Func`1"/>, protecting the caller against exception.s
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        T Invoke<T>(Func<T> func)
        {
            Contract.Requires<ArgumentNullException>(func != null);

            try
            {
                return func();
            }
            catch (Exception e)
            {
                HandleException(e);
            }

            return default(T);
        }

        /// <summary>
        /// Invokes any <see cref="IOnInit"/> interfaces the first time the document is loaded.
        /// </summary>
        void InvokeInit()
        {
            while (true)
            {
                var inits = xml
                    .DescendantNodesAndSelf()
                    .Where(i => i.InterfaceOrDefault<IOnInit>() != null)
                    .Where(i => i.AnnotationOrCreate<ObjectAnnotation>().Init == false)
                    .ToList();

                if (inits.Count == 0)
                    break;

                foreach (var init in inits)
                    if (init.Document != null)
                        Invoke(() =>
                        {
                            init.Interface<IOnInit>().Init();
                            init.AnnotationOrCreate<ObjectAnnotation>().Init = true;
                        });
            }
        }

        /// <summary>
        /// Invokes any <see cref="IOnLoad"/> interfaces.
        /// </summary>
        void InvokeLoad()
        {
            var loads = xml
                .DescendantNodesAndSelf()
                .Where(i => i.InterfaceOrDefault<IOnLoad>() != null)
                .ToList();

            foreach (var load in loads)
                if (load.Document != null)
                    Invoke(() =>
                    {
                        load.Interface<IOnLoad>().Load();
                    });
        }

        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> used to resolve services.
        /// </summary>
        public CompositionContainer Container
        {
            get { return host; }
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
                    run |= Invoke(() => invoke.Invoke());
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
                NamespaceHandling = NamespaceHandling.Default,
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
