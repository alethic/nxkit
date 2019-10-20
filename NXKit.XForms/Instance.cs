using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Schema;

using NXKit.Diagnostics;
using NXKit.DOMEvents;
using NXKit.XForms.Converters;
using NXKit.XForms.IO;
using NXKit.Xml;

namespace NXKit.XForms
{

    [Extension("{http://www.w3.org/2002/xforms}instance")]
    [Extension(typeof(IOnLoad), "{http://www.w3.org/2002/xforms}instance")]
    public class Instance :
        ElementExtension,
        IOnLoad
    {

        readonly InstanceAttributes attributes;
        readonly ITraceService trace;
        readonly IModelRequestService requestService;
        readonly IEnumerable<IXsdTypeConverter> xsdTypeConverters;
        readonly Lazy<InstanceState> state;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attributes"></param>
        /// <param name="requestService"></param>
        /// <param name="xsdTypeConverters"></param>
        public Instance(
            XElement element,
            InstanceAttributes attributes,
            IModelRequestService requestService,
            IEnumerable<IXsdTypeConverter> xsdTypeConverters,
            ITraceService trace)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
            if (xsdTypeConverters == null)
                throw new ArgumentNullException(nameof(xsdTypeConverters));

            this.requestService = requestService ?? throw new ArgumentNullException(nameof(requestService));
            this.attributes = attributes ?? throw new ArgumentNullException(nameof(attributes));
            this.xsdTypeConverters = xsdTypeConverters.ToList();
            this.trace = trace ?? throw new ArgumentNullException(nameof(trace));

            state = new Lazy<InstanceState>(() => Element.AnnotationOrCreate<InstanceState>());
        }

        /// <summary>
        /// Gets the model of the instance.
        /// </summary>
        private XElement Model => Element.Ancestors(Constants.XForms + "model").First();

        /// <summary>
        /// Gets the instance state associated with this instance visual.
        /// </summary>
        public InstanceState State => state.Value;

        /// <summary>
        /// Gets the available <see cref="IXsdTypeConverter"/>s for this instance.
        /// </summary>
        public IEnumerable<IXsdTypeConverter> XsdTypeConverters => xsdTypeConverters;

        /// <summary>
        /// Loads the instance data from the instance element.
        /// </summary>
        internal void Load()
        {
            if (attributes.Src != null)
                Load(attributes.Src);
            else
            {
                // extract instance data model from xml
                var instanceChildElements = Element.Elements().ToArray();
                Element.RemoveNodes();

                // invalid number of elements
                if (instanceChildElements.Length >= 2)
                    throw new DOMTargetEventException(Element, Events.LinkException,
                        "Instance can only have single child element.");

                // proper number of elements
                if (instanceChildElements.Length == 1)
                    Load(new XDocument(instanceChildElements[0].PrefixSafeClone()));
            }
        }

        /// <summary>
        /// Loads the instance data from the given URI in string format.
        /// </summary>
        /// <param name="resourceUri"></param>
        internal void Load(string resourceUri)
        {
            if (string.IsNullOrEmpty(resourceUri))
                throw new ArgumentOutOfRangeException(nameof(resourceUri));

            try
            {
                Load(new Uri(resourceUri, UriKind.RelativeOrAbsolute));
            }
            catch (UriFormatException e)
            {
                throw new DOMTargetEventException(Element, Events.LinkException, e);
            }
        }

        /// <summary>
        /// Loads the instance data from the given <see cref="Uri"/>.
        /// </summary>
        /// <param name="resourceUri"></param>
        internal void Load(Uri resourceUri)
        {
            if (resourceUri == null)
                throw new ArgumentNullException(nameof(resourceUri));

            try
            {
                // normalize uri with base
                if (Element.GetBaseUri() != null && !resourceUri.IsAbsoluteUri)
                    resourceUri = new Uri(Element.GetBaseUri(), resourceUri);
            }
            catch (UriFormatException e)
            {
                throw new DOMTargetEventException(Element, Events.LinkException, e);
            }

            // return resource as a stream
            var response = requestService.Submit(new ModelRequest(resourceUri, ModelMethod.Get));
            if (response == null ||
                response.Status == ModelResponseStatus.Error)
                throw new DOMTargetEventException(Element, Events.LinkException,
                    string.Format("Error retrieving resource '{0}'.", resourceUri));

            // load instance
            Load(response.Body);
        }

        /// <summary>
        /// Loads the instance data from the given <see cref="XDocument"/>.
        /// </summary>
        /// <param name="document"></param>
        internal void Load(XDocument document)
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));

            State.Initialize(Model, Element, document);

            // applies XML schema validation information
            XmlSchemaValidate();
        }

        void IOnLoad.Load()
        {
            // ensure instances are reloaded properly
            State.Initialize(Model, Element);
        }

        internal void Rebuild()
        {
            XmlSchemaValidate();
        }

        internal void Calculate()
        {

        }

        /// <summary>
        /// Validates the instance data.
        /// </summary>
        internal void Validate()
        {
            // all model items
            var modelItems = State.Document.Root
                .DescendantNodesAndSelf()
                .OfType<XElement>()
                .SelectMany(i => i.Attributes().Cast<XObject>().Prepend(i))
                .Select(i => i.AnnotationOrCreate<ModelItem>(() => new ModelItem(i, trace)));

            // rerun the XML schema validation
            XmlSchemaValidate();

            // initiate individual model item validation
            foreach (var modelItem in modelItems)
                modelItem.Validate();
        }

        /// <summary>
        /// Runs the XML schema validation which attaches annotations to the model items.
        /// </summary>
        internal void XmlSchemaValidate()
        {
            var schema = Model.Interface<Model>().State.XmlSchemas;
            if (schema.IsCompiled == false)
                throw new InvalidOperationException("The model's XML schema has not yet been compiled.");

            try
            {
                State.Document.Validate(schema, XmlSchemaValidate_ValidationEvent, true);
            }
            catch (XmlSchemaValidationException e)
            {
                XmlSchemaValidationEvent(e, null, XmlSeverityType.Error);
            }
        }

        /// <summary>
        /// Invoked when an XML validation event occurs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void XmlSchemaValidate_ValidationEvent(object sender, ValidationEventArgs args)
        {
            XmlSchemaValidationEvent(args.Exception, args.Message, args.Severity);
        }

        /// <summary>
        /// Invoked when an exception occurs during validation.
        /// </summary>
        /// <param name="exception"></param>
        void XmlSchemaValidationEvent(XmlSchemaException exception, string message, XmlSeverityType severity)
        {
            // default message to exception output
            if (message == null && exception != null)
                message = exception.Message;

            // log based on severity
            switch (severity)
            {
                case XmlSeverityType.Error:
                    trace.Error(message);
                    break;
                case XmlSeverityType.Warning:
                    trace.Warning(message);
                    break;
            }
        }

    }

}
