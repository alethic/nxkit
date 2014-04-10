namespace NXKit.XForms
{

    /// <summary>
    /// Provides an interface to an item that can be selected.
    /// </summary>
    [Remote]
    public interface ISelectable
    {

        /// <summary>
        /// Gets a unique value that can be used to specify the selection.
        /// </summary>
        [Remote]
        string Id { get; }

        /// <summary>
        /// Applies the appropriate value to the binding for a selection.
        /// </summary>
        /// <param name="binding"></param>
        void Select(UIBinding binding);

        /// <summary>
        /// Unapplies the appropriate value to the binding for a selection.
        /// </summary>
        /// <param name="binding"></param>
        void Deselect(UIBinding binding);

        /// <summary>
        /// Returns <c>true</c> if the selectable is selected by the binding.
        /// </summary>
        /// <param name="binding"></param>
        /// <returns></returns>
        bool IsSelected(UIBinding binding);

        /// <summary>
        /// Gets a hash code that identifies the value of the selectable.
        /// </summary>
        int GetValueHashCode();

    }

}