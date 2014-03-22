using System;
using System.Collections.Generic;
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
        INXDocument
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

            return new NXDocument(new DefaultResolver(), uri, configuration);
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
                new SingleUriResolver("document.xml", () => document),
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

        readonly NXDocumentConfiguration configuration;
        readonly IResolver resolver;
        readonly Uri uri;
        readonly XDocument xml;
        readonly VisualStateCollection visualState;
        int nextElementId;

        ContentVisual rootVisual;
        Module[] modules;

        [ContractInvariantMethod]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
        void ObjectInvariant()
        {
            Contract.Invariant(configuration != null);
            Contract.Invariant(xml != null);
            Contract.Invariant(resolver != null);
            Contract.Invariant(visualState != null);
            Contract.Invariant(nextElementId >= 0);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        /// <param name="uri"></param>
        public NXDocument(IResolver resolver, Uri uri)
            : this(resolver, uri, CreateDefaultConfiguration())
        {
            Contract.Requires<ArgumentNullException>(resolver != null);
            Contract.Requires<ArgumentNullException>(uri != null);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        /// <param name="uri"></param>
        /// <param name="configuration"></param>
        /// <param name="nextElementId"></param>
        /// <param name="visualState"></param>
        public NXDocument(IResolver resolver, Uri uri, NXDocumentConfiguration configuration)
        {
            Contract.Requires<ArgumentNullException>(resolver != null);
            Contract.Requires<ArgumentNullException>(uri != null);
            Contract.Requires<ArgumentNullException>(configuration != null);

            this.configuration = configuration;
            this.nextElementId = 1;
            this.visualState = new VisualStateCollection();

            // resolve uri
            var stream = resolver.Get(uri);
            if (stream == null)
                throw new FileNotFoundException("Could not resolve specified Uri.");

            this.resolver = resolver;
            this.uri = uri;
            this.xml = XDocument.Load(stream);

            Initialize();
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        /// <param name="state"></param>
        public NXDocument(IResolver resolver, NXDocumentState state)
            : this(resolver, state.Uri, state.Configuration, state.Xml, state.NextElementId, state.VisualState)
        {
            Contract.Requires<ArgumentNullException>(resolver != null);
            Contract.Requires<ArgumentNullException>(state != null);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        /// <param name="uri"></param>
        /// <param name="configuration"></param>
        /// <param name="xml"></param>
        /// <param name="nextElementId"></param>
        /// <param name="visualState"></param>
        NXDocument(IResolver resolver, Uri uri, NXDocumentConfiguration configuration, string xml, int nextElementId, VisualStateCollection visualState)
        {
            Contract.Requires<ArgumentNullException>(resolver != null);
            Contract.Requires<ArgumentNullException>(uri != null);
            Contract.Requires<ArgumentNullException>(configuration != null);
            Contract.Requires<ArgumentNullException>(xml != null);
            Contract.Requires<ArgumentOutOfRangeException>(nextElementId >= 0);
            Contract.Requires<ArgumentNullException>(visualState != null);

            this.configuration = configuration;
            this.nextElementId = nextElementId;
            this.visualState = visualState;

            this.resolver = resolver;
            this.uri = uri;
            this.xml = XDocument.Parse(xml);

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
        /// Gets a reference to the current <see cref="Xml"/> being handled.
        /// </summary>
        public XDocument Xml
        {
            get { return xml; }
        }

        /// <summary>
        /// Gets a reference to the <see cref="IResolver"/> which is used to save or load external resources.
        /// </summary>
        public IResolver Resolver
        {
            get { return resolver; }
        }

        /// <summary>
        /// Gets the loaded module instance of the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetModule<T>()
            where T : Module
        {
            return modules.OfType<T>().FirstOrDefault();
        }

        /// <summary>
        /// Gets all the loaded modules.
        /// </summary>
        public IEnumerable<Module> Modules
        {
            get { return modules; }
        }

        /// <summary>
        /// Gets a reference to the per-<see cref="Visual"/> state collection.
        /// </summary>
        public VisualStateCollection VisualState
        {
            get { return visualState; }
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

            // raise the added event for visuals that have not yet had it raised
            foreach (var visual in RootVisual.Descendants())
                visual.RaiseAddedEvent();
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
        /// Gets a reference to the root <see cref="Visual"/> instance for navigating the visual tree.
        /// </summary>
        public ContentVisual RootVisual
        {
            get { return rootVisual ?? (rootVisual = CreateRootVisual()); }
        }

        /// <summary>
        /// Creates a new root <see cref="Visual"/> instance for navigating the visual tree.
        /// </summary>
        /// <returns></returns>
        ContentVisual CreateRootVisual()
        {
            Contract.Ensures(Contract.Result<ContentVisual>() != null);

            return (ContentVisual)((INXDocument)this).CreateVisual(null, Xml.Root) ?? new UnknownRootVisual(this, null, Xml.Root);
        }

        /// <summary>
        /// Creates a <see cref="Visual"/> from the loaded <see cref="Module"/>s.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        Visual CreateVisualFromModules(XElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            return modules.Select(i => i.CreateVisual(element.Name)).FirstOrDefault(i => i != null);
        }

        /// <summary>
        /// Implements IVisualBuilder.CreateVisual.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        Visual INXDocument.CreateVisual(ContentVisual parent, XNode node)
        {
            if (node is XText)
            {
                var v = new TextVisual();
                v.Initialize(this, parent, node);
                return v;
            }
            else if (node is XElement)
            {
                // create new instance of visual using extensions
                var visual = CreateVisualFromModules((XElement)node);
                if (visual != null)
                {
                    visual.Initialize(this, parent, node);

                    // give each module a chance to add additional information to the visual
                    foreach (var module2 in modules)
                        module2.AnnotateVisual(visual);

                    return visual;
                }
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
                xml.ToString(SaveOptions.DisableFormatting),
                nextElementId,
                visualState);
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
