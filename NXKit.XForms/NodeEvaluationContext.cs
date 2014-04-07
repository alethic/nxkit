using System;
using System.Diagnostics.Contracts;
using System.Linq;

using NXKit.DOMEvents;

namespace NXKit.XForms
{

    /// <summary>
    /// Obtains the evaluation context to be used by a node.
    /// </summary>
    [NXElementInterface]
    public class NodeEvaluationContext
    {

        readonly NXElement element;
        readonly CommonAttributes attributes;
        readonly Lazy<NXElement> modelElement;
        readonly Lazy<Model> model;
        readonly Lazy<EvaluationContext> context;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public NodeEvaluationContext(NXElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.element = element;
            this.attributes = new CommonAttributes(element);
            this.modelElement = new Lazy<NXElement>(() => GetModelElement());
            this.model = new Lazy<Model>(() => modelElement.Value != null ? modelElement.Value.Interface<Model>() : null);
            this.context = new Lazy<EvaluationContext>(() => GetContext());
        }

        /// <summary>
        /// Attempts to obtain the model element.
        /// </summary>
        /// <returns></returns>
        NXElement GetModelElement()
        {
            var r = attributes.Model != null ? element.ResolveId(attributes.Model) : null;
            if (r == null)
                element.Interface<INXEventTarget>().DispatchEvent(Events.BindingException);

            return r;
        }

        /// <summary>
        /// Gets the <see cref="EvaluationContext"/> specified explicitly on the element.
        /// </summary>
        /// <returns></returns>
        EvaluationContext GetProvidedEvaluationContext()
        {
            return element
                .Interfaces<IEvaluationContextProvider>()
                .Select(i => i.Context)
                .FirstOrDefault(i => i != null);
        }

        /// <summary>
        /// Gets the in-scope evaluation context.
        /// </summary>
        /// <returns></returns>
        EvaluationContext GetInScopeEvaluationContext()
        {
            return element.Ancestors()
                .OfType<NXElement>()
                .Select(i => i.ResolveEvaluationContext())
                .FirstOrDefault(i => i != null);
        }

        /// <summary>
        /// Gets the evaluation context to be used by default, unless overridden.
        /// </summary>
        /// <returns></returns>
        EvaluationContext GetInitialEvaluationContext()
        {
            return GetProvidedEvaluationContext() ?? GetInScopeEvaluationContext();
        }

        /// <summary>
        /// Implements the getter for Context.
        /// </summary>
        /// <returns></returns>
        EvaluationContext GetContext()
        {
            // obtain starting context from 'model' attribute or existing in-scope context
            var ec = model.Value != null ? model.Value.DefaultEvaluationContext : GetInitialEvaluationContext();
            if (ec == null)
            {
                element.Interface<INXEventTarget>().DispatchEvent(Events.BindingException);
                return null;
            }

            // 'context' attribute specified, override with resolved context
            if (attributes.Context != null)
            {
                var b = new Binding(element, ec, attributes.Context);
                if (b.ModelItem != null)
                    ec = new EvaluationContext(b.ModelItem.Model, b.ModelItem.Instance, b.ModelItem, 1, 1);
            }

            return ec;
        }

        /// <summary>
        /// Gets the evaluation context provided by the model.
        /// </summary>
        public EvaluationContext Context
        {
            get { return context.Value; }
        }

    }

}
