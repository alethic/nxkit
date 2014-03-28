using System;
using System.Diagnostics.Contracts;

namespace NXKit.DOMEvents
{

    /// <summary>
    /// Provides event instances configured as appropriate.
    /// </summary>
    [ContractClass(typeof(IEventFactory_Contract))]
    public interface IEventFactory
    {

        /// <summary>
        /// Creates a new event for the given event type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Event CreateEvent(string type);

        /// <summary>
        /// Creates a new event for the given event type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        T CreateEvent<T>(string type)
            where T : Event;

    }

    [ContractClassFor(typeof(IEventFactory))]
    abstract class IEventFactory_Contract :
        IEventFactory
    {

        public Event CreateEvent(string eventInterface)
        {
            Contract.Requires<ArgumentNullException>(eventInterface != null);
            Contract.Requires<ArgumentNullException>(eventInterface.Length != 0);
            throw new NotImplementedException();
        }

        public T CreateEvent<T>(string type) where T : Event
        {
            Contract.Requires<ArgumentNullException>(type != null);
            Contract.Requires<ArgumentNullException>(type.Length != 0);
            throw new NotImplementedException();
        }

    }

}
