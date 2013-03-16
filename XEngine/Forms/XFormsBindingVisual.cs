using System.Xml.Linq;

namespace XEngine.Forms
{

    public abstract class XFormsBindingVisual : XFormsVisual, IEvaluationContextScope
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="parent"></param>
        /// <param name="element"></param>
        public XFormsBindingVisual(StructuralVisual parent, XElement element)
            : base(parent, element)
        {

        }

        /// <summary>
        /// Returns the context which will be inherited by scoped elements.
        /// </summary>
        public abstract XFormsEvaluationContext Context { get; }

        /// <summary>
        /// Gets a reference to the default binding expressed on this node.
        /// </summary>
        public XFormsBinding Binding { get; protected set; }

        /// <summary>
        /// Refreshes the visual from the underlying data.
        /// </summary>
        public virtual void Refresh()
        {

        }

    }

}
