using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.Xml;

namespace NXKit.XForms
{

    [Interface("{http://www.w3.org/2002/xforms}repeat")]
    public class Repeat :
        ElementExtension,
        IOnRefresh
    {

        readonly Lazy<RepeatAttributes> attributes;
        readonly Lazy<IUIBindingNode> uiBindingNode;
        readonly Lazy<UIBinding> uiBinding;
        readonly Lazy<RepeatState> state;
        int nextId;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Repeat(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.attributes = new Lazy<RepeatAttributes>(() => Element.AnnotationOrCreate<RepeatAttributes>(() => new RepeatAttributes(element)));
            this.uiBindingNode = new Lazy<IUIBindingNode>(() => Element.Interface<IUIBindingNode>());
            this.uiBinding = new Lazy<UIBinding>(() => uiBindingNode.Value.UIBinding);
            this.state = new Lazy<RepeatState>(() => Element.AnnotationOrCreate<RepeatState>());
        }

        /// <summary>
        /// Gets the attributes of the repeat element.
        /// </summary>
        public RepeatAttributes Attributes
        {
            get { return attributes.Value; }
        }

        /// <summary>
        /// Allocates a new ID for this naming scope.
        /// </summary>
        /// <returns></returns>
        public string AllocateId()
        {
            return (nextId++).ToString();
        }

        /// <summary>
        /// Dynamically generate repeat items, reusing existing instances if available.
        /// </summary>
        /// <returns></returns>
        protected void CreateNodes()
        {
            throw new NotImplementedException();

            //element.RemoveNodes();

            //if (Binding == null ||
            //    Binding.ModelItems == null)
            //    return;

            //// build proper list of items
            //for (int i = 0; i < Binding.ModelItems.Length; i++)
            //    Add(GetOrCreateItem(Binding.Context.Model, Binding.Context.Instance, Binding.ModelItems[i], i + 1, Binding.ModelItems.Length));

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
            throw new NotImplementedException();

            //// ensure index value is within range
            //if (Index < 0)
            //    if (Binding == null ||
            //        Binding.ModelItems == null ||
            //        Binding.ModelItems.Length == 0)
            //        Index = 0;
            //    else
            //        Index = attributes.StartIndex;

            //if (Binding != null &&
            //    Binding.ModelItems != null)
            //    if (Index > Binding.ModelItems.Length)
            //        Index = Binding.ModelItems.Length;

            //// rebuild node tree
            //CreateNodes();

            //// refresh children
            //foreach (var node in this.Descendants().Select(i => i.InterfaceOrDefault<IUIBindingNode>()))
            //    if (node != null)
            //        node.UIBinding.Refresh();

            //// refresh children
            //foreach (var node in this.Descendants().Select(i => i.InterfaceOrDefault<IUINode>()))
            //    if (node != null)
            //        node.Refresh();
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
