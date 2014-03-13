
namespace NXKit.XForms
{

    /// <summary>
    /// Abstract implementation for all visuals that support binding expressions.
    /// </summary>
    public abstract class XFormsBindingVisual : 
        XFormsVisual, 
        IEvaluationContextScope
    {

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
