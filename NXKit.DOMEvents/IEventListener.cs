using System;
using System.Diagnostics.Contracts;

namespace NXKit.DOMEvents
{

    [ContractClass(typeof(IEventListener_Contract))]
    public interface IEventListener
    {

        void HandleEvent(Event evt);

    }

    [ContractClassFor(typeof(IEventListener))]
    abstract class IEventListener_Contract :
         IEventListener
    {

        public void HandleEvent(Event evt)
        {
            Contract.Requires<ArgumentNullException>(evt != null);
            throw new NotImplementedException();
        }

    }

}
