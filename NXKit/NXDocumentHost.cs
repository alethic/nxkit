﻿using System;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
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
    public class NXDocumentHost :
        IDisposable
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

            return new NXDocumentHost(host =>
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

            return new NXDocumentHost(host =>
                XNodeAnnotationSerializer.Deserialize(
                    XDocument.Load(
                        NXKit.Xml.IOXmlReader.Create(
                            host.Container.GetExportedValue<IIOService>(),
                            uri),
                        LoadOptions.PreserveWhitespace | LoadOptions.SetBaseUri)),
                catalog,
                exports);
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
        NXDocumentHost(
            Func<NXDocumentHost, XDocument> xml,
            ComposablePartCatalog catalog,
            ExportProvider exports)
        {
            Contract.Requires<ArgumentNullException>(xml != null);

            // configure composition
            this.configuration = GetConfiguration(catalog, exports);
            this.container = new CompositionContainer(configuration.HostCatalog, new CompositionContainer(configuration.GlobalCatalog, configuration.Exports));
            this.container.GetExportedValue<DocumentEnvironment>().SetHost(this);

            // required services
            this.invoker = container.GetExportedValue<IInvoker>();
            this.trace = container.GetExportedValue<ITraceService>();

            // initialize xml
            this.xml = xml(this);
            this.xml.AddAnnotation(this);

            Initialize();
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
        /// Initializes modules
        /// </summary>
        void Initialize()
        {
            xml.Changed += xml_Changed;

            // start up document
            InvokeInit();
            InvokeLoad();
            Invoke();
        }

        /// <summary>
        /// Invoked when any nodes are changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void xml_Changed(object sender, XObjectChangeEventArgs args)
        {
            if (args.ObjectChange == XObjectChange.Add)
            {
                InvokeInit();
                InvokeLoad();
            }
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
                    .Where(i => i.GetObjectId() > 0)
                    .Where(i => i.InterfaceOrDefault<IOnInit>() != null)
                    .Where(i => i.AnnotationOrCreate<ObjectAnnotation>().Init == true)
                    .ToLinkedList();

                if (inits.Count == 0)
                    break;

                foreach (var init in inits)
                    if (init.Document != null)
                    {
                        invoker.Invoke(() => init.Interface<IOnInit>().Init());
                        init.AnnotationOrCreate<ObjectAnnotation>().Init = false;
                    }
            }
        }

        /// <summary>
        /// Invokes any <see cref="IOnLoad"/> interfaces.
        /// </summary>
        void InvokeLoad()
        {
            var loads = xml
                .DescendantNodesAndSelf()
                .Where(i => i.GetObjectId() > 0)
                .Where(i => i.InterfaceOrDefault<IOnLoad>() != null)
                .Where(i => i.AnnotationOrCreate<ObjectAnnotation>().Load == true)
                .ToLinkedList();

            foreach (var load in loads)
                if (load.Document != null)
                {
                    invoker.Invoke(() => load.Interface<IOnLoad>().Load());
                    load.AnnotationOrCreate<ObjectAnnotation>().Load = false;
                }
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
                    .ToLinkedList();

                run = false;
                foreach (var invoke in invokes)
                    run |= invoker.Invoke(() => invoke.Invoke());
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

            // instruct any interfaces to save their state
            var saves = Xml.DescendantsAndSelf()
                .SelectMany(i => i.Interfaces<IOnSave>())
                .ToLinkedList();
            foreach (var save in saves)
                save.Save();

            // serialize document to writer
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

        /// <summary>
        /// Disposes of the <see cref="NXDocumentHost"/>.
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
        ~NXDocumentHost()
        {
            Dispose();
        }

    }

}
