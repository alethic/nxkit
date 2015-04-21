using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Schema;

using NXKit.Composition;
using NXKit.DOMEvents;
using NXKit.Util;
using NXKit.XForms.IO;
using NXKit.XForms.Converters;
using NXKit.Xml;

namespace NXKit.XForms
{

    [Extension("{http://www.w3.org/2002/xforms}instance")]
    [Extension(typeof(IOnLoad), "{http://www.w3.org/2002/xforms}instance")]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class Instance :
        ElementExtension,
        IOnLoad
    {

        readonly InstanceAttributes attributes;
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
        [ImportingConstructor]
        public Instance(
            XElement element,
            InstanceAttributes attributes,
            IModelRequestService requestService,
            [ImportMany] IEnumerable<IXsdTypeConverter> xsdTypeConverters)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(attributes != null);
            Contract.Requires<ArgumentNullException>(requestService != null);
            Contract.Requires<ArgumentNullException>(xsdTypeConverters != null);

            this.requestService = requestService;
            this.attributes = attributes;
            this.xsdTypeConverters = xsdTypeConverters.ToList();
            this.state = new Lazy<InstanceState>(() => Element.AnnotationOrCreate<InstanceState>());
        }

        /// <summary>
        /// Gets the model of the instance.
        /// </summary>
        XElement Model
        {
            get { return Element.Ancestors(Constants.XForms_1_0 + "model").First(); }
        }

        /// <summary>
        /// Gets the instance state associated with this instance visual.
        /// </summary>
        public InstanceState State
        {
            get { return state.Value; }
        }

        /// <summary>
        /// Gets the available <see cref="IXsdTypeConverter"/>s for this instance.
        /// </summary>
        public IEnumerable<IXsdTypeConverter> XsdTypeConverters
        {
            get { return xsdTypeConverters; }
        }

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
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(resourceUri));

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
            Contract.Requires<ArgumentNullException>(resourceUri != null);

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
            Contract.Requires<ArgumentNullException>(document != null);

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
                .Select(i => i.AnnotationOrCreate<ModelItem>(() => new ModelItem(i)));

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
            // initiate validation against schema
            State.Document.Validate(
                Model.Interface<Model>().State.XmlSchemas,
                XmlSchemaValidate_ValidationEvent,
                true);
        }

        /// <summary>
        /// Invoked when an XML validation event occurs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void XmlSchemaValidate_ValidationEvent(object sender, EventArgs args)
        {

        }

    }

}
