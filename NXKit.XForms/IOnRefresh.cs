namespace NXKit.XForms
{

    /// <summary>
    /// Describes an element's participation in the Refresh phase.
    /// </summary>
    public interface IOnRefresh
    {

        /// <summary>
        /// Refreshes the element's binding.
        /// </summary>
        void RefreshBinding();

        /// <summary>
        /// Refreshes the element's interface.
        /// </summary>
        void Refresh();

        /// <summary>
        /// Dispatches any pending events.
        /// </summary>
        void DispatchEvents();

        /// <summary>
        /// Discards any pending events.
        /// </summary>
        void DiscardEvents();

    }

}
