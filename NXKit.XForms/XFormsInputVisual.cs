namespace NXKit.XForms
{

    /// <summary>
    /// This form control enables free-form data entry or a user interface component appropriate to the datatype of the
    /// bound node.
    /// </summary>
    [Visual("input")]
    public class XFormsInputVisual :
        XFormsSingleNodeBindingVisual,
        ISupportsUiCommonAttributes,
        ISupportsIncrementalAttribute
    {

        /// <summary>
        /// Gets or sets the value of the bound data.
        /// </summary>
        [Interactive]
        public object Value
        {
            get { return Binding != null ? Binding.Value : null; }
            set { if (Binding != null) Binding.SetValue(value != null ? value.ToString() : null); }
        }

    }

}
