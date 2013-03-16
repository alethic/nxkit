using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace XEngine.Forms
{

    /// <summary>
    /// Hosts an ISIS Forms document. Provides access to the visual tree for a renderer.
    /// </summary>
    public class FormProcessor : IFormProcessor
    {

        /// <summary>
        /// Composes parts available to the forms implementation.
        /// </summary>
        internal static CompositionContainer container = new CompositionContainer(new ApplicationCatalog());

        /// <summary>
        /// Set of modules providing functionality to the form processor.
        /// </summary>
        [ImportMany(typeof(Module))]
        private List<Module> modules = null;

        private StructuralVisual rootVisual;
        private int nextElementId;
        private VisualStateCollection visualState;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="resolver"></param>
        public FormProcessor(string document, IResourceResolver resolver)
        {
            Initialize(document, resolver);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="resolver"></param>
        public FormProcessor(XmlDocument document, IResourceResolver resolver)
        {
            Initialize(document.InnerXml, resolver);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="resolver"></param>
        public FormProcessor(XDocument document, IResourceResolver resolver)
        {
            Initialize(document.ToString(SaveOptions.DisableFormatting), resolver);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="state"></param>
        public FormProcessor(FormProcessorState state, IResourceResolver resolver)
        {
            Initialize(state, resolver);
        }

        /// <summary>
        /// Initializes a new form.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="resolver"></param>
        private void Initialize(string document, IResourceResolver resolver)
        {
            Document = StringToXDocument(document, resolver);
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
        private void Initialize(FormProcessorState state, IResourceResolver resolver)
        {
            Document = StringToXDocument(state.Document, resolver);
            Resolver = resolver;

            nextElementId = state.NextElementId;
            visualState = state.VisualState;

            Initialize();
        }

        /// <summary>
        /// Initializes modules
        /// </summary>
        private void Initialize()
        {
            container.ComposeExportedValue(AttributedModelServices.GetContractName(typeof(FormProcessor)), this);
            container.SatisfyImportsOnce(this);

            // initialize the modules
            foreach (var module in modules)
                module.Initialize();
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
        private StructuralVisual CreateRootVisual()
        {
            return (StructuralVisual)((IFormProcessor)this).CreateVisual(null, Document.Root);
        }

        /// <summary>
        /// Implements IVisualBuilder.CreateVisual.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        Visual IFormProcessor.CreateVisual(StructuralVisual parent, XNode node)
        {
            if (node is XElement)
            {
                var element = (XElement)node;

                // look up descriptor
                var visualTypeDescriptor = VisualTypeDescriptorContainer.DefaultInstance.GetDescriptor(element.Name);
                if (visualTypeDescriptor != null)
                {
                    // create new instance of visual
                    var visual = visualTypeDescriptor.CreateVisual(this, parent, node);
                    if (visual != null)
                    {
                        // give each module a chance to add additional information to the visual
                        foreach (var module2 in modules)
                            module2.AnnotateVisual(visual);

                        return visual;
                    }
                }

                // unknown element
                throw new Exception(string.Format("Could not create Visual from unknown element '{0}'.", element.Name));
            }
            else if (node is XText)
                // default processing for text
                return new TextVisual(this, parent, (XText)node);
            else
                return null;
        }

        /// <summary>
        /// Saves the current state of the processor in a serializable format.
        /// </summary>
        /// <returns></returns>
        public FormProcessorState Save()
        {
            return new FormProcessorState()
            {
                Document = XDocumentToString(Document),
                NextElementId = nextElementId,
                VisualState = visualState,
            };
        }

        /// <summary>
        /// Parses a string into a new <see cref="XDocument"/>, using the available form schema.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="resolver"></param>
        /// <returns></returns>
        public static XDocument StringToXDocument(string document, IResourceResolver resolver)
        {
            var rdr = XmlReader.Create(new System.IO.StringReader(document), new XmlReaderSettings()
            {
                ConformanceLevel = ConformanceLevel.Document,
                DtdProcessing = DtdProcessing.Prohibit,
                IgnoreComments = true,
                IgnoreProcessingInstructions = true,
                IgnoreWhitespace = true,
                Schemas = FormSchema.SchemaSet,
                ValidationFlags = XmlSchemaValidationFlags.AllowXmlAttributes | XmlSchemaValidationFlags.ProcessIdentityConstraints | XmlSchemaValidationFlags.ProcessInlineSchema,
                ValidationType = ValidationType.Schema,
                XmlResolver = null,
            });

            return XDocument.Load(rdr, LoadOptions.None);
        }

        /// <summary>
        /// Transforms the <see cref="XDocument"/> into a string representation.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        internal static string XDocumentToString(XDocument document)
        {
            return document.ToString(SaveOptions.DisableFormatting);
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
