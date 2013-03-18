using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace NXKit
{

    /// <summary>
    /// Hosts an ISIS Forms document. Provides access to the visual tree for a renderer.
    /// </summary>
    public class Engine : IEngine
    {

        /// <summary>
        /// Configuration for the engine.
        /// </summary>
        public EngineConfiguration Configuration { get; private set; }

        /// <summary>
        /// Set of generated modules.
        /// </summary>
        Module[] modules;

        /// <summary>
        /// Root visual of the document.
        /// </summary>
        StructuralVisual rootVisual;

        /// <summary>
        /// Auto-assigned element ID to use next.
        /// </summary>
        int nextElementId;

        /// <summary>
        /// Stores per-<see cref="Visual"/> state.
        /// </summary>
        VisualStateCollection visualState;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="resolver"></param>
        public Engine(EngineConfiguration configuration, string document, IResourceResolver resolver)
        {
            Initialize(configuration, document, resolver);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="resolver"></param>
        public Engine(EngineConfiguration configuration, XmlDocument document, IResourceResolver resolver)
        {
            Initialize(configuration, document.InnerXml, resolver);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="resolver"></param>
        public Engine(EngineConfiguration configuration, XDocument document, IResourceResolver resolver)
        {
            Initialize(configuration, document.ToString(SaveOptions.DisableFormatting), resolver);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="state"></param>
        public Engine(EngineState state, IResourceResolver resolver)
        {
            Initialize(state, resolver);
        }

        /// <summary>
        /// Initializes a new form.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="resolver"></param>
        private void Initialize(EngineConfiguration configuration, string document, IResourceResolver resolver)
        {
            Configuration = configuration;
            Document = XDocument.Parse(document);
            Resolver = resolver;

            nextElementId = 1;
            visualState = new VisualStateCollection();

            Initialize();
        }

        /// <summary>
        /// Restores a previous form.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="resolver"></param>
        private void Initialize(EngineState state, IResourceResolver resolver)
        {
            Configuration = state.Configuration;
            Document = XDocument.Parse(state.Document);
            Resolver = resolver;

            nextElementId = state.NextElementId;
            visualState = state.VisualState;

            Initialize();
        }

        /// <summary>
        /// Initializes modules
        /// </summary>
        void Initialize()
        {
            // create modules
            modules = Configuration.ModuleTypes
                .Select(i => (Module)Activator.CreateInstance(i))
                .ToArray();

            // initialize modules
            foreach (var module in modules)
                module.Initialize(this);
        }

        /// <summary>
        /// Gets a reference to the current <see cref="Document"/> being handled.
        /// </summary>
        public XDocument Document { get; private set; }

        /// <summary>
        /// Gets a reference to the <see cref="IResourceResolver"/> which is used to save or load external resources.
        /// </summary>
        public IResourceResolver Resolver { get; private set; }

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
        /// Gets a reference to the per-<see cref="Visual"/> state collection.
        /// </summary>
        public VisualStateCollection VisualState
        {
            get { return visualState; }
        }

        /// <summary>
        /// Runs any outstanding actions.
        /// </summary>
        public void Run()
        {
            // run each module until no module does anything
            bool run;
            do
            {
                run = false;
                foreach (var module in modules)
                    run |= module.Run();
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
        public StructuralVisual RootVisual
        {
            get { return rootVisual ?? (rootVisual = CreateRootVisual()); }
        }

        /// <summary>
        /// Creates a new root <see cref="Visual"/> instance for navigating the visual tree.
        /// </summary>
        /// <returns></returns>
        StructuralVisual CreateRootVisual()
        {
            return (StructuralVisual)((IEngine)this).CreateVisual(null, Document.Root);
        }

        /// <summary>
        /// Creates a <see cref="Visual"/> from the loaded <see cref="Module"/>s.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        Visual CreateVisualFromModules(XElement element)
        {
            return modules.Select(i => i.CreateVisual(element.Name)).FirstOrDefault(i => i != null);
        }

        /// <summary>
        /// Implements IVisualBuilder.CreateVisual.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        Visual IEngine.CreateVisual(StructuralVisual parent, XNode node)
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
        public EngineState Save()
        {
            return new EngineState()
            {
                Configuration = Configuration,
                Document = Document.ToString(SaveOptions.DisableFormatting),
                NextElementId = nextElementId,
                VisualState = visualState,
            };
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
