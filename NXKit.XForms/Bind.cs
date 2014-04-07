using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;

namespace NXKit.XForms
{

    [NXElementInterface("{http://www.w3.org/2002/xforms}bind")]
    public class Bind
    {

        readonly NXElement element;
        readonly ModelItemPropertyAttributes attributes;
        readonly Lazy<IBindingNode> nodeBinding;
        readonly Lazy<EvaluationContext> context;
        readonly Lazy<Binding> calculate;
        readonly Lazy<Binding> readOnly;
        readonly Lazy<Binding> required;
        readonly Lazy<Binding> relevant;
        readonly Lazy<Binding> constraint;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Bind(NXElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.element = element;
            this.attributes = new ModelItemPropertyAttributes(element);
            this.nodeBinding = new Lazy<IBindingNode>(() => element.Interface<IBindingNode>());

            this.context = new Lazy<EvaluationContext>(() => element.ResolveEvaluationContext());
            this.calculate = new Lazy<Binding>(() => attributes.Calculate != null ? new Binding(element, Context, attributes.Calculate) : null);
            this.readOnly = new Lazy<Binding>(() => attributes.ReadOnly != null ? new Binding(element, Context, attributes.ReadOnly) : null);
            this.required = new Lazy<Binding>(() => attributes.Required != null ? new Binding(element, Context, attributes.Required) : null);
            this.relevant = new Lazy<Binding>(() => attributes.Relevant != null ? new Binding(element, Context, attributes.Relevant) : null);
            this.constraint = new Lazy<Binding>(() => attributes.Constraint != null ? new Binding(element, Context, attributes.Constraint) : null);
        }

        /// <summary>
        /// Gets the 'bind' element.
        /// </summary>
        public NXElement Element
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
            get { return calculate.Value != null ? calculate.Value.Value : null; }
        }

        public bool? ReadOnly
        {
            get { return ParseBooleanValue(readOnly); }
        }

        public bool? Required
        {
            get { return ParseBooleanValue(required); }
        }

        public bool? Relevant
        {
            get { return ParseBooleanValue(relevant); }
        }

        public bool? Constraint
        {
            get { return ParseBooleanValue(constraint); }
        }

        /// <summary>
        /// Refreshes all of the bindings.
        /// </summary>
        public void Refresh()
        {
            if (nodeBinding.IsValueCreated &&
                nodeBinding.Value != null)
                nodeBinding.Value.Binding.Refresh();

            if (calculate.IsValueCreated &&
                calculate.Value != null)
                calculate.Value.Refresh();

            if (readOnly.IsValueCreated &&
                readOnly.Value != null)
                readOnly.Value.Refresh();

            if (required.IsValueCreated &&
                required.Value != null)
                required.Value.Refresh();

            if (relevant.IsValueCreated &&
                relevant.Value != null)
                relevant.Value.Refresh();

            if (constraint.IsValueCreated &&
                constraint.Value != null)
                constraint.Value.Refresh();
        }

    }

}
