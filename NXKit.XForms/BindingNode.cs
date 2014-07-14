using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.Composition;
using NXKit.DOMEvents;
using NXKit.Xml;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides a <see cref="Binding"/> for an element.
    /// </summary>
    [Extension]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class BindingNode :
        ElementExtension,
        IBindingNode
    {

        readonly BindingAttributes attributes;
        readonly Lazy<BindingProperties> properties;
        readonly Lazy<EvaluationContextResolver> resolver;
        readonly Lazy<Binding> binding;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public BindingNode(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.attributes = new BindingAttributes(element);
            this.resolver = new Lazy<EvaluationContextResolver>(() => element.Interface<EvaluationContextResolver>());
            this.binding = new Lazy<Binding>(() => GetOrCreateBinding());
            this.properties = new Lazy<BindingProperties>(() => new BindingProperties(element, resolver));
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
            var bindIdRef = properties.Value.Bind;
            if (bindIdRef != null)
                return GetBindBinding(bindIdRef);

            // determine 'ref' or 'nodeset' attribute
            var expression = properties.Value.Ref ?? properties.Value.NodeSet;
            if (expression == null)
                return null;

            // obtain evaluation context
            var context = resolver.Value.Context;
            if (context == null)
                throw new DOMTargetEventException(Element, Events.BindingException,
                    "Could not resolve binding context.");

            return new Binding(Element, context, expression);
        }

        /// <summary>
        /// Gets the <see cref="Binding"/> returned by the referenced 'bind' element.
        /// </summary>
        /// <returns></returns>
        Binding GetBindBinding(string bindIdRef)
        {
            // resolve bind element
            var bind = Element.ResolveId(bindIdRef);
            if (bind == null)
                throw new DOMTargetEventException(Element, Events.BindingException,
                    string.Format("Could not resolve IDREF '{0}'", bindIdRef));

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
