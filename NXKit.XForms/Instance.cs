using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;

using NXKit.DOMEvents;
using NXKit.XForms.IO;
using NXKit.Xml;

namespace NXKit.XForms
{

    [Interface("{http://www.w3.org/2002/xforms}instance")]
    public class Instance :
        ElementExtension,
        IOnLoad
    {

        readonly IRequestService requestService;
        readonly InstanceAttributes attributes;
        readonly Lazy<InstanceState> state;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Instance(XElement element, IRequestService requestService)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(requestService != null);

            this.requestService = requestService;
            this.attributes = new InstanceAttributes(Element);
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
                    throw new DOMTargetEventException(Element, Events.LinkException);

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
            try
            {
                Load(new Uri(resourceUri, UriKind.RelativeOrAbsolute));
            }
            catch (UriFormatException)
            {
                throw new DOMTargetEventException(Element, Events.LinkException);
            }
        }

        /// <summary>
        /// Loads the instance data from the given <see cref="Uri"/>.
        /// </summary>
        /// <param name="resourceUri"></param>
        internal void Load(Uri resourceUri)
        {
            try
            {
                // normalize uri with base
                if (Element.GetBaseUri() != null && !resourceUri.IsAbsoluteUri)
                    resourceUri = new Uri(Element.GetBaseUri(), resourceUri);
            }
            catch (UriFormatException)
            {
                throw new DOMTargetEventException(Element, Events.LinkException);
            }

            // return resource as a stream
            var response = requestService.Submit(new Request(resourceUri, RequestMethod.Get));
            if (response == null ||
                response.Status == ResponseStatus.Error)
                throw new DOMTargetEventException(Element, Events.LinkException);

            // load instance
            Load(response.Body);
        }

        /// <summary>
        /// Loads the instance data from the given <see cref="XDocument"/>.
        /// </summary>
        /// <param name="document"></param>
        internal void Load(XDocument document)
        {
            State.Initialize(Model, Element, document);
        }

        void IOnLoad.Load()
        {
            // ensure instances are reloaded properly
            State.Initialize(Model, Element);
        }

    }

}
