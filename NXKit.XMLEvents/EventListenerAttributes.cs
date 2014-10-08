using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.Composition;

namespace NXKit.XMLEvents
{

    [Extension(typeof(EventListenerAttributes))]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class EventListenerAttributes :
        AttributeAccessor
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        [ImportingConstructor]
        public EventListenerAttributes(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
        }


        /// <summary>
        /// Gets the value of the 'event' attribute.
        /// </summary>
        /// <returns></returns>
        public string Event
        {
            get { return GetAttributeValue("event"); }
        }

        /// <summary>
        /// Gets the value of the 'handler' attribute.
        /// </summary>
        /// <returns></returns>
        public string Handler
        {
            get { return GetAttributeValue("handler"); }
        }

        /// <summary>
        /// Gets the value of the 'observer' attribute.
        /// </summary>
        /// <returns></returns>
        public string Observer
        {
            get { return GetAttributeValue("observer"); }
        }

        /// <summary>
        /// Gets the value of the 'target' attribute.
        /// </summary>
        /// <returns></returns>
        public string Target
        {
            get { return GetAttributeValue("target"); }
        }

        /// <summary>
        /// Gets the value of the 'phase' attribute.
        /// </summary>
        /// <returns></returns>
        public string Phase
        {
            get { return GetAttributeValue("phase"); }
        }

        /// <summary>
        /// Gets the value of the 'propagate' attribute.
        /// </summary>
        /// <returns></returns>
        public string Propagate
        {
            get { return GetAttributeValue("propagate"); }
        }

        /// <summary>
        /// Gets the value of the 'defaultAction' attribute.
        /// </summary>
        /// <returns></returns>
        public string DefaultAction
        {
            get { return GetAttributeValue("defaultAction"); }
        }

    }

}
