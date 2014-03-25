namespace NXKit.XForms
{

    /// <summary>
    /// Describes a <see cref="XFormsElement"/> which provides a value to be selected for a list control.
    /// </summary>
    public interface ISelectableNode
    {

        void Select(SingleNodeBindingElement visual);

        void Deselect(SingleNodeBindingElement visual);

        bool Selected(SingleNodeBindingElement visual);

        /// <summary>
        /// Gets a hash code that identifies the value of the selectable.
        /// </summary>
        int GetValueHashCode();

    }

}