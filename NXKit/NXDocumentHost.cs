using System;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Xml;
using System.Xml.Linq;

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
        /// <param name="container"></param>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static NXDocumentHost Load(CompositionContainer container, XmlReader reader)
        {
            Contract.Requires<ArgumentNullException>(container != null);
            Contract.Requires<ArgumentNullException>(reader != null);

            return new NXDocumentHost(
                container,
                XNodeAnnotationSerializer.Deserialize(
                    XDocument.Load(
                        reader,
                        LoadOptions.PreserveWhitespace | LoadOptions.SetBaseUri)));
        }

        /// <summary>
        /// Loads a <see cref="NXDocumentHost"/> from the given <see cref="XmlReader"/>.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static NXDocumentHost Load(XmlReader reader)
        {
            Contract.Requires<ArgumentNullException>(reader != null);

            return Load(CompositionUtil.CreateContainer(), reader);
        }

        /// <summary>
        /// Loads a <see cref="NXDocumentHost"/> from the given <see cref="TextReader"/>.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="reader"></param>
        public static NXDocumentHost Load(CompositionContainer container, TextReader reader)
        {
            Contract.Requires<ArgumentNullException>(container != null);
            Contract.Requires<ArgumentNullException>(reader != null);

            using (var rdr = XmlReader.Create(reader))
                return Load(container, rdr);
        }

        /// <summary>
        /// Loads a <see cref="NXDocumentHost"/> from the given <see cref="TextReader"/>.
        /// </summary>
        /// <param name="reader"></param>
        public static NXDocumentHost Load(TextReader reader)
        {
            Contract.Requires<ArgumentNullException>(reader != null);

            return Load(CompositionUtil.CreateContainer(), reader);
        }

        /// <summary>
        /// Loads a <see cref="NXDocumentHost"/> from the given <see cref="Stream"/>.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static NXDocumentHost Load(CompositionContainer container, Stream stream)
        {
            Contract.Requires<ArgumentNullException>(container != null);
            Contract.Requires<ArgumentNullException>(stream != null);

            using (var rdr = new StreamReader(stream))
                return Load(container, rdr);
        }

        /// <summary>
        /// Loads a <see cref="NXDocumentHost"/> from the given <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream"></param>
        public static NXDocumentHost Load(Stream stream)
        {
            Contract.Requires<ArgumentNullException>(stream != null);

            return Load(CompositionUtil.CreateContainer(), stream);
        }

        /// <summary>
        /// Loads a <see cref="NXDocumentHost"/> from the given <see cref="Uri"/>.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static NXDocumentHost Load(CompositionContainer container, Uri uri)
        {
            Contract.Requires<ArgumentNullException>(container != null);
            Contract.Requires<ArgumentNullException>(uri != null);

            return Load(
                container,
                NXKit.Xml.IOXmlReader.Create(
                    container.GetExportedValue<IIOService>(),
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

            return Load(CompositionUtil.CreateContainer(), uri);
        }

        /// <summary>
        /// Loads a <see cref="NXDocumentHost"/> from the given <see cref="XDocument"/>.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="document"></param>
        /// <returns></returns>
        public static NXDocumentHost Load(CompositionContainer container, XDocument document)
        {
            Contract.Requires<ArgumentNullException>(container != null);
            Contract.Requires<ArgumentNullException>(document != null);

            return Load(container, document.CreateReader());
        }

        /// <summary>
        /// Loads a <see cref="NXDocumentHost"/> from the given <see cref="XDocument"/>.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static NXDocumentHost Load(XDocument document)
        {
            Contract.Requires<ArgumentNullException>(document != null);

            return Load(CompositionUtil.CreateContainer(), document);
        }

        readonly CompositionContainer container;
        readonly ITraceService trace;
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
            this.trace = container.GetExportedValue<ITraceService>();
            this.xml = xml;

            Initialize();
        }

        /// <summary>
        /// Initializes modules
        /// </summary>
        void Initialize()
        {
            // ensures the document is in the container
            container.WithExport<NXDocumentHost>(this);
            container.WithExport<ExportProvider>(container);

            // ensure XML document has access to document host
            xml.AddAnnotation(this);

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
            trace.Debug("InvokeInit");

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
                            trace.Debug("InvokeInit: {0}", init);
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
            trace.Debug("InvokeLoad");

            var loads = xml
                .DescendantNodesAndSelf()
                .Where(i => i.InterfaceOrDefault<IOnLoad>() != null)
                .ToList();

            foreach (var load in loads)
                if (load.Document != null)
                    Invoke(() =>
                    {
                        trace.Debug("InvokeLoad: {0}", load);
                        load.Interface<IOnLoad>().Load();
                    });
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
