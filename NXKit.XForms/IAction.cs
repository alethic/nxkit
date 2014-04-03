using NXKit.DOMEvents;

namespace NXKit.XForms
{

    /// <summary>
    /// Marks an interface object as an XForms action element.
    /// </summary>
    public interface IAction :
        IEventHandler
    {

        /// <summary>
        /// Invokes the action.
        /// </summary>
        void Invoke();

    }

}
