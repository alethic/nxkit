using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Linq;

using NXKit.DOMEvents;
using NXKit.Xml;

namespace NXKit.XForms
{

    [Interface("{http://www.w3.org/2002/xforms}instance")]
    public class Instance :
        IOnInitialize
    {

        readonly XElement element;
        readonly InstanceAttributes attributes;
        readonly Lazy<InstanceState> state;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Instance(XElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.element = element;
            this.attributes = new InstanceAttributes(element);
            this.state = new Lazy<InstanceState>(() => element.AnnotationOrCreate<InstanceState>());
        }

        /// <summary>
        /// Gets the associated element.
        /// </summary>
        public XElement Element
        {
            get { return element; }
        }

        /// <summary>
        /// Gets the model of the instance.
        /// </summary>
        XElement Model
        {
            get { return element.Ancestors(Constants.XForms_1_0 + "model").First(); }
        }

        /// <summary>
        /// Gets the instance state associated with this instance visual.
        /// </summary>
        public InstanceState State
        {
            get { return state.Value; }
        }

        /// <summary>
        /// Loads the instance data.
        /// </summary>
        internal bool Load()
        {
            if (attributes.Src != null)
            {
                try
                {
                    // normalize uri with base
                    var u = new Uri(attributes.Src, UriKind.RelativeOrAbsolute);
                    if (element.BaseUri() != null && !u.IsAbsoluteUri)
                        u = new Uri(new Uri(element.BaseUri()), u);

                    // return resource as a stream
                    var request = WebRequest.Create(u);
                    request.Method = "GET";
                    var response = request.GetResponse().GetResponseStream();
                    if (response == null)
                        throw new FileNotFoundException("Could not load resource", attributes.Src);

                    // parse resource into new DOM
                    var instanceDataDocument = XDocument.Load(response);

                    // add to model
                    State.Initialize(Model, element, instanceDataDocument);

                    // clear body of instance
                    element.RemoveNodes();

                    return true;
                }
                catch (UriFormatException)
                {
                    throw new DOMTargetEventException(element, Events.BindingException);
                }
            }

            // extract instance values from xml
            var instanceChildElements = element.Elements().ToArray();
            element.RemoveNodes();

            // invalid number of children elements
            if (instanceChildElements.Length >= 2)
                throw new DOMTargetEventException(element, Events.LinkException);

            if (instanceChildElements.Length == 1)
            {
                State.Initialize(Model, element, new XDocument(instanceChildElements[0]));
                return true;
            }

            return false;
        }

        void IOnInitialize.Init()
        {
            State.Initialize(Model, element);
        }

    }

}
