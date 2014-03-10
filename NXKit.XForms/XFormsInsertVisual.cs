using System;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace NXKit.XForms
{

    [Visual("insert")]
    public class XFormsInsertVisual : XFormsNodeSetBindingVisual, IActionVisual
    {

        public void Handle(Event ev)
        {
            Module.InvokeAction(this);
        }

        public void Invoke()
        {
            // ensure values are up to date
            Refresh();

            if (Binding == null ||
                Binding.Nodes == null ||
                Binding.Nodes.Length == 0)
                return;

            // only element nodes allowed
            if (Binding.Nodes.Any(i => !(i is XElement)))
                throw new Exception();

            // ensure we have at least one node
            var firstNode = (XElement)Binding.Nodes[0];
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

            var atAttr = Module.GetAttributeValue(Element, "at");
            if (atAttr == null)
                throw new Exception();

            var positionAttr = Module.GetAttributeValue(Element, "position");
            if (positionAttr == null)
                throw new Exception();

            // evaluation context is either own context, or in-scope context
            var ec = Context ?? Binding.Context;
            var nc = new XFormsXsltContext(this);

            var atD = (double?)Module.EvaluateXPath(ec, nc, this, atAttr, XPathResultType.Number);

            if (double.IsNaN((double)atD))
            {
                // outside legal range, we insert after last node
                atD = Binding.Nodes.Length;
                positionAttr = "after";
            }
            else if (atD < 1)
                // out of range, start
                atD = 1;
            else if (atD > Binding.Nodes.Length)
                // out of range, end
                atD = Binding.Nodes.Length;

            int at = (int)atD;
            int index;

            // current node
            var curNode = (XElement)Binding.Nodes[at - 1];

            // clone last node to create new node
            var newNode = new XElement((XElement)Binding.Nodes[Binding.Nodes.Length - 1]).Elements().First();

            foreach (var i in curNode.Descendants().Zip(newNode.Descendants(), (a, b) => new { A = a, B = b }))
            {
                var miA = Module.GetModelItem(i.A);
                var miB = Module.GetModelItem(i.B);

                miB.Type = miA.Type;
                miB.Relevant = miA.Relevant;
                miB.ReadOnly = miA.ReadOnly;
                miB.Required = miA.Required;
                miB.Clear = false;
                miB.NewElement = null;
                miB.NewValue = null;
            }

            if (positionAttr == "before")
            {
                // before: simply use insert before current node
                curNode.AddBeforeSelf(newNode);
                index = at;
            }
            else if (at == Binding.Nodes.Length)
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
            var nodeSetVisuals = Module.Engine.RootVisual
                .Descendants(true)
                .OfType<XFormsNodeSetBindingVisual>()
                .Where(i => i.Binding != null)
                .Where(i => i.Binding.Nodes != null)
                .Where(i => i.Binding.Nodes.Any(j => j.Parent == parent));

            foreach (var nodeSetVisual in nodeSetVisuals)
            {
                // refresh visual's binding
                nodeSetVisual.Refresh();

                // if visual is a repeat element, resposition it to the new index
                if (nodeSetVisual is XFormsRepeatVisual)
                    ((XFormsRepeatVisual)nodeSetVisual).Index = index;
            }

            // instruct model to complete deferred update
            ec.Model.State.RebuildFlag = true;
            ec.Model.State.RecalculateFlag = true;
            ec.Model.State.RevalidateFlag = true;
            ec.Model.State.RefreshFlag = true;

            ec.Instance.DispatchEvent<XFormsInsertEvent>();
        }

    }

}
