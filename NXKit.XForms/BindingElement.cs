using System.Xml.Linq;

namespace NXKit.XForms
{

    /// <summary>
    /// Abstract implementation for all visuals that support binding expressions.
    /// </summary>
    public abstract class BindingElement :
        XFormsElement,
        IEvaluationContextScope
    {

        bool evaluationContextCached;
        EvaluationContext evaluationContext;
        Binding binding;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        protected BindingElement()
            : base()
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="xml"></param>
        protected BindingElement(XElement xml)
            : base(xml)
        {

        }

        /// <summary>
        /// Returns the context which will be inherited by scoped elements.
        /// </summary>
        public EvaluationContext Context
        {
            get { return GetEvaluationContext(); }
        }

        /// <summary>
        /// Implements the getter for Context.
        /// </summary>
        /// <returns></returns>
        EvaluationContext GetEvaluationContext()
        {
            if (!evaluationContextCached)
            {
                evaluationContext = CreateEvaluationContext();
                evaluationContextCached = true;
            }

            return evaluationContext;
        }

        /// <summary>
        /// Creates a new <see cref="EvaluationContext"/> for this <see cref="NXNode"/>.
        /// </summary>
        /// <returns></returns>
        protected abstract EvaluationContext CreateEvaluationContext();

        /// <summary>
        /// Creates a new <see cref="Binding"/>.
        /// </summary>
        /// <returns></returns>
        protected abstract Binding CreateBinding();

        /// <summary>
        /// Gets a reference to the default binding expressed on this node.
        /// </summary>
        public Binding Binding
        {
            get { return binding; }
        }

        /// <summary>
        /// Performs the initial binding.
        /// </summary>
        internal void Bind()
        {
            if (binding == null)
                binding = CreateBinding();
        }

        /// <summary>
        /// Refreshes the visual from the underlying data.
        /// </summary>
        public virtual void Refresh()
        {
            Bind();

            evaluationContext = null;
            evaluationContextCached = false;

            if (binding != null)
                binding.Refresh();
        }

    }

}
