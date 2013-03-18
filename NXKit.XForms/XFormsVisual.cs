using System.Xml.Linq;

namespace NXKit.XForms
{

    /// <summary>
    /// Base class for XForms <see cref="Visual"/> types.
    /// </summary>
    public abstract class XFormsVisual : StructuralVisual
    {

        /// <summary>
        /// Gets a reference to the <see cref="XFormsModule"/> instance.
        /// </summary>
        public XFormsModule Module
        {
            get { return Engine.GetModule<XFormsModule>(); }
        }

        /// <summary>
        /// Unique identifier for the <see cref="Visual"/> within the current naming scope.
        /// </summary>
        public override string Id
        {
            get { return Module.Engine.GetElementId(Element); }
        }

    }

}
