namespace NXKit.DOMEvents
{

    /// <summary>
    /// Extensions implementing this interface provide a default action for the specified event type.
    /// </summary>
    public interface IEventDefaultAction
    {

        /// <summary>
        /// Invoked to execute the default action.
        /// </summary>
        /// <param name="evt"></param>
        void DefaultAction(Event evt);

    }

}
