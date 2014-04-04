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
        string modelAttr;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public NodeUIBindingEvaluationContextModel(NXElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.element = element;
        }

        /// <summary>
        /// Gets the XForms attribute of the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        string GetAttribute(string name)
        {
            Contract.Requires<ArgumentNullException>(name != null);

            var fq = element.Attribute(Constants.XForms_1_0 + name);
            if (fq != null)
                return (string)fq;

            var ln = element.Name.Namespace == Constants.XForms_1_0 ? element.Attribute(name) : null;
            if (ln != null)
                return (string)ln;

            return null;
        }

        /// <summary>
        /// Gets the 'ref' or 'nodeset' attribute values.
        /// </summary>
        public string ModelAttribute
        {
            get { return modelAttr ?? (modelAttr = GetAttribute("model")); }
        }

        /// <summary>
        /// Gets the specified model element.
        /// </summary>
        public NXElement ModelElement
        {
            get { return ModelAttribute != null ? element.ResolveId(ModelAttribute) : null; }
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
