namespace NXKit.XForms.Web.UI
{

    /// <summary>
    /// Describes a control that can be brought into focus.
    /// </summary>
    public interface IFocusTarget
    {

        /// <summary>
        /// Gets the ClientID of the component that should be targeted to gain focus.
        /// </summary>
        string TargetID { get; }

    }

}
