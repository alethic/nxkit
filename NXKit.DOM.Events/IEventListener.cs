using System;
using System.Diagnostics.Contracts;

namespace NXKit.DOM.Events
{

    [ContractClass(typeof(IEventListener_Contract))]
    public interface IEventListener
    {

        void HandleEvent(IEvent evt);

    }

    [ContractClassFor(typeof(IEventListener))]
    abstract class IEventListener_Contract :
         IEventListener
    {

        public void HandleEvent(IEvent evt)
        {
            Contract.Requires<ArgumentNullException>(evt != null);
            throw new NotImplementedException();
        }

    }

}
