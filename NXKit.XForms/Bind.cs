using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using NXKit.Composition;
using NXKit.Diagnostics;
using NXKit.DOMEvents;
using NXKit.Xml;

namespace NXKit.XForms
{

    [Extension("{http://www.w3.org/2002/xforms}bind")]
    public class Bind : ElementExtension
    {

        readonly BindProperties properties;
        readonly IExport<IBindingNode> bindingNode;
        readonly ITraceService trace;
        readonly Lazy<EvaluationContext> context;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="properties"></param>
        /// <param name="bindingNode"></param>
        /// <param name="context"></param>
        public Bind(
            XElement element,
            BindProperties properties,
            IExport<IBindingNode> bindingNode,
            IExport<EvaluationContextResolver> context,
            ITraceService trace)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            this.properties = properties ?? throw new ArgumentNullException(nameof(properties));
            this.bindingNode = bindingNode ?? throw new ArgumentNullException(nameof(bindingNode));
            this.trace = trace ?? throw new ArgumentNullException(nameof(trace));
            this.context = new Lazy<EvaluationContext>(() => context.Value.Context);
        }

        /// <summary>
        /// Gets the binding of the element.
        /// </summary>
        private Binding GetBinding() => bindingNode.Value?.Binding;

        /// <summary>
        /// Extracts a boolean value from the given binding.
        /// </summary>
        /// <param name="binding"></param>
        /// <returns></returns>
        bool? ParseBooleanValue(Binding binding)
        {
            if (binding == null)
                throw new ArgumentNullException(nameof(binding));

            if (binding.Result is bool)
                return (bool?)binding.Result;
            else if (binding.Result is bool?)
                return (bool?)binding.Result;
            else if (binding.Result is string && !string.IsNullOrWhiteSpace((string)binding.Result))
                return bool.Parse((string)binding.Result);
            else
                throw new DOMTargetEventException(Element, Events.BindingException,
                    string.Format("{0}", binding.Result));
        }

        /// <summary>
        /// Recalculates all of the bindings.
        /// </summary>
        public void Recalculate()
        {
            if (bindingNode.Value != null)
                bindingNode.Value.Binding.Recalculate();
        }

        internal ModelItem[] GetBoundNodes()
        {
            // TODO this is a poor implementation of nested bind elements
            var modelItems = GetBinding().ModelItems.ToList();
            var parentBind = Element.Ancestors(Constants.XForms + "bind")
                .SelectMany(i => i.Interfaces<Bind>())
                .FirstOrDefault();
            if (parentBind != null)
            {
                var xpath = Element.Attribute("ref") ?? Element.Attribute("nodeset");
                if (xpath != null)
                {
                    var parentItems = parentBind.GetBoundNodes();
                    modelItems.Clear();
                    for (int i = 1; i <= parentItems.Length; i++)
                    {
                        var parentModelItem = parentItems[i - 1];
                        var ec = new EvaluationContext(
                            parentModelItem.Model,
                            parentModelItem.Instance,
                            parentModelItem,
                            i,
                            parentItems.Length);
                        modelItems.AddRange(new Binding(xpath, ec, (string)xpath, trace).ModelItems);
                    }
                }
            }

            return modelItems.ToArray();
        }

        /// <summary>
        /// Applies the bindings to the model item.
        /// </summary>
        public void Apply()
        {
            // TODO this is a poor implementation of nested bind elements
            var modelItems = GetBoundNodes();

            for (int i = 1; i <= modelItems.Length; i++)
            {
                var modelItem = modelItems[i - 1];
                if (modelItem == null)
                    continue;

                // evaluation context for attributes
                var context = new Lazy<EvaluationContext>(() => new EvaluationContext(modelItem.Model, modelItem.Instance, modelItem, i, modelItems.Length));

                // item type does not get applied to elements with contents
                if (properties.Type != null && (modelItem.Xml as XElement)?.HasElements != true)
                {
                    modelItem.ItemType = properties.Type;
                }

                if (properties.ReadOnly != null)
                {
                    var readOnly = ParseBooleanValue(new Binding(Element, context.Value, properties.ReadOnly, trace));
                    if (readOnly != null)
                        modelItem.ReadOnly = (bool)readOnly;
                }

                if (properties.Required != null)
                {
                    var required = ParseBooleanValue(new Binding(Element, context.Value, properties.Required, trace));
                    if (required != null)
                        modelItem.Required = (bool)required;
                }

                if (properties.Relevant != null)
                {
                    var relevant = ParseBooleanValue(new Binding(Element, context.Value, properties.Relevant, trace));
                    if (relevant != null)
                        modelItem.Relevant = (bool)relevant;
                }

                if (properties.Constraint != null)
                {
                    var constraint = ParseBooleanValue(new Binding(Element, context.Value, properties.Constraint, trace));
                    if (constraint != null)
                        modelItem.Constraint = (bool)constraint;
                }

                if (properties.Calculate != null)
                {
                    var calculate = new Binding(Element, context.Value, properties.Calculate, trace).Value;
                    modelItem.ReadOnly = true;
                    modelItem.Value = calculate ?? "";
                }
            }
        }

    }

}
