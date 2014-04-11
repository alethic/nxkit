using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Xml;
using System.Xml.Linq;
using System.Linq;

using NXKit.Xml;

namespace NXKit.DOMEvents
{

    /// <summary>
    /// Relays mutation events from XLinq to DOM events.
    /// </summary>
    [Interface(XmlNodeType.Document)]
    public class DocumentEventDispatcher :
        IOnLoad,
        IOnInvoke
    {

        readonly XDocument document;
        readonly LinkedList<XNode> subtreeDispatchList = new LinkedList<XNode>();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="document"></param>
        public DocumentEventDispatcher(XDocument document)
        {
            Contract.Requires<ArgumentNullException>(document != null);

            this.document = document;
        }

        /// <summary>
        /// Attach to events at document load.
        /// </summary>
        public void Load()
        {
            this.document.Changing += document_Changing;
            this.document.Changed += document_Changed;
        }

        void document_Changing(object sender, XObjectChangeEventArgs args)
        {
            Contract.Requires<ArgumentNullException>(sender != null);
            Contract.Requires<ArgumentNullException>(args != null);

            // store parent for subtree dispatch
            var obj = (XObject)sender;
            var par = (XNode)obj.Parent ?? (XNode)obj.Document;
            if (par != null)
                subtreeDispatchList.AddLast(par);

            switch (args.ObjectChange)
            {
                case XObjectChange.Add:
                    break;
                case XObjectChange.Remove:
                    OnRemoving(obj);
                    break;
                case XObjectChange.Value:
                    break;
                case XObjectChange.Name:
                    break;
            }
        }

        void document_Changed(object sender, XObjectChangeEventArgs args)
        {
            Contract.Requires<ArgumentNullException>(sender != null);
            Contract.Requires<ArgumentNullException>(args != null);

            // store parent for subtree dispatch
            var obj = (XObject)sender;
            var par = (XNode)obj.Parent ?? (XNode)obj.Document;
            if (par != null)
                subtreeDispatchList.AddLast(par);

            switch (args.ObjectChange)
            {
                case XObjectChange.Add:
                    OnAdded(obj);
                    break;
                case XObjectChange.Remove:
                    break;
                case XObjectChange.Value:
                    break;
                case XObjectChange.Name:
                    break;
            }
        }

        /// <summary>
        /// Creates a new mutation event for the given node.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="eventType"></param>
        /// <returns></returns>
        MutationEvent CreateEvent(XObject obj, string eventType)
        {
            Contract.Requires<ArgumentNullException>(obj != null);
            Contract.Requires<ArgumentNullException>(obj.Document != null);
            Contract.Requires<ArgumentNullException>(eventType != null);

            var events = obj.Document.Interface<INXDocumentEvent>();
            var event_ = events.CreateEvent<MutationEvent>(eventType);
            event_.InitMutationEvent(eventType, event_.Bubbles, event_.Cancelable);
            return event_;
        }

        /// <summary>
        /// Dispatches the given event against the given node.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="eventType"></param>
        void DispatchEvent(XNode node, string eventType)
        {
            Contract.Requires<ArgumentNullException>(node != null);
            Contract.Requires<ArgumentNullException>(node.Document != null);
            Contract.Requires<ArgumentNullException>(eventType != null);

            node.Interface<IEventTarget>().DispatchEvent(CreateEvent(node, eventType));
        }

        /// <summary>
        /// Raises the DOMNodeInserted event for the specified object.
        /// </summary>
        /// <param name="obj"></param>
        void OnAdded(XObject obj)
        {
            Contract.Requires<ArgumentNullException>(obj != null);

            var node = obj as XNode;
            if (node != null)
                OnAdded(node);
        }

        /// <summary>
        /// Raises the DOMNodeInserted event for the specified node.
        /// </summary>
        /// <param name="node"></param>
        void OnAdded(XNode node)
        {
            Contract.Requires<ArgumentNullException>(node != null);

            DispatchEvent(node, Events.DOMNodeInserted);

            var element = node as XElement;
            if (element != null)
                foreach (var node_ in element.DescendantNodesAndSelf())
                    OnNodeAddedToDocument(node_);
        }

        /// <summary>
        /// Raises the DOMNodeInsertedIntoDocument event for the given node.
        /// </summary>
        /// <param name="node"></param>
        void OnNodeAddedToDocument(XNode node)
        {
            Contract.Requires<ArgumentNullException>(node != null);

            DispatchEvent(node, Events.DOMNodeInsertedIntoDocument);
        }

        /// <summary>
        /// Raises the DOMNodeRemoved event for the specified object.
        /// </summary>
        /// <param name="obj"></param>
        void OnRemoving(XObject obj)
        {
            Contract.Requires<ArgumentNullException>(obj != null);

            var node = obj as XNode;
            if (node != null)
                OnRemoving(node);
        }

        /// <summary>
        /// Raises the DOMNodeRemoved event for the specified node.
        /// </summary>
        /// <param name="node"></param>
        void OnRemoving(XNode node)
        {
            Contract.Requires<ArgumentNullException>(node != null);

            DispatchEvent(node, Events.DOMNodeRemoved);

            var element = node as XElement;
            if (element != null)
                foreach (var node_ in element.DescendantNodesAndSelf())
                    OnNodeRemovedFromDocument(node_);
        }

        /// <summary>
        /// Raises the DOMNodeRemovedFromDocument event for the specified node.
        /// </summary>
        /// <param name="node"></param>
        void OnNodeRemovedFromDocument(XNode node)
        {
            Contract.Requires<ArgumentNullException>(node != null);

            DispatchEvent(node, Events.DOMNodeRemovedFromDocument);
        }

        /// <summary>
        /// Raises the DOMSubtreeModified event.
        /// </summary>
        bool OnSubtreeModified()
        {
            if (subtreeDispatchList.Count == 0)
                return false;

            // copy list
            var nodes = subtreeDispatchList
                .Distinct()
                .ToList();

            // clear before dispatching events (events might add more)
            subtreeDispatchList.Clear();

            // dispatch event to nodes
            foreach (var node in nodes)
                DispatchEvent(node, Events.DOMSubtreeModified);

            // indicate we did work
            return nodes.Count > 0;
        }

        bool IOnInvoke.Invoke()
        {
            return OnSubtreeModified();
        }

    }

}
