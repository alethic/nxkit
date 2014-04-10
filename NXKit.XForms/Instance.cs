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
        ElementExtension,
        IOnInitialize
    {

        readonly InstanceAttributes attributes;
        readonly Lazy<InstanceState> state;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Instance(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

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
                    if (Element.BaseUri() != null && !u.IsAbsoluteUri)
                        u = new Uri(new Uri(Element.BaseUri()), u);

                    // return resource as a stream
                    var request = WebRequest.Create(u);
                    request.Method = "GET";
                    var response = request.GetResponse().GetResponseStream();
                    if (response == null)
                        throw new FileNotFoundException("Could not load resource", attributes.Src);

                    // parse resource into new DOM
                    var instanceDataDocument = XDocument.Load(response);

                    // add to model
                    State.Initialize(Model, Element  , instanceDataDocument);

                    // clear body of instance
                    Element.RemoveNodes();

                    return true;
                }        
                catch (UriFormatException)
                {
                    throw new DOMTargetEventException(Element, Events.BindingException);
                }
            }

            // extract instance values from xml
            var instanceChildElements = Element.Elements().ToArray();
            Element.RemoveNodes();

            // invalid number of children elements
            if (instanceChildElements.Length >= 2)
                throw new DOMTargetEventException(Element, Events.LinkException);

            if (instanceChildElements.Length == 1)
            {
                State.Initialize(Model, Element, new XDocument(instanceChildElements[0]));
                return true;
            }

            return false;
        }

        void IOnInitialize.Init()
        {
            State.Initialize(Model, Element);
        }

    }

}
