using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;

using NXKit.DOMEvents;
using NXKit.Xml;

namespace NXKit.XForms
{

    [Interface("{http://www.w3.org/2002/xforms}delete")]
    public class Delete :
        IAction
    {

        readonly XElement element;
        readonly DeleteAttributes attributes;
        readonly Lazy<IBindingNode> bindingNode;
        readonly Lazy<EvaluationContext> context;
        readonly Lazy<Binding> atBinding;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Delete(XElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.element = element;
            this.attributes = new DeleteAttributes(element);
            this.bindingNode = new Lazy<IBindingNode>(() => element.Interface<IBindingNode>());
            this.atBinding = new Lazy<Binding>(() => 
                attributes.At != null ? 
                new Binding(element, attributes.AtAttribute.Interface<AttributeEvaluationContextResolver>().GetContextForAttribute(), attributes.At) : 
                null);
        }

        /// <summary>
        /// Gets the binding applied to the node.
        /// </summary>
        Binding Binding
        {
            get { return bindingNode.Value != null ? bindingNode.Value.Binding : null; }
        }

        public void Handle(Event ev)
        {
            Invoke();
        }

        /// <summary>
        /// Parses the return value of the 'at' expression.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        double ParseAt(string value)
        {
            // no value, signals all values to be deleted
            if (value == null)
                return double.PositiveInfinity;

            double v;
            if (double.TryParse(value, out v))
                // round to nearest integer
                return Math.Round(v);
            else
                // delete last node
                return double.NaN;
        }

        public void Invoke()
        {
            // extract model items for processing
            var items = Binding != null && Binding.ModelItems != null ? Binding.ModelItems.ToArray() : null;
            if (items.Length == 0)
                return;

            // ensure we have at least one node
            var firstNode = Binding.ModelItems.Cast<XElement>().FirstOrDefault();
            if (firstNode == null)
                return;

            // must have a parent
            var parent = firstNode.Parent;
            if (parent == null)
                throw new Exception();

            // parent and children must be in the same namespace
            if (parent.Name.Namespace != firstNode.Name.Namespace)
                throw new Exception();

            // start and end locations
            var at = ParseAt(atBinding.Value != null ? atBinding.Value.Value : null);
            int s, e;
            if (at == double.PositiveInfinity)
            {
                s = 1;
                e = items.Length;
            }
            else if (at == double.NaN)
            {
                s = items.Length;
                e = items.Length;
            }
            else
            {
                s = (int)at;
                e = (int)at;

                // constrain to first element
                if (s < 1)
                    s = 1;

                // constrain to last element
                if (e > Binding.ModelItems.Length)
                    e = Binding.ModelItems.Length;
            }

            // set of instances to dispatch delete events to
            var dispatch = new HashSet<Instance>();

            // cycle through nodes to delete in reverse
            for (int i = e; i >= s; i--)
            {
                var item = items[i];
                if (item == null)
                    continue;

                if (item.ReadOnly)
                    continue;

                var element = item.Xml as XElement;
                if (element != null)
                {
                    if (element == item.Xml.Document.Root)
                        continue;

                    element.Remove();
                }

                var attribute = item.Xml as XAttribute;
                if (attribute != null)
                {
                    if (attribute.IsNamespaceDeclaration)
                        continue;

                    attribute.Remove();
                }

                item.Model.State.Rebuild = true;
                item.Model.State.Recalculate = true;
                item.Model.State.Revalidate = true;
                item.Model.State.Refresh = true;

                dispatch.Add(item.Instance);
            }

            // dispatch appropriate events
            foreach (var instance in dispatch)
                instance.Element.Interface<INXEventTarget>().DispatchEvent(Events.Delete);
        }

    }

}
