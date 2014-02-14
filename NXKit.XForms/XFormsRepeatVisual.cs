using System.Collections.Generic;
using System.Xml.Linq;

using NXKit.Util;

namespace NXKit.XForms
{

    [Visual("repeat")]
    public class XFormsRepeatVisual : XFormsNodeSetBindingVisual, INamingScope
    {

        bool startIndexCached;
        int startIndex;
        bool numberCached;
        int? number;

        Dictionary<XObject, XFormsRepeatItemVisual> items = new Dictionary<XObject, XFormsRepeatItemVisual>();

        /// <summary>
        /// Dynamically generate repeat items, reusing existing instances if available.
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
         XFormsRepeatItemVisual GetOrCreateItem(XFormsModelVisual model, XFormsInstanceVisual instance, XObject node, int position, int size)
        {
            // new context for child
            var ec = new XFormsEvaluationContext(model, instance, node, position, size);

            // obtain or create child
            var item = items.GetOrDefault(node);
            if (item == null)
            {
                item = items[node] = new XFormsRepeatItemVisual();
                item.Initialize(Engine, this, Element);
                item.SetContext(ec);
            }
            else
                item.SetContext(ec);

            return item;
        }

        /// <summary>
        /// Provides no context to children, as children are dynamically generated items.
        /// </summary>
        public override XFormsEvaluationContext Context
        {
            get { return null; }
        }

        /// <summary>
        /// Gets or sets the current index of the repeat.
        /// </summary>
        public int Index
        {
            get { return GetState<XFormsRepeatState>().Index ?? StartIndex; }
            set { GetState<XFormsRepeatState>().Index = value; }
        }

        /// <summary>
        /// Optional 1-based initial value of the repeat index.
        /// </summary>
        public int StartIndex
        {
            get
            {
                if (!startIndexCached)
                {
                    var startIndexAttr = Module.GetAttributeValue(Element, "startindex");
                    if (startIndexAttr != null)
                        startIndex = int.Parse(startIndexAttr);
                    else
                        startIndex = 1;

                    startIndexCached = true;
                }

                return startIndex;
            }
        }

        /// <summary>
        /// Optional hint to the XForms Processor as to how many elements from the collection to display.
        /// </summary>
        public int? Number
        {
            get
            {
                if (!numberCached)
                {
                    var numberAttr = Module.GetAttributeValue(Element, "number");
                    if (numberAttr != null)
                        number = int.Parse(numberAttr);

                    numberCached = true;
                }

                return number;
            }
        }

        public override void Refresh()
        {
            // ensure index value is within range
            if (Index < 0)
                if (Binding == null ||
                    Binding.Nodes == null ||
                    Binding.Nodes.Length == 0)
                    Index = 0;
                else
                    Index = StartIndex;

            if (Binding != null &&
                Binding.Nodes != null)
                if (Index > Binding.Nodes.Length)
                    Index = Binding.Nodes.Length;

            base.Refresh();
        }

    }

}
