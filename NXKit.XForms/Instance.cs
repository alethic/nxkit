using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;

using NXKit.Composition;
using NXKit.DOMEvents;
using NXKit.XForms.IO;
using NXKit.XForms.XmlSchema;
using NXKit.Xml;

namespace NXKit.XForms
{

    [Extension("{http://www.w3.org/2002/xforms}instance")]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class Instance :
        ElementExtension,
        IOnLoad
    {

        readonly IModelRequestService requestService;
        readonly Extension<InstanceAttributes> attributes;
        readonly Lazy<InstanceState> state;
        readonly IEnumerable<IXsdTypeConverter> xsdTypeConverters;

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
            Extension<InstanceAttributes> attributes,
            IModelRequestService requestService,
            [ImportMany] IEnumerable<IXsdTypeConverter> xsdTypeConverters)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(requestService != null);
            Contract.Requires<ArgumentNullException>(xsdTypeConverters != null);

            this.requestService = requestService;
            this.attributes = attributes;
            this.state = new Lazy<InstanceState>(() => Element.AnnotationOrCreate<InstanceState>());
            this.xsdTypeConverters = xsdTypeConverters.ToList();
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
            if (attributes.Value.Src != null)
                Load(attributes.Value.Src);
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
        }

        void IOnLoad.Load()
        {
            // ensure instances are reloaded properly
            State.Initialize(Model, Element);
        }

    }

}
