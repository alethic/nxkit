using System.Xml.Linq;
namespace NXKit.XForms
{

    /// <summary>
    /// Abstract implementation for all visuals that support binding expressions.
    /// </summary>
    public abstract class XFormsBindingVisual :
        XFormsVisual,
        IEvaluationContextScope
    {

        bool evaluationContextCached;
        XFormsEvaluationContext evaluationContext;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public XFormsBindingVisual(XElement element)
            : base(element)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="element"></param>
        public XFormsBindingVisual(NXElement parent, XElement element)
            : base(parent, element)
        {

        }

        /// <summary>
        /// Returns the context which will be inherited by scoped elements.
        /// </summary>
        public XFormsEvaluationContext Context
        {
            get { return GetEvaluationContext(); }
        }

        /// <summary>
        /// Implements the getter for Context.
        /// </summary>
        /// <returns></returns>
        XFormsEvaluationContext GetEvaluationContext()
        {
            if (!evaluationContextCached)
            {
                evaluationContext = CreateEvaluationContext();
                evaluationContextCached = true;
            }

            return evaluationContext;
        }

        /// <summary>
        /// Creates a new <see cref="XFormsEvaluationContext"/> for this <see cref="NXNode"/>.
        /// </summary>
        /// <returns></returns>
        protected abstract XFormsEvaluationContext CreateEvaluationContext();

        /// <summary>
        /// Gets a reference to the default binding expressed on this node.
        /// </summary>
        public XFormsBinding Binding { get; protected set; }

        /// <summary>
        /// Refreshes the visual from the underlying data.
        /// </summary>
        public virtual void Refresh()
        {
            evaluationContext = null;
            evaluationContextCached = false;
        }

    }

}
