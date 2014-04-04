using System;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

using NXKit.DOMEvents;

namespace NXKit.XForms
{

    [Element("delete")]
    public class DeleteElement :
        NodeSetBindingElement,
        IAction
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="xml"></param>
        public DeleteElement(XElement xml)
            : base(xml)
        {

        }

        public void Handle(Event ev)
        {
            Module.InvokeAction(this);
        }

        public void Invoke()
        {
            // only element nodes allowed
            if (Binding.ModelItems.Any(i => !(i is XElement)))
                throw new Exception();

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

            // check for non homogeneous collection
            //var qn = new QName(firstNode.getNamespaceURI(), firstNode.getLocalName());
            //if (boundNodes.Any(i => i.getNamespaceURI() != qn.getNamespaceURI() || i.getLocalName() != qn.getLocalPart()))
            //    throw new Exception();

            var atAttr = Module.GetAttributeValue(Xml, "at");
            if (atAttr == null)
                throw new Exception();

            // evaluation context is either own context, or in-scope context
            var ec = Context ?? Binding.Context;

            var atD = (double?)Module.EvaluateXPath(this, ec, atAttr, XPathResultType.Number);

            if ((atD ?? double.NaN) == double.NaN)
            {
                // outside legal range, we insert after last node
                atD = Binding.ModelItems.Length;
            }
            else if (atD < 1)
                // out of range, start
                atD = 1;
            else if (atD > Binding.ModelItems.Length)
                // out of range, end
                atD = Binding.ModelItems.Length;

            int at = (int)atD;

            // current node
            var curNode = Binding.ModelItems[at - 1].Xml;

            //// set of dependent visuals
            //var dependentVisualsState = Module.Document.Root
            //    .Descendants(true)
            //    .OfType<NodeSetBindingElement>()
            //    .Where(i => i.Binding != null && i.Binding.ModelItems != null && i.Binding.ModelItems.Any(j => j.Xml.Parent == parent))
            //    .Select(i => new { Visual = i, BoundNodes = i.Binding.ModelItems })
            //    .ToList();

            // remove node from instance data
            if (curNode is XElement)
                ((XElement)curNode).Remove();
            else if (curNode is XAttribute)
                ((XAttribute)curNode).Remove();
            else
                throw new Exception();

            //foreach (var dependentVisualState in dependentVisualsState)
            //{
            //    // if visual is a repeat element, resposition it to the new index
            //    var repeatVisual = dependentVisualState.Visual as RepeatElement;
            //    if (repeatVisual != null)
            //    {
            //        // index of deleted item
            //        var deleteIndex = dependentVisualState.BoundNodes
            //            .Select((i, j) => new { Index = j + 1, ModelItem = i })
            //            .Where(i => i.ModelItem.Xml == curNode)
            //            .Select(i => (int?)i.Index)
            //            .FirstOrDefault();
            //        if (deleteIndex == null)
            //            continue;

            //        if (repeatVisual.Index < deleteIndex)
            //        {
            //            // index remains unchanged, deletion occured after index
            //            repeatVisual.Refresh();
            //            continue;
            //        }
            //        else if (repeatVisual.Index == deleteIndex)
            //            // index is equal to the one deleted, maintain position, but refresh; refresh will force index
            //            // to the proper position
            //            repeatVisual.Index = (int)deleteIndex;
            //        else if (repeatVisual.Index > deleteIndex)
            //            // index was after the position that was deleted; index moves down by one
            //            repeatVisual.Index = repeatVisual.Index - 1;
            //        else
            //            throw new Exception();

            //        // refresh repeat visual
            //        repeatVisual.Refresh();

            //        // reinitialize inner repeat visuals
            //        foreach (var innerRepeatVisual in repeatVisual.Descendants(false).OfType<RepeatElement>())
            //            innerRepeatVisual.Index = innerRepeatVisual.StartIndex;
            //    }
            //    else
            //        dependentVisualState.Visual.Refresh();
            //}

            // instruct model to complete deferred update
            ec.Model.State.RebuildFlag = true;
            ec.Model.State.RecalculateFlag = true;
            ec.Model.State.RevalidateFlag = true;
            ec.Model.State.RefreshFlag = true;

            ec.Instance.Element.Interface<INXEventTarget>().DispatchEvent(Events.Delete);
        }

    }

}
