using System.Collections.Generic;
using System.Xml.Linq;

using NXKit.Util;

namespace NXKit.XForms
{

    [Element("itemset")]
    public class ItemSetElement :
        NodeSetBindingElement,
        INamingScope
    {

        readonly Dictionary<XObject, ItemSetItemElement> items =
            new Dictionary<XObject, ItemSetItemElement>();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="xml"></param>
        public ItemSetElement(XElement xml)
            : base(xml)
        {

        }

        protected override EvaluationContext CreateEvaluationContext()
        {
            return null;
        }

        protected override Binding CreateBinding()
        {
            return Module.ResolveNodeSetBinding(this);
        }

        protected override void CreateNodes()
        {
            RemoveNodes();

            if (Binding == null ||
                Binding.ModelItems == null)
                return;

            // obtain or create items
            for (int i = 0; i < Binding.ModelItems.Length; i++)
                Add(GetOrCreateItem(Binding.Context.Model, Binding.Context.Instance, Binding.ModelItems[i], i + 1, Binding.ModelItems.Length));
        }

        ItemSetItemElement GetOrCreateItem(ModelElement model, InstanceElement instance, ModelItem modelItem, int position, int size)
        {
            // new context for child
            var ec = new EvaluationContext(model, instance, modelItem, position, size);

            // obtain or create child
            var item = items.GetOrDefault(modelItem.Xml);
            if (item == null)
                item = items[modelItem.Xml] = new ItemSetItemElement(Xml);

            item.SetContext(ec);

            return item;
        }

    }

}
