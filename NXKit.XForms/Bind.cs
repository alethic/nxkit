using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;

using NXKit.Xml;

namespace NXKit.XForms
{

    [Interface("{http://www.w3.org/2002/xforms}bind")]
    public class Bind :
        ElementExtension
    {

        readonly BindAttributes attributes;
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
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.attributes = new BindAttributes(Element);
            this.nodeBinding = new Lazy<IBindingNode>(() => Element.Interface<IBindingNode>());

            this.context = new Lazy<EvaluationContext>(() => Element.Interface<EvaluationContextResolver>().Context);
            this.calculateBinding = new Lazy<Binding>(() => BindingUtil.ForAttribute(attributes.CalculateAttribute));
            this.readOnlyBinding = new Lazy<Binding>(() => BindingUtil.ForAttribute(attributes.ReadOnlyAttribute));
            this.requiredBinding = new Lazy<Binding>(() => BindingUtil.ForAttribute(attributes.RequiredAttribute));
            this.relevantBinding = new Lazy<Binding>(() => BindingUtil.ForAttribute(attributes.RelevantAttribute));
            this.constraintBinding = new Lazy<Binding>(() => BindingUtil.ForAttribute(attributes.CalculateAttribute));
        }

        /// <summary>
        /// Gets the model item property attribute collection.
        /// </summary>
        BindAttributes Attributes
        {
            get { return attributes; }
        }

        /// <summary>
        /// Gets the evaluation context under which binding expressions are built.
        /// </summary>
        EvaluationContext Context
        {
            get { return context.Value; }
        }

        /// <summary>
        /// Gets the binding of the element.
        /// </summary>
        Binding Binding
        {
            get { return nodeBinding.Value != null ? nodeBinding.Value.Binding : null; }
        }

        IEnumerable<ModelItem> ModelItems
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

            return attributes.TypeAttribute.ResolvePrefixedName(attributes.Type);
        }

        /// <summary>
        /// Extracts a boolean value from the given binding.
        /// </summary>
        /// <param name="binding"></param>
        /// <returns></returns>
        bool? ParseBooleanValue(Lazy<Binding> binding)
        {
            Contract.Requires<ArgumentNullException>(binding != null);

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
        /// Recalculates all of the bindings.
        /// </summary>
        public void Recalculate()
        {
            if (nodeBinding.IsValueCreated &&
                nodeBinding.Value != null)
                nodeBinding.Value.Binding.Recalculate();

            if (calculateBinding.IsValueCreated &&
                calculateBinding.Value != null)
                calculateBinding.Value.Recalculate();

            if (readOnlyBinding.IsValueCreated &&
                readOnlyBinding.Value != null)
                readOnlyBinding.Value.Recalculate();

            if (requiredBinding.IsValueCreated &&
                requiredBinding.Value != null)
                requiredBinding.Value.Recalculate();

            if (relevantBinding.IsValueCreated &&
                relevantBinding.Value != null)
                relevantBinding.Value.Recalculate();

            if (constraintBinding.IsValueCreated &&
                constraintBinding.Value != null)
                constraintBinding.Value.Recalculate();
        }

        /// <summary>
        /// Applies the bindings to the model item.
        /// </summary>
        public void Apply()
        {
            foreach (var modelItem in ModelItems)
            {
                var state = modelItem.State;
                if (state == null)
                    continue;

                // bind applies a type
                if (Type != null)
                    if (state.Type != Type)
                        state.Type = Type;

                // bind applies read-only
                if (ReadOnly != null)
                    if (state.ReadOnly != ReadOnly)
                        state.ReadOnly = ReadOnly;

                // bind applies reqired
                if (Required != null)
                    if (state.Required != Required)
                        state.Required = Required;

                // bind applies relevant
                if (Relevant != null)
                    if (state.Relevant != Relevant)
                        state.Relevant = Relevant;

                // bind applies constraint
                if (Constraint != null)
                    if (state.Constraint != Constraint)
                        state.Constraint = Constraint;

                // bind applies calculate
                if (Calculate != null)
                {
                    if (state.ReadOnly == false)
                        state.ReadOnly = true;
                    if (modelItem.Value != Calculate)
                        modelItem.Value = Calculate;
                }
            }
        }

    }

}
