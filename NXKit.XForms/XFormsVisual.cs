namespace NXKit.XForms
{

    /// <summary>
    /// Base class for XForms <see cref="Visual"/> types.
    /// </summary>
    public abstract class XFormsVisual : 
        ContentVisual
    {

        /// <summary>
        /// Gets a reference to the <see cref="XFormsModule"/> instance.
        /// </summary>
        public XFormsModule Module
        {
            get { return Document.GetModule<XFormsModule>(); }
        }

        /// <summary>
        /// Unique identifier for the <see cref="Visual"/> within the current naming scope.
        /// </summary>
        public override string Id
        {
            get { return Document.GetElementId(Element); }
        }

    }

}
