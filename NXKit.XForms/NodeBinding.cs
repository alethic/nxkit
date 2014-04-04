using System;
using System.Diagnostics.Contracts;

using NXKit.DOMEvents;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides a <see cref="Binding"/> for a UI element.
    /// </summary>
    [NXElement("http://www.w3.org/2002/xforms", null)]
    public class NodeBinding :
        INodeBinding
    {

        readonly NXElement element;
        NodeBindingAttributes attributes;
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

        NodeBindingAttributes Attributes
        {
            get { return attributes ?? (attributes = element.Interface<NodeBindingAttributes>()); }
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
            var bindIdRef = Attributes.Bind;
            if (bindIdRef != null)
                return GetBindBinding(bindIdRef);

            // otherwise 'ref' or 'nodeset'
            var expression = Attributes.Ref ?? Attributes.NodeSet;
            if (expression == null)
                return null;

            // obtain evaluation context
            var context = EvaluationContext;
            if (context == null)
                return null;

            return new Binding(element, context, expression);
        }

        /// <summary>
        /// Gets the <see cref="Binding"/> returned by the referenced 'bind' element.
        /// </summary>
        /// <returns></returns>
        Binding GetBindBinding(string bindIdRef)
        {
            // resolve bind element
            var bind = element.ResolveId(bindIdRef);
            if (bind == null)
            {
                element.Interface<INXEventTarget>().DispatchEvent(Events.BindingException);
                return null;
            }

            var binding = bind.InterfaceOrDefault<INodeBinding>();
            if (binding != null)
                return binding.Binding;

            return null;
        }

    }

}
