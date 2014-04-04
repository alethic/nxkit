using System;
using System.Diagnostics.Contracts;

namespace NXKit.XForms
{

    /// <summary>
    /// Obtains the evaluation context for a UI binding element based on it's model.
    /// </summary>
    public class NodeUIBindingEvaluationContextModel
    {

        readonly NXElement element;
        NodeBindingAttributes attributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public NodeUIBindingEvaluationContextModel(NXElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.element = element;
        }

        NodeBindingAttributes Attributes
        {
            get { return attributes ?? (attributes = element.Interface<NodeBindingAttributes>()); }
        }

        /// <summary>
        /// Gets the specified model element.
        /// </summary>
        public NXElement ModelElement
        {
            get { return Attributes.Model != null ? element.ResolveId(Attributes.Model) : null; }
        }

        /// <summary>
        /// Gets the specified model interface.
        /// </summary>
        public Model Model
        {
            get { return ModelElement != null ? ModelElement.InterfaceOrDefault<Model>() : null; }
        }

        /// <summary>
        /// Gets the evaluation context provided by the model.
        /// </summary>
        public EvaluationContext Context
        {
            get { return Model != null ? Model.DefaultEvaluationContext : null; }
        }

    }

}
