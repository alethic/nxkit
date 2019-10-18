using System;
using System.Xml.Linq;

using NXKit.Composition;
using NXKit.Diagnostics;
using NXKit.DOMEvents;
using NXKit.Xml;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides a <see cref="Binding"/> for an element.
    /// </summary>
    [Extension]
    public class BindingNode :
        ElementExtension,
        IBindingNode
    {

        readonly IExport<BindingProperties> properties;
        readonly IExport<EvaluationContextResolver> resolver;
        readonly ITraceService trace;
        readonly Lazy<Binding> binding;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="properties"></param>
        /// <param name="resolver"></param>
        /// <param name="trace"></param>
        public BindingNode(
            XElement element,
            IExport<BindingProperties> properties,
            IExport<EvaluationContextResolver> resolver,
            ITraceService trace)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            this.properties = properties ?? throw new ArgumentNullException(nameof(properties));
            this.resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
            this.trace = trace ?? throw new ArgumentNullException(nameof(trace));
            this.binding = new Lazy<Binding>(() => GetOrCreateBinding());
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

            return new Binding(Element, context, expression, trace);
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
