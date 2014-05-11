using System;
using System.Diagnostics.Contracts;
using System.Xml;
using System.Xml.Linq;

using NXKit.DOM;

namespace NXKit.DOMEvents
{

    /// <summary>
    /// Provides a <see cref="IDocumentEvent"/> implementation.
    /// </summary>
    [Interface(XmlNodeType.Document)]
    public class DocumentEvent :
         IDocumentEvent
    {

        readonly NXDocumentHost host;
        readonly XDocument document;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="document"></param>
        public DocumentEvent(NXDocumentHost host, XDocument document)
        {
            Contract.Requires<ArgumentNullException>(host != null);
            Contract.Requires<ArgumentNullException>(document != null);

            this.host = host;
            this.document = document;
        }

        public Event CreateEvent(string eventInterface)
        {
            switch (eventInterface)
            {
                case "Event":
                    return new Event(host);
                case "UIEvent":
                    return new UIEvent(host);
                case "FocusEvent":
                    return new FocusEvent(host);
                case "MutationEvent":
                    return new MutationEvent(host);
                default:
                    throw new DOMException();
            }
        }

    }

}
