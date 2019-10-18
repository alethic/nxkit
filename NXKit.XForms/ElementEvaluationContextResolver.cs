using System;
using System.Linq;
using System.Xml.Linq;
using NXKit.Diagnostics;
using NXKit.DOMEvents;
using NXKit.Xml;

namespace NXKit.XForms
{

    /// <summary>
    /// Resolves various <see cref="EvaluationContext"/> instances with regards to the specified <see cref="XElement"/>.
    /// </summary>
    [Extension(typeof(EvaluationContextResolver), ExtensionObjectType.Element)]
    public class ElementEvaluationContextResolver :
        EvaluationContextResolver,
        IEvaluationContextScope,
        IExtension
    {

        readonly Lazy<CommonAttributes> attributes;
        readonly ITraceService trace;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attributes"></param>
        /// <param name="trace"></param>
        public ElementEvaluationContextResolver(
            XElement element,
            Lazy<CommonAttributes> attributes,
            ITraceService trace)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            this.attributes = attributes ?? throw new ArgumentNullException(nameof(attributes));
            this.trace = trace ?? throw new ArgumentNullException(nameof(trace));
        }

        /// <summary>
        /// Gets the <see cref="XElement"/> this instance is resolving <see cref="EvaluationContext"/>s for.
        /// </summary>
        public XElement Element
        {
            get { return (XElement)base.Object; }
        }

        /// <summary>
        /// Gets the common attribute set for the <see cref="XElement"/>.
        /// </summary>
        public CommonAttributes Attributes
        {
            get { return attributes.Value; }
        }

        /// <summary>
        /// Resolves the <see cref="Model"/> interface given the 'model' attribute.
        /// </summary>
        /// <returns></returns>
        Model ResolveModel()
        {
            if (Attributes.Model != null)
            {
                var model = Element.ResolveId(Attributes.Model);
                if (model == null)
                    throw new DOMTargetEventException(Element, Events.BindingException,
                        string.Format("Could not resolve model IDREF '{0}'", Attributes.Model));

                return model.Interface<Model>();
            }

            return null;
        }

        /// <summary>
        /// Returns the <see cref="EvaluationContext"/> provided by any specified 'model' attribute.
        /// </summary>
        /// <returns></returns>
        internal EvaluationContext GetSpecifiedModelEvaluationContext()
        {
            var model = ResolveModel();
            if (model == null)
                return null;

            return model.DefaultEvaluationContext;
        }

        /// <summary>
        /// Gets the <see cref="EvaluationContext"/> to be used when executing the expression specified by the
        /// 'context' attribute.
        /// </summary>
        /// <returns></returns>
        internal EvaluationContext GetContextForSpecifiedContext()
        {
            return
                GetSpecifiedModelEvaluationContext() ??
                GetSelfEvaluationContext() ??
                GetInScopeEvaluationContext() ??
                GetDefaultEvaluationContext();
        }

        /// <summary>
        /// Returns the <see cref="EvaluationContext"/> provided by any specified 'context' attribute.
        /// </summary>
        /// <returns></returns>
        internal EvaluationContext GetSpecifiedContextEvaluationContext()
        {
            if (Attributes.Context != null)
            {
                var context = GetContextForSpecifiedContext();
                if (context == null)
                    throw new DOMTargetEventException(Element, Events.BindingException,
                        "Null Context for specified Context.");

                var binding = new Binding(Element, context, Attributes.Context, trace);
                if (binding.ModelItem == null)
                    return null;

                return new EvaluationContext(binding.ModelItem.Model, binding.ModelItem.Instance, binding.ModelItem, 1, 1);
            }

            return null;
        }

        /// <summary>
        /// Returns the <see cref="EvaluationContext"/> to be used by any binding elements.
        /// </summary>
        /// <returns></returns>
        internal EvaluationContext GetContextForBinding()
        {
            return
                GetSpecifiedContextEvaluationContext() ??
                GetSpecifiedModelEvaluationContext() ??
                base.GetContext();
        }

        /// <summary>
        /// Returns the <see cref="EvaluationContext"/> computed by any binding expressions on the element.
        /// </summary>
        /// <returns></returns>
        internal EvaluationContext GetBindingEvaluationContext()
        {
            return Element
                .Interfaces<IBindingNode>()
                .Select(i => i.Context)
                .FirstOrDefault(i => i != null);
        }

        protected override EvaluationContext GetContext()
        {
            return GetContextForBinding();
        }

        /// <summary>
        /// Returns the <see cref="EvaluationContext"/> that is provided to descendant elements as their initial
        /// in-scope evaluation context.
        /// </summary>
        /// <returns></returns>
        internal EvaluationContext GetContextForDescendants()
        {
            return
                GetBindingEvaluationContext() ??
                GetSpecifiedContextEvaluationContext() ??
                GetSpecifiedModelEvaluationContext() ??
                base.GetContext();
        }

        EvaluationContext IEvaluationContextScope.Context
        {
            get { return GetContextForDescendants(); }
        }

    }

}
