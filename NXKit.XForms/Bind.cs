using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;

using NXKit.Composition;
using NXKit.DOMEvents;
using NXKit.Xml;

namespace NXKit.XForms
{

    [Extension("{http://www.w3.org/2002/xforms}bind")]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class Bind :
        ElementExtension
    {

        readonly string id;
        readonly BindAttributes attributes;
        readonly Extension<IBindingNode> bindingNode;
        readonly Lazy<EvaluationContext> context;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        [ImportingConstructor]
        public Bind(
            XElement element,
            BindAttributes attributes,
            Extension<IBindingNode> bindingNode,
            Extension<EvaluationContextResolver> context)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(attributes != null);
            Contract.Requires<ArgumentNullException>(bindingNode != null);
            Contract.Requires<ArgumentNullException>(context != null);

            this.id = (string)element.Attribute("id");
            this.attributes = attributes;
            this.bindingNode = bindingNode;
            this.context = new Lazy<EvaluationContext>(() => context.Value.Context);
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
            get { return bindingNode.Value != null ? bindingNode.Value.Binding : null; }
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
            if (attributes.Type == null)
                return null;

            return Element.ResolvePrefixedName(attributes.Type);
        }

        /// <summary>
        /// Extracts a boolean value from the given binding.
        /// </summary>
        /// <param name="binding"></param>
        /// <returns></returns>
        bool? ParseBooleanValue(Binding binding)
        {
            Contract.Requires<ArgumentNullException>(binding != null);

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
            var modelItems = Binding.ModelItems.ToList();
            var parentBind = Element.Ancestors(Constants.XForms_1_0 + "bind")
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
                        modelItems.AddRange(new Binding(xpath, ec, (string)xpath).ModelItems);
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

                var state = modelItem.State;
                if (state == null)
                    continue;

                if (Type != null)
                    if (state.Type != Type)
                        state.Type = Type;

                var ec = new EvaluationContext(modelItem.Model, modelItem.Instance, modelItem, i, modelItems.Length);

                if (!string.IsNullOrWhiteSpace(attributes.ReadOnly))
                {
                    var readOnly = ParseBooleanValue(new Binding(Element, ec, attributes.ReadOnly));
                    if (readOnly != null)
                        state.ReadOnly = readOnly;
                }

                if (!string.IsNullOrWhiteSpace(attributes.Required))
                {
                    var required = ParseBooleanValue(new Binding(Element, ec, attributes.Required));
                    if (required != null)
                        state.Required = required;
                }

                if (!string.IsNullOrWhiteSpace(attributes.Relevant))
                {
                    var relevant = ParseBooleanValue(new Binding(Element, ec, attributes.Relevant));
                    if (relevant != null &&
                        relevant != state.Relevant)
                    {
                        state.Relevant = relevant;
                        Debug.WriteLine("ModelItem relevancy changed: {0}", state.Relevant);
                    }
                }

                if (!string.IsNullOrWhiteSpace(attributes.Constraint))
                {
                    var constraint = ParseBooleanValue(new Binding(Element, ec, attributes.Constraint));
                    if (constraint != null)
                        state.Constraint = constraint;
                }

                if (!string.IsNullOrWhiteSpace(attributes.Calculate))
                {
                    var calculate = new Binding(Element, ec, attributes.Calculate).Value;
                    if (calculate != null)
                    {
                        if (state.ReadOnly == false)
                            state.ReadOnly = true;

                        modelItem.Value = calculate;
                    }
                }
            }
        }

    }

}
