using System;
using System.Diagnostics.Contracts;
using NXKit.DOMEvents;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides a <see cref="Binding"/> for a UI element.
    /// </summary>
    [NXElement(null, null)]
    public class NodeBinding :
        IBinding
    {

        readonly NXElement element;
        string ref_;
        Binding binding;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public NodeBinding(NXElement element)
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
        public string RefAttribute
        {
            get { return ref_ ?? (ref_ = GetAttribute("ref") ?? GetAttribute("nodeset")); }
        }

        /// <summary>
        /// Gets the 'bind' attribute value.
        /// </summary>
        public string BindAttribute
        {
            get { return GetAttribute("bind"); }
        }

        /// <summary>
        /// Gets the evaluation context to be used for the binding.
        /// </summary>
        public EvaluationContext EvaluationContext
        {
            get { return GetEvaluationContext(); }
        }

        /// <summary>
        /// Implements the getter for EvaluationContext.
        /// </summary>
        /// <returns></returns>
        EvaluationContext GetEvaluationContext()
        {
            var local = element.InterfaceOrDefault<NodeUIBindingEvaluationContext>();
            if (local != null)
                if (local.Context != null)
                    return local.Context;

            var model = element.InterfaceOrDefault<NodeUIBindingEvaluationContextModel>();
            if (model != null)
                if (model.Context != null)
                    return model.Context;

            var scope = element.InterfaceOrDefault<NodeUIBindingEvaluationContextInScope>();
            if (scope != null)
                if (scope.Context != null)
                    return scope.Context;

            return null;
        }

        /// <summary>
        /// Gets the binding provided.
        /// </summary>
        public Binding Binding
        {
            get { return binding ?? (binding = GetOrCreateBinding()); }
        }

        /// <summary>
        /// Creates the binding.
        /// </summary>
        /// <returns></returns>
        Binding GetOrCreateBinding()
        {
            // bind attribute overrides
            var bind = BindAttribute;
            if (bind != null)
                return GetBindBinding();

            // otherwise 'ref' or 'nodeset'
            var xpath = RefAttribute;
            if (xpath == null)
                return null;

            // obtain evaluation context
            var context = EvaluationContext;
            if (context == null)
                return null;

            return new Binding(element, context, xpath);
        }

        /// <summary>
        /// Gets the <see cref="Binding"/> returned by the referenced 'bind' element.
        /// </summary>
        /// <returns></returns>
        Binding GetBindBinding()
        {
            Contract.Requires(BindAttribute != null);

            // resolve bind element
            var bind = element.ResolveId(BindAttribute);
            if (bind == null)
            {
                element.Interface<INXEventTarget>().DispatchEvent(Events.BindingException);
                return null;
            }

            var binding = bind.InterfaceOrDefault<IBinding>();
            if (binding != null)
                return binding.Binding;

            return null;
        }

    }

}
