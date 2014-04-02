using System;
using System.Diagnostics.Contracts;

using NXKit.DOM;

namespace NXKit.DOMEvents
{

    /// <summary>
    /// Provides a <see cref="IDocumentEvent"/> implementation.
    /// </summary>
    class DocumentEvent :
        IDocumentEvent
    {

        readonly NXDocument document;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="document"></param>
        public DocumentEvent(NXDocument document)
        {
            Contract.Requires<ArgumentNullException>(document != null);

            this.document = document;
        }

        public Event CreateEvent(string eventInterface)
        {
            switch (eventInterface)
            {
                case "Event":
                    return new Event();
                case "UIEvent":
                    return new UIEvent();
                case "FocusEvent":
                    return new FocusEvent();
                default:
                    throw new DOMException();
            }
        }

    }

}
