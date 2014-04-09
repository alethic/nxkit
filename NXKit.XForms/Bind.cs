using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;

namespace NXKit.XForms
{

    [Interface("{http://www.w3.org/2002/xforms}bind")]
    public class Bind
    {

        readonly XElement element;
        readonly ModelItemPropertyAttributes attributes;
        readonly Lazy<IBindingNode> nodeBinding;
        readonly Lazy<EvaluationContext> context;
        readonly Lazy<Binding> calculateBinding;
        readonly Lazy<Binding> readOnlyBinding;
        readonly Lazy<Binding> requiredBinding;
        readonly Lazy<Binding> relevantBinding;
        readonly Lazy<Binding> constraintBinding;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Bind(XElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.element = element;
            this.attributes = new ModelItemPropertyAttributes(element);
            this.nodeBinding = new Lazy<IBindingNode>(() => element.Interface<IBindingNode>());

            this.context = new Lazy<EvaluationContext>(() => element.Interface<EvaluationContextResolver>().Context);
            this.calculateBinding = new Lazy<Binding>(() => BindingUtil.ForAttribute(attributes.CalculateAttribute));
            this.readOnlyBinding = new Lazy<Binding>(() => BindingUtil.ForAttribute(attributes.ReadOnlyAttribute));
            this.requiredBinding = new Lazy<Binding>(() => BindingUtil.ForAttribute(attributes.RequiredAttribute));
            this.relevantBinding = new Lazy<Binding>(() => BindingUtil.ForAttribute(attributes.RelevantAttribute));
            this.constraintBinding = new Lazy<Binding>(() => BindingUtil.ForAttribute(attributes.CalculateAttribute));
        }

        /// <summary>
        /// Gets the 'bind' element.
        /// </summary>
        public XElement Element
        {
            get { return element; }
        }

        /// <summary>
        /// Gets the model item property attribute collection.
        /// </summary>
        public ModelItemPropertyAttributes Attributes
        {
            get { return attributes; }
        }

        /// <summary>
        /// Gets the evaluation context under which binding expressions are built.
        /// </summary>
        public EvaluationContext Context
        {
            get { return context.Value; }
        }

        /// <summary>
        /// Gets the binding of the element.
        /// </summary>
        public Binding Binding
        {
            get { return nodeBinding.Value != null ? nodeBinding.Value.Binding : null; }
        }

        public IEnumerable<ModelItem> ModelItems
        {
            get { return GetModelItems(); }
        }

        IEnumerable<ModelItem> GetModelItems()
        {
            if (Binding != null)
                return Binding.ModelItems ?? Enumerable.Empty<ModelItem>();
            else
                throw new Exception();
        }

        public XName Type
        {
            get { return GetModelItemType(); }
        }

        XName GetModelItemType()
        {
            if (Attributes.Type == null)
                return null;

            // lookup namespace of type specifier
            var nc = new XFormsXsltContext(Element, Context);
            var st = Attributes.Type.Split(':');
            var ns = st.Length == 2 ? nc.LookupNamespace(st[0]) : null;
            var lp = st.Length == 2 ? st[1] : st[0];

            return XName.Get(lp, ns);
        }

        /// <summary>
        /// Extracts a boolean value from the given binding.
        /// </summary>
        /// <param name="binding"></param>
        /// <returns></returns>
        bool? ParseBooleanValue(Lazy<Binding> binding)
        {
            if (binding.Value == null)
                return null;

            if (binding.Value.Result is bool)
                return (bool?)binding.Value.Result;
            else if (binding.Value.Result is bool?)
                return (bool?)binding.Value.Result;
            else if (binding.Value.Result is string && !string.IsNullOrWhiteSpace((string)binding.Value.Result))
                return bool.Parse((string)binding.Value.Result);
            else
                throw new InvalidOperationException();
        }

        public string Calculate
        {
            get { return calculateBinding.Value != null ? calculateBinding.Value.Value : null; }
        }

        public bool? ReadOnly
        {
            get { return ParseBooleanValue(readOnlyBinding); }
        }

        public bool? Required
        {
            get { return ParseBooleanValue(requiredBinding); }
        }

        public bool? Relevant
        {
            get { return ParseBooleanValue(relevantBinding); }
        }

        public bool? Constraint
        {
            get { return ParseBooleanValue(constraintBinding); }
        }

        /// <summary>
        /// Refreshes all of the bindings.
        /// </summary>
        public void Refresh()
        {
            if (nodeBinding.IsValueCreated &&
                nodeBinding.Value != null)
                nodeBinding.Value.Binding.Refresh();

            if (calculateBinding.IsValueCreated &&
                calculateBinding.Value != null)
                calculateBinding.Value.Refresh();

            if (readOnlyBinding.IsValueCreated &&
                readOnlyBinding.Value != null)
                readOnlyBinding.Value.Refresh();

            if (requiredBinding.IsValueCreated &&
                requiredBinding.Value != null)
                requiredBinding.Value.Refresh();

            if (relevantBinding.IsValueCreated &&
                relevantBinding.Value != null)
                relevantBinding.Value.Refresh();

            if (constraintBinding.IsValueCreated &&
                constraintBinding.Value != null)
                constraintBinding.Value.Refresh();
        }

    }

}
