namespace NXKit.XForms
{

    /// <summary>
    /// Describes a <see cref="XFormsVisual"/> which provides a value to be selected for a list control.
    /// </summary>
    public interface ISelectableVisual
    {

        void Select(XFormsSingleNodeBindingVisual visual);

        void Deselect(XFormsSingleNodeBindingVisual visual);

        bool Selected(XFormsSingleNodeBindingVisual visual);

        /// <summary>
        /// Gets a hash code that identifies the value of the selectable.
        /// </summary>
        int GetValueHashCode();

    }

}