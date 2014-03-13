using System.Collections.Generic;
using System.Xml.Linq;

using NXKit.Util;

namespace NXKit.XForms
{

    [Visual("itemset")]
    public class XFormsItemSetVisual : XFormsNodeSetBindingVisual, INamingScope
    {

        Dictionary<XObject, XFormsItemSetItemVisual> items = new Dictionary<XObject, XFormsItemSetItemVisual>();

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
        XFormsItemSetItemVisual GetOrCreateItem(XFormsModelVisual model, XFormsInstanceVisual instance, XObject node, int position, int size)
        {
            // new context for child
            var ec = new XFormsEvaluationContext(model, instance, node, position, size);

            // obtain or create child
            var item = items.GetOrDefault(node);
            if (item == null)
            {
                item = items[node] = new XFormsItemSetItemVisual();
                item.Initialize(Document, this, Element);
                item.SetContext(ec);
            }
            else
                item.SetContext(ec);

            return item;
        }

        /// <summary>
        /// Provides no context to children, as children are dynamically generated item.
        /// </summary>
        protected override XFormsEvaluationContext CreateEvaluationContext()
        {
            return null;
        }

    }

}
