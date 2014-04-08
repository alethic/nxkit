using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
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
        int nextElementId;
        XDocument xml;

        Module[] modules;

        [ContractInvariantMethod]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
        void ObjectInvariant()
        {
            Contract.Invariant(container != null);
            Contract.Invariant(nextElementId >= 0);
        }

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

            this.nextElementId = 1;

            //// resolve uri
            //var request = WebRequest.Create(uri);
            //if (request == null)
            //    throw new FileNotFoundException("No request handler.");

            //var response = request.GetResponse();
            //if (response == null)
            //    throw new FileNotFoundException("No response.");

            //var stream = response.GetResponseStream();
            //if (stream == null)
            //    throw new FileNotFoundException("No response stream.");

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
            : this(container, state.Uri, state.Xml, state.NextElementId)
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
        NXDocumentHost(CompositionContainer container, Uri uri, string xml, int nextElementId)
            : base()
        {
            Contract.Requires<ArgumentNullException>(container != null);
            Contract.Requires<ArgumentNullException>(uri != null);
            Contract.Requires<ArgumentNullException>(xml != null);
            Contract.Requires<ArgumentOutOfRangeException>(nextElementId >= 0);

            this.nextElementId = nextElementId;

            this.container = container;
            this.uri = uri;
            this.Xml = XDocument.Parse(xml);

            Initialize();
        }

        /// <summary>
        /// Initializes modules
        /// </summary>
        void Initialize()
        {
            // ensures the document is in the container
            Container.WithExport<NXDocumentHost>(this);

            // generate final module list
            modules = Container.GetExportedValues<Module>()
                .ToArray();

            // initialize modules
            foreach (var module in modules)
                module.Initialize();

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
        /// Gets all the loaded modules.
        /// </summary>
        public IEnumerable<Module> Modules()
        {
            return modules;
        }

        /// <summary>
        /// Gets the loaded module instance of the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Module<T>()
            where T : Module
        {
            return modules.OfType<T>().FirstOrDefault();
        }

        /// <summary>
        /// Invokes any outstanding actions.
        /// </summary>
        public void Invoke()
        {
            // run each module until no module does anything
            bool run;
            do
            {
                run = false;
                foreach (var module in modules)
                    run |= module.Invoke();
            }
            while (run);
        }

        /// <summary>
        /// Gets the 'id' attribute for the given Element, or creates it on demand.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public string GetElementId(XElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            var idAttr = (string)element.Attribute("id");
            if (idAttr == null)
                idAttr = "_element" + ++nextElementId;

            element.SetAttributeValue("id", idAttr);

            return idAttr;
        }

        /// <summary>
        /// Gets a reference to the root <see cref="XElement"/> instance for navigating the visual tree.
        /// </summary>
        public XElement Root
        {
            get { return xml.Root; }
        }

        /// <summary>
        /// Saves the current state of the processor in a serializable format.
        /// </summary>
        /// <returns></returns>
        public NXDocumentState Save()
        {
            return new NXDocumentState(
                uri,
                Xml.ToString(SaveOptions.DisableFormatting),
                nextElementId);
        }

    }

}
