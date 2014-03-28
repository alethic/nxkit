using System;
using System.Diagnostics.Contracts;

namespace NXKit.DOMEvents
{

    /// <summary>
    /// The DocumentEvent interface provides a mechanism by which the user can create an Event object of a type supported by the implementation. The DocumentEvent interface must be implemented on the same object that implements the Document interface.
    /// </summary>
    [ContractClass(typeof(IDocumentEvent_Contract))]
    public interface IDocumentEvent
    {

        /// <summary>
        /// Creates an event object of the type specified. Returns the newly created object.
        /// </summary>
        /// <param name="eventInterface"></param>
        /// <returns></returns>
        Event CreateEvent(string eventInterface);

    }

    [ContractClassFor(typeof(IDocumentEvent))]
    abstract class IDocumentEvent_Contract :
        IDocumentEvent
    {

        public Event CreateEvent(string eventInterface)
        {
            Contract.Requires<ArgumentNullException>(eventInterface != null);
            Contract.Requires<ArgumentNullException>(eventInterface.Length != 0);
            throw new NotImplementedException();
        }

    }

}
