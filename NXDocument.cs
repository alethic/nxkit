using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace NXKit
{

    /// <summary>
    /// Hosts an NXKit document. Provides access to the visual tree for a renderer or other processor.
    /// </summary>
    public class NXDocument :
        NXContainer
    {

        /// <summary>
        /// Creates a new default <see cref="NXDocumentConfiguration"/> instance.
        /// </summary>
        /// <returns></returns>
        public static NXDocumentConfiguration CreateDefaultConfiguration()
        {
            return new NXDocumentConfiguration();
        }

        /// <summary>
        /// Loads a <see cref="NXDocument"/> from the given <see cref="Uri"/>.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static NXDocument Load(Uri uri, NXDocumentConfiguration configuration)
        {
            Contract.Requires<ArgumentNullException>(uri != null);
            Contract.Requires<ArgumentNullException>(configuration != null);

            return new NXDocument(
                CompositionUtil.CreateContainer(),
                uri,
                configuration);
        }

        /// <summary>
        /// Loads a <see cref="NXDocument"/> from the given <see cref="Uri"/>.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static NXDocument Load(Uri uri)
        {
            Contract.Requires<ArgumentNullException>(uri != null);

            return Load(uri, CreateDefaultConfiguration());
        }

        /// <summary>
        /// Loads a <see cref="NXDocument"/> from the given <see cref="Stream"/>.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static NXDocument Load(Stream document, NXDocumentConfiguration configuration)
        {
            Contract.Requires<ArgumentNullException>(document != null);
            Contract.Requires<ArgumentNullException>(configuration != null);

            return new NXDocument(
                CompositionUtil.CreateContainer()
                    .WithExport<IResolver>(
                        new SingleUriResolver("document.xml", () => document)),
                new Uri("document.xml", UriKind.Relative),
                configuration);
        }

        /// <summary>
        /// Loads a <see cref="NXDocument"/> from the given <see cref="Stream"/>.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static NXDocument Load(Stream document)
        {
            Contract.Requires<ArgumentNullException>(document != null);

            return Load(document, CreateDefaultConfiguration());
        }

        /// <summary>
        /// Parses a <see cref="NXDocument"/> from the given string.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static NXDocument Parse(string document)
        {
            Contract.Requires<ArgumentNullException>(document != null);

            return Parse(document, CreateDefaultConfiguration());
        }

        /// <summary>
        /// Parses a <see cref="NXDocument"/> from the given string.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static NXDocument Parse(string document, NXDocumentConfiguration configuration)
        {
            Contract.Requires<ArgumentNullException>(document != null);
            Contract.Requires<ArgumentNullException>(configuration != null);

            return Load(new MemoryStream(Encoding.UTF8.GetBytes(document)), configuration);
        }

        readonly LinkedList<object> storage = new LinkedList<object>();
        readonly NXDocumentConfiguration configuration;
        readonly CompositionContainer container;
        readonly Uri uri;
        readonly NodeStateCollection nodeState;
        int nextElementId;

        Module[] modules;

        [ContractInvariantMethod]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
        void ObjectInvariant()
        {
            Contract.Invariant(configuration != null);
            Contract.Invariant(container != null);
            Contract.Invariant(nodeState != null);
            Contract.Invariant(nextElementId >= 0);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="uri"></param>
        public NXDocument(CompositionContainer container, Uri uri)
            : this(container, uri, CreateDefaultConfiguration())
        {
            Contract.Requires<ArgumentNullException>(container != null);
            Contract.Requires<ArgumentNullException>(uri != null);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="uri"></param>
        /// <param name="configuration"></param>
        /// <param name="nextElementId"></param>
        /// <param name="visualState"></param>
        public NXDocument(CompositionContainer container, Uri uri, NXDocumentConfiguration configuration)
            : base()
        {
            Contract.Requires<ArgumentNullException>(container != null);
            Contract.Requires<ArgumentNullException>(uri != null);
            Contract.Requires<ArgumentNullException>(configuration != null);

            this.configuration = configuration;
            this.nextElementId = 1;
            this.nodeState = new NodeStateCollection();

            // resolve uri
            var stream = container.GetExportedValue<IResolver>().Get(uri);
            if (stream == null)
                throw new FileNotFoundException("Could not resolve specified Uri.");

            this.container = container;
            this.uri = uri;
            this.Xml = XDocument.Load(stream);

            Initialize();
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="state"></param>
        public NXDocument(CompositionContainer container, NXDocumentState state)
            : this(container, state.Uri, state.Configuration, state.Xml, state.NextElementId, state.NodeState)
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
        NXDocument(CompositionContainer container, Uri uri, NXDocumentConfiguration configuration, string xml, int nextElementId, NodeStateCollection nodeState)
            : base()
        {
            Contract.Requires<ArgumentNullException>(container != null);
            Contract.Requires<ArgumentNullException>(uri != null);
            Contract.Requires<ArgumentNullException>(configuration != null);
            Contract.Requires<ArgumentNullException>(xml != null);
            Contract.Requires<ArgumentOutOfRangeException>(nextElementId >= 0);
            Contract.Requires<ArgumentNullException>(nodeState != null);

            this.configuration = configuration;
            this.nextElementId = nextElementId;
            this.nodeState = nodeState;

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
            // dictionary of types to instances
            var m = configuration.ModuleTypes
                .ToDictionary(i => i, i => (Module)null);

            do
            {
                // instantiate types
                foreach (var i in m.ToList())
                    if (i.Value == null)
                        m[i.Key] = (Module)Activator.CreateInstance(i.Key);

                // add dependency types
                foreach (var i in m.ToList())
                    foreach (var d in i.Value.DependsOn)
                        if (!m.ContainsKey(d))
                            m[d] = null;
            }
            // end when all types are instantiated
            while (m.Any(i => i.Value == null));

            // generate final module list
            modules = m.Values.ToArray();

            // initialize modules
            foreach (var module in modules)
                module.Initialize(this);

            // create the root node and add it to the document
            Add(CreateRootNode());

            // ensure document has been invoked at least once
            Invoke();
        }

        /// <summary>
        /// Gets the current engine configuration.
        /// </summary>
        public NXDocumentConfiguration Configuration
        {
            get { return configuration; }
        }

        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> used to resolve services.
        /// </summary>
        public CompositionContainer Container
        {
            get { return container; }
        }

        public override NXDocument Document
        {
            get { return this; }
        }

        /// <summary>
        /// Gets a reference to the current <see cref="Xml"/> being handled.
        /// </summary>
        public new XDocument Xml
        {
            get { return (XDocument)base.Xml; }
            internal set { base.Xml = value; }
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
        /// Gets a reference to the per-<see cref="NXNode"/> state collection.
        /// </summary>
        public NodeStateCollection NodeState
        {
            get { return nodeState; }
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
            var idAttr = element.Attribute("id");
            if (idAttr == null)
            {
                element.SetAttributeValue("id", "_element" + ++nextElementId);
                idAttr = element.Attribute("id");
            }

            return idAttr.Value;
        }

        /// <summary>
        /// Gets a reference to the root <see cref="NXELement"/> instance for navigating the visual tree.
        /// </summary>
        public NXElement Root
        {
            get { return Elements().FirstOrDefault(); }
        }

        /// <summary>
        /// Creates a new root <see cref="NXNode"/> instance for navigating the visual tree.
        /// </summary>
        /// <returns></returns>
        NXElement CreateRootNode()
        {
            Contract.Ensures(Contract.Result<NXElement>() != null);

            return (NXElement)CreateNodeFromModules(Xml.Root) ?? new UnknownRootElement(Xml.Root);
        }

        /// <summary>
        /// Creates a <see cref="NXNode"/> from the loaded <see cref="Module"/>s.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="xml"></param>
        /// <returns></returns>
        NXNode CreateNodeFromModules(XElement xml)
        {
            Contract.Requires<ArgumentNullException>(xml != null);

            return modules
                .Select(i => i.CreateNode(xml))
                .FirstOrDefault(i => i != null);
        }

        /// <summary>
        /// Implements IVisualBuilder.CreateVisual.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="xml"></param>
        /// <returns></returns>
        internal NXNode CreateNode(XNode xml)
        {
            if (xml is XText)
            {
                return new NXText((XText)xml);
            }
            else if (xml is XElement)
            {
                // create new instance of visual using extensions
                var node = CreateNodeFromModules((XElement)xml);
                if (node != null)
                    return node;
            }

            return null;
        }

        /// <summary>
        /// Saves the current state of the processor in a serializable format.
        /// </summary>
        /// <returns></returns>
        public NXDocumentState Save()
        {
            return new NXDocumentState(
                configuration,
                uri,
                Xml.ToString(SaveOptions.DisableFormatting),
                nextElementId,
                nodeState);
        }

        /// <summary>
        /// Invoke to begin a form submission.
        /// </summary>
        public void Submit()
        {
            if (ProcessSubmit != null)
                ProcessSubmit(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raised to initiate submission of the form.
        /// </summary>
        public event EventHandler ProcessSubmit;

    }

}
