namespace NXKit.XForms
{

    /// <summary>
    /// Provides an interface to an item that can be selected.
    /// </summary>
    public interface ISelectableValue : IElementExtension
    {

        /// <summary>
        /// Applies the appropriate value to the binding for a selection.
        /// </summary>
        /// <param name="ui"></param>
        void Select(UIBinding ui);

        /// <summary>
        /// Unapplies the appropriate value to the binding for a selection.
        /// </summary>
        /// <param name="ui"></param>
        void Deselect(UIBinding ui);

        /// <summary>
        /// Returns <c>true</c> if the selectable is selected by the binding.
        /// </summary>
        /// <param name="ui"></param>
        /// <returns></returns>
        bool IsSelected(UIBinding ui);

        /// <summary>
        /// Gets a hash code that identifies the value of the selectable.
        /// </summary>
        int GetValueHashCode();

    }

}