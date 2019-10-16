using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using NXKit.Xml;

namespace NXKit.DOMEvents
{

    /// <summary>
    /// Relays mutation events from XLinq to DOM events.
    /// </summary>
    //[Interface(XmlNodeType.Document)]  // disabled because it's slow, need better solution, with deferred execution
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
            this.document = document ?? throw new ArgumentNullException(nameof(document));
        }

        /// <summary>
        /// Gets a reference to the extended object.
        /// </summary>
        public XObject Object => document;

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
            if (sender == null)
                throw new ArgumentNullException(nameof(sender));
            if (args == null)
                throw new ArgumentNullException(nameof(args));

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
            if (sender == null)
                throw new ArgumentNullException(nameof(sender));
            if (args == null)
                throw new ArgumentNullException(nameof(args));

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
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));
            if (eventType == null)
                throw new ArgumentNullException(nameof(eventType));

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
            if (node == null)
                throw new ArgumentNullException(nameof(node));
            if (eventType == null)
                throw new ArgumentNullException(nameof(eventType));

            node.Interface<EventTarget>().Dispatch(CreateEvent(node, eventType));
        }

        /// <summary>
        /// Raises the DOMNodeInserted event for the specified object.
        /// </summary>
        /// <param name="obj"></param>
        void OnAdded(XObject obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

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
            if (node == null)
                throw new ArgumentNullException(nameof(node));

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
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            DispatchEvent(node, Events.DOMNodeInsertedIntoDocument);
        }

        /// <summary>
        /// Raises the DOMNodeRemoved event for the specified object.
        /// </summary>
        /// <param name="obj"></param>
        void OnRemoving(XObject obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

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
            if (node == null)
                throw new ArgumentNullException(nameof(node));

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
            if (node == null)
                throw new ArgumentNullException(nameof(node));

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
