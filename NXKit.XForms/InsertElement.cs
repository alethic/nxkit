using System;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

using NXKit.DOMEvents;

namespace NXKit.XForms
{

    [Element("insert")]
    public class InsertElement :
        NodeSetBindingElement,
        IActionElement
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="xml"></param>
        public InsertElement(XElement xml)
            : base(xml)
        {

        }

        public void Handle(Event ev)
        {
            Module.InvokeAction(this);
        }

        public void Invoke()
        {
            // ensure values are up to date
            Refresh();

            if (Binding == null ||
                Binding.ModelItems == null ||
                Binding.ModelItems.Length == 0)
                return;

            // only element nodes allowed
            if (Binding.ModelItems.Any(i => !(i is XElement)))
                throw new Exception();

            // ensure we have at least one node
            var firstNode = (XElement)Binding.ModelItems[0];
            if (firstNode == null)
                return;

            // must have a parent
            var parent = firstNode.Parent;
            if (parent == null)
                throw new Exception();

            // parent and children must be in the same namespace
            if (parent.Name.Namespace != firstNode.Name.Namespace)
                throw new Exception();

            // check for non homogeneous collection
            //var qn = new QName(firstNode.getNamespaceURI(), firstNode.getLocalName());
            //if (boundNodes.Any(i => i.getNamespaceURI() != qn.getNamespaceURI() || i.getLocalName() != qn.getLocalPart()))
            //    throw new Exception();

            var atAttr = Module.GetAttributeValue(Xml, "at");
            if (atAttr == null)
                throw new Exception();

            var positionAttr = Module.GetAttributeValue(Xml, "position");
            if (positionAttr == null)
                throw new Exception();

            // evaluation context is either own context, or in-scope context
            var ec = Context ?? Binding.Context;

            // execute xpath
            var atD = (double?)Module.EvaluateXPath(this, ec, atAttr, XPathResultType.Number);

            if (double.IsNaN((double)atD))
            {
                // outside legal range, we insert after last node
                atD = Binding.ModelItems.Length;
                positionAttr = "after";
            }
            else if (atD < 1)
                // out of range, start
                atD = 1;
            else if (atD > Binding.ModelItems.Length)
                // out of range, end
                atD = Binding.ModelItems.Length;

            int at = (int)atD;
            int index;

            // current node
            var curNode = (XElement)Binding.ModelItems[at - 1];

            // clone last node to create new node
            var newNode = new XElement((XElement)Binding.ModelItems[Binding.ModelItems.Length - 1]).Elements().First();

            foreach (var i in curNode.Descendants().Zip(newNode.Descendants(), (a, b) => new { A = a, B = b }))
            {
                var miA = Module.GetModelItem(i.A);
                var miB = Module.GetModelItem(i.B);

                miB.Type = miA.Type;
                miB.Relevant = miA.Relevant;
                miB.ReadOnly = miA.ReadOnly;
                miB.Required = miA.Required;
                miB.Remove = false;
                miB.NewElement = null;
                miB.NewValue = null;
            }

            if (positionAttr == "before")
            {
                // before: simply use insert before current node
                curNode.AddBeforeSelf(newNode);
                index = at;
            }
            else if (at == Binding.ModelItems.Length)
            {
                // after last element: append to parent
                parent.Add(newNode);
                index = at + 1;
            }
            else
            {
                // after non-last element, insert before next sibling
                curNode.NextNode.AddBeforeSelf(newNode);
                index = at + 1;
            }

            // node set visuals that are bound to nodes sharing the same parent
            var nodeSetVisuals = Module.Document.Root
                .Descendants(true)
                .OfType<NodeSetBindingElement>()
                .Where(i => i.Binding != null)
                .Where(i => i.Binding.ModelItems != null)
                .Where(i => i.Binding.ModelItems.Any(j => j.Parent == parent));

            foreach (var nodeSetVisual in nodeSetVisuals)
            {
                // refresh visual's binding
                nodeSetVisual.Refresh();

                // if visual is a repeat element, resposition it to the new index
                if (nodeSetVisual is RepeatElement)
                    ((RepeatElement)nodeSetVisual).Index = index;
            }

            // instruct model to complete deferred update
            ec.Model.State.RebuildFlag = true;
            ec.Model.State.RecalculateFlag = true;
            ec.Model.State.RevalidateFlag = true;
            ec.Model.State.RefreshFlag = true;

            ec.Instance.DispatchEvent<InsertEvent>();
        }

    }

}
