using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.Util;
using NXKit.Xml;

namespace NXKit.XForms
{

    [Interface("{http://www.w3.org/2002/xforms}repeat")]
    public class Repeat :
        ElementExtension,
        IOnRefresh,
        IOnInitialize
    {

        readonly RepeatAttributes attributes;
        readonly Lazy<IBindingNode> bindingNode;
        readonly Lazy<Binding> binding;
        readonly Lazy<IUIBindingNode> uiBindingNode;
        readonly Lazy<UIBinding> uiBinding;
        readonly Lazy<RepeatState> state;
        readonly Lazy<XElement> template;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Repeat(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.attributes = new RepeatAttributes(Element);
            this.bindingNode = new Lazy<IBindingNode>(() => Element.Interface<IBindingNode>());
            this.binding = new Lazy<Binding>(() => bindingNode.Value.Binding);
            this.uiBindingNode = new Lazy<IUIBindingNode>(() => Element.Interface<IUIBindingNode>());
            this.uiBinding = new Lazy<UIBinding>(() => uiBindingNode.Value.UIBinding);
            this.state = new Lazy<RepeatState>(() => Element.AnnotationOrCreate<RepeatState>());
            this.template = new Lazy<XElement>(() => State.Template);
        }

        RepeatState State
        {
            get { return state.Value; }
        }

        UIBinding UIBinding
        {
            get { return uiBinding.Value; }
        }

        Binding Binding
        {
            get { return binding.Value; }
        }

        /// <summary>
        /// Gets or sets the current repeat index.
        /// </summary>
        public int Index
        {
            get { return State.Index; }
            set { State.Index = value; }
        }

        /// <summary>
        /// Gets or sets the persisted template.
        /// </summary>
        XElement Template
        {
            get { return State.Template; }
            set { State.Template = value; }
        }

        void Initialize()
        {
            // acquire template
            Template = new XElement(
                Constants.XForms_1_0 + "group", 
                Element.GetNamespacePrefixAttributes(),
                Element.Nodes());
            Element.RemoveNodes();
        }

        /// <summary>
        /// Dynamically generate repeat items, reusing existing instances if available.
        /// </summary>
        /// <returns></returns>
        protected void RefreshNodes()
        {
            //if (Binding == null ||
            //    Binding.ModelItems == null)
            //    Element.RemoveNodes();

            //// build proper list of items
            //for (int i = 0; i < Binding.ModelItems.Length; i++)
            //    GetOrCreateItem(Binding.Context.Model, Binding.Context.Instance, Binding.ModelItems[i], i + 1, Binding.ModelItems.Length);

            //// clear stale items from cache
            //foreach (var i in items.ToList())
            //    if (!Nodes().Contains(i.Value))
            //        items.Remove(i.Key);
        }

        /// <summary>
        /// Gets or creates a repeat item for the specified node and position.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="instance"></param>
        /// <param name="modelItem"></param>
        /// <param name="position"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        RepeatItem GetOrCreateItem(Model model, Instance instance, ModelItem modelItem, int position, int size)
        {
            throw new NotImplementedException();

            //// new context for child
            //var ec = new EvaluationContext(model, instance, modelItem, position, size);

            //// obtain or create child
            //var item = items.GetOrDefault(modelItem.Xml);
            //if (item == null)
            //    item = items[modelItem.Xml] = new RepeatItem(Xml);

            //// ensure child is configured
            //item.SetContext(ec);

            //return item;
        }

        /// <summary>
        /// Refreshes the interface of this element.
        /// </summary>
        void Refresh()
        {
            // ensure index value is within range
            if (Index < 0)
                if (Binding == null ||
                    Binding.ModelItems == null ||
                    Binding.ModelItems.Length == 0)
                    Index = 0;
                else
                    Index = attributes.StartIndex;

            if (Binding != null &&
                Binding.ModelItems != null)
                if (Index > Binding.ModelItems.Length)
                    Index = Binding.ModelItems.Length;

            // rebuild node tree
            RefreshNodes();
        }


        void IOnRefresh.RefreshBinding()
        {

        }

        void IOnRefresh.Refresh()
        {
            Refresh();
        }

        void IOnRefresh.DispatchEvents()
        {

        }

        void IOnRefresh.DiscardEvents()
        {

        }

        void IOnInitialize.Initialize()
        {
            Initialize();
        }

    }

}
