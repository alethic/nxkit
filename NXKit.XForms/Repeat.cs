﻿using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using NXKit.Util;

namespace NXKit.XForms
{

    [Element("repeat")]
    public class Repeat :
        NodeSetBindingElement,
        INamingScope,
        IUIRefreshable
    {

        int nextId;
        bool startIndexCached;
        int startIndex;
        bool numberCached;
        int? number;

        readonly Dictionary<XObject, RepeatItem> items =
            new Dictionary<XObject, RepeatItem>();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="xml"></param>
        public Repeat(XElement xml)
            : base(xml)
        {

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
        protected override void CreateNodes()
        {
            RemoveNodes();

            if (Binding == null ||
                Binding.ModelItems == null)
                return;

            // build proper list of items
            for (int i = 0; i < Binding.ModelItems.Length; i++)
                Add(GetOrCreateItem(Binding.Context.Model, Binding.Context.Instance, Binding.ModelItems[i], i + 1, Binding.ModelItems.Length));

            // clear stale items from cache
            foreach (var i in items.ToList())
                if (!Nodes().Contains(i.Value))
                    items.Remove(i.Key);
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
        RepeatItem GetOrCreateItem(ModelElement model, InstanceElement instance, ModelItem modelItem, int position, int size)
        {
            // new context for child
            var ec = new EvaluationContext(model, instance, modelItem, position, size);

            // obtain or create child
            var item = items.GetOrDefault(modelItem.Xml);
            if (item == null)
                item = items[modelItem.Xml] = new RepeatItem(Xml);

            // ensure child is configured
            item.SetContext(ec);

            return item;
        }

        /// <summary>
        /// Provides no context to children, as children are dynamically generated items.
        /// </summary>
        protected override EvaluationContext CreateEvaluationContext()
        {
            return null;
        }

        /// <summary>
        /// Gets or sets the current index of the repeat.
        /// </summary>
        public int Index
        {
            get { return GetState<RepeatState>().Index ?? StartIndex; }
            set { GetState<RepeatState>().Index = value; }
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
                    var startIndexAttr = Module.GetAttributeValue(Xml, "startindex");
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
                    var numberAttr = Module.GetAttributeValue(Xml, "number");
                    if (numberAttr != null)
                        number = int.Parse(numberAttr);

                    numberCached = true;
                }

                return number;
            }
        }

        public void Refresh()
        {
            Binding.Refresh();

            // ensure index value is within range
            if (Index < 0)
                if (Binding == null ||
                    Binding.ModelItems == null ||
                    Binding.ModelItems.Length == 0)
                    Index = 0;
                else
                    Index = StartIndex;

            if (Binding != null &&
                Binding.ModelItems != null)
                if (Index > Binding.ModelItems.Length)
                    Index = Binding.ModelItems.Length;

            // rebuild node tree
            CreateNodes();

            // refresh children
            foreach (var node in this.Descendants().Select(i => i.InterfaceOrDefault<IUIBindingNode>()))
                if (node != null)
                    node.UIBinding.Refresh();

            // refresh children
            foreach (var node in this.Descendants().Select(i => i.InterfaceOrDefault<IUIRefreshable>()))
                if (node != null)
                    node.Refresh();
        }

    }

}
