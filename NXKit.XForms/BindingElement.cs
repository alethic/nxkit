using System;
using System.Diagnostics.Contracts;
using System.Xml;
using System.Xml.Linq;

using NXKit.DOMEvents;
using NXKit.Xml;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides a <see cref="Binding"/> for a UI element.
    /// </summary>
    [Interface(XmlNodeType.Element)]
    public class BindingElement :
        IBindingNode
    {

        readonly XElement element;
        readonly BindingAttributes attributes;
        readonly Lazy<EvaluationContextResolver> resolver;
        readonly Lazy<Binding> binding;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public BindingElement(XElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.element = element;
            this.attributes = new BindingAttributes(element);
            this.resolver = new Lazy<EvaluationContextResolver>(() => element.Interface<EvaluationContextResolver>());
            this.binding = new Lazy<Binding>(() => GetOrCreateBinding());
        }

        /// <summary>
        /// Gets the node binding attributes of the element.
        /// </summary>
        public BindingAttributes Attributes
        {
            get { return attributes; }
        }

        /// <summary>
        /// Gets the binding provided.
        /// </summary>
        public Binding Binding
        {
            get { return binding.Value; }
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
            var context = resolver.Value.Context;
            if (context == null)
                throw new DOMTargetEventException(element, Events.BindingException);

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
                throw new DOMTargetEventException(element, Events.BindingException);

            var binding = bind.InterfaceOrDefault<IBindingNode>();
            if (binding != null)
                return binding.Binding;

            return null;
        }

        /// <summary>
        /// Returns a new context based on this node's binding.
        /// </summary>
        public EvaluationContext Context
        {
            get { return GetContext(); }
        }

        /// <summary>
        /// Implements the getter for <see cref="Context" />.
        /// </summary>
        /// <returns></returns>
        EvaluationContext GetContext()
        {
            if (Binding != null &&
                Binding.ModelItem != null)
                return new EvaluationContext(Binding.ModelItem.Model, Binding.ModelItem.Instance, Binding.ModelItem, 1, 1);

            return null;
        }

    }

}
