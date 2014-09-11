using System;
using System.Diagnostics.Contracts;

namespace NXKit.DOMEvents
{

    /// <summary>
    /// Provides <see cref="Event"/> instances.
    /// </summary>
    [ContractClass(typeof(IEventInstanceProvider_Contract))]
    public interface IEventInstanceProvider
    {

        Event CreateEvent(string eventInterface);

    }

    [ContractClassFor(typeof(IEventInstanceProvider))]
    abstract class IEventInstanceProvider_Contract :
        IEventInstanceProvider
    {

        public Event CreateEvent(string eventInterface)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(eventInterface));
            throw new System.NotImplementedException();
        }

    }

}
