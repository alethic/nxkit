using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;
using NXKit.Xml;


namespace NXKit.XForms
{

    [Interface("{http://www.w3.org/2002/xforms}repeat")]
    public class Repeat :
        ElementExtension,
        IOnInit,
        IOnRefresh
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
                Constants.XForms_1_0 + "template",
                Element.GetNamespacePrefixAttributes(),
                Element.Nodes());
            Element.RemoveNodes();
        }

        /// <summary>
        /// Dynamically generate repeat items, reusing existing instances if available.
        /// </summary>
        /// <returns></returns>
        void RefreshNodes()
        {
            // store current index item
            var lastIndexItem = Element
                .Nodes()
                .FirstOrDefault(i => i.AnnotationOrCreate<RepeatItemState>().Index == Index);

            // build new list of properly ordered nodes
            var items = Binding != null ? Binding.ModelItems.Select(i => i.Xml).ToArray() : new XObject[0];
            var nodes = Element.Elements().ToArray();
            var sorts = new XElement[items.Length];
            for (int index = 0; index < items.Length; index++)
            {
                // model item at current index
                var item = items[index];

                // get existing item or create new
                var indx = Array.FindIndex(nodes, i => i != null && i.Annotation<RepeatItemState>().ModelItemId == item.GetObjectId());
                var node = indx >= 0 ? nodes[indx] :
                    new XElement(
                        Constants.XForms_1_0 + "group",
                        Template.GetNamespacePrefixAttributes(),
                        Template.Nodes());

                // remove node from source list
                if (indx >= 0) nodes[indx] = null;

                // set node into output list
                sorts[index] = node;

                // configure item state
                var anno = node.AnnotationOrCreate<RepeatItemState>();
                anno.ModelItemId = item.GetObjectId();
                anno.Index = index + 1;
                anno.Size = items.Length;
            }

            // remove nodes which are no longer present
            nodes.Where(i => i != null).Remove();
            nodes = Element.Elements().ToArray();

            for (int i = 0; i < sorts.Length; i++)
            {
                // node is currently in the correct position
                if (nodes.Length > i &&
                    nodes[i] == sorts[i])
                    continue;

                // current position is occupied by a different node
                else if (nodes.Length > i &&
                    nodes[i] != sorts[i])
                {
                    nodes[i].AddBeforeSelf(sorts[i]);
                    nodes = Element.Elements().ToArray();
                }

                // new item is at the end of the node set
                else
                    Element.Add(sorts[i]);
            }

            // model-construct-done sequence applied to new children
            foreach (var node in Element.Elements())
                foreach (var i in GetAllExtensions<IOnRefresh>(node))
                    i.RefreshBinding();

            // discard refresh events
            foreach (var node in Element.Elements())
                foreach (var i in GetAllExtensions<IOnRefresh>(node))
                    i.DiscardEvents();

            // final refresh
            foreach (var node in Element.Elements())
                foreach (var i in GetAllExtensions<IOnRefresh>(node))
                    i.Refresh();

            // restore or reset index
            if (lastIndexItem != null &&
                lastIndexItem.Parent != null)
                Index = lastIndexItem.AnnotationOrCreate<RepeatItemState>().Index;
            else if (Element.Elements().Count() > 0)
                Index = 1;
            else
                Index = 0;
        }

        /// <summary>
        /// Gets all implementations of the given extension type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<T> GetAllExtensions<T>(XElement root)
        {
            Contract.Requires<ArgumentNullException>(root != null);

            return root
                .DescendantNodesAndSelf()
                .SelectMany(i => i.Interfaces<T>());
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

        /// <summary>
        /// Gets the <see cref="EvaluationContext"/> for a specific item.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        internal EvaluationContext GetItemContext(XElement element)
        {
            var item = element.Annotation<RepeatItemState>();
            if (item == null)
                throw new InvalidOperationException();

            if (Binding == null ||
                Binding.ModelItems == null)
                return null;

            var xml = Binding.ModelItem.Instance.State.Document.ResolveObjectId(item.ModelItemId);
            if (xml == null)
                throw new InvalidOperationException();

            return new EvaluationContext(
                ModelItem.Get(xml),
                item.Index,
                item.Size);
        }

        void IOnInit.Init()
        {
            Initialize();
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


    }

}
