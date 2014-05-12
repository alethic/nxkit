namespace NXKit.DOMEvents
{

    /// <summary>
    /// <see cref="NXNode"/>s implementing this interface provide a default action for the specified event type.
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
