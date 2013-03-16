using System.Xml.Linq;

namespace XEngine.Forms
{

    /// <summary>
    /// Base class for XForms <see cref="Visual"/> types.
    /// </summary>
    public abstract class XFormsVisual : StructuralVisual
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="form"></param>
        /// <param name="parent"></param>
        /// <param name="element"></param>
        public XFormsVisual(IFormProcessor form, StructuralVisual parent, XElement element)
            : base(form, parent, element)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="parent"></param>
        /// <param name="element"></param>
        public XFormsVisual(StructuralVisual parent, XElement element)
            : base(parent, element)
        {

        }

        /// <summary>
        /// Gets a reference to the <see cref="XFormsModule"/> instance.
        /// </summary>
        public XFormsModule Module
        {
            get { return Form.GetModule<XFormsModule>(); }
        }

        /// <summary>
        /// Unique identifier for the <see cref="Visual"/> within the current naming scope.
        /// </summary>
        public override string Id
        {
            get { return Module.Form.GetElementId(Element); }
        }

    }

}
