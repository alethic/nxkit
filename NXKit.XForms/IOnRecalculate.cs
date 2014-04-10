namespace NXKit.XForms
{

    /// <summary>
    /// Describes an element's participation in the Refresh phase.
    /// </summary>
    public interface IOnRecalculate
    {

        /// <summary>
        /// Refreshes the element's interface.
        /// </summary>
        void Recalculate();

    }

}
