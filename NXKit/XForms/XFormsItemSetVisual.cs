using System.Collections.Generic;
using System.Xml.Linq;

using NXKit.Util;

namespace NXKit.XForms
{

    [VisualTypeDescriptor(Constants.XForms_1_0_NS, "itemset")]
    public class XFormsItemSetVisualTypeDescriptor : VisualTypeDescriptor
    {

        public override Visual CreateVisual(IFormProcessor form, StructuralVisual parent, XNode node)
        {
            return new XFormsItemSetVisual(parent, (XElement)node);
        }

    }

    public class XFormsItemSetVisual : XFormsNodeSetBindingVisual, INamingScope
    {

        private Dictionary<XObject, XFormsItemSetItemVisual> items = new Dictionary<XObject, XFormsItemSetItemVisual>();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="element"></param>
        public XFormsItemSetVisual(StructuralVisual parent, XElement element)
            : base(parent, element)
        {

        }

        /// <summary>
        /// Dynamically generate itemset items.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<Visual> CreateChildren()
        {
            if (Binding == null ||
                Binding.Nodes == null)
                yield break;

            // obtain or create items
            for (int i = 0; i < Binding.Nodes.Length; i++)
                yield return GetOrCreateItem(Binding.Context.Model, Binding.Context.Instance, Binding.Nodes[i], i + 1, Binding.Nodes.Length);
        }

        /// <summary>
        /// Gets or creates a repeat item for the specified node and position.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="instance"></param>
        /// <param name="node"></param>
        /// <param name="position"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private XFormsItemSetItemVisual GetOrCreateItem(XFormsModelVisual model, XFormsInstanceVisual instance, XObject node, int position, int size)
        {
            // new context for child
            var ec = new XFormsEvaluationContext(model, instance, node, position, size);

            // obtain or create child
            var item = items.ValueOrDefault(node);
            if (item == null)
                item = items[node] = new XFormsItemSetItemVisual(this, Element, ec);
            else
                item.SetContext(ec);

            return item;
        }

        /// <summary>
        /// Provides no context to children, as children are dynamically generated item.
        /// </summary>
        public override XFormsEvaluationContext Context
        {
            get { return null; }
        }

    }

}
