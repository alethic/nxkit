using System;
using System.Diagnostics.Contracts;
using System.Xml;
using System.Xml.Linq;

namespace NXKit.DOMEvents
{

    /// <summary>
    /// Relays mutation events from XLinq to DOM events.
    /// </summary>
    [Interface(XmlNodeType.Document)]
    public class DocumentEventDispatcher :
        IOnInitialize
    {

        readonly XDocument document;

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
        /// Ensures we're initialized at the start.
        /// </summary>
        public void Init()
        {
            this.document.Changing += document_Changing;
            this.document.Changed += document_Changed;
        }

        void document_Changing(object sender, XObjectChangeEventArgs args)
        {
            switch (args.ObjectChange)
            {
                case XObjectChange.Add:
                    break;
                case XObjectChange.Remove:
                    OnRemoving((XObject)sender);
                    break;
                case XObjectChange.Value:
                    break;
                case XObjectChange.Name:
                    break;
            }

            OnSubtreeModified((XObject)sender);
        }

        void document_Changed(object sender, XObjectChangeEventArgs args)
        {
            switch (args.ObjectChange)
            {
                case XObjectChange.Add:
                    OnAdded((XObject)sender);
                    break;
                case XObjectChange.Remove:
                    break;
                case XObjectChange.Value:
                    break;
                case XObjectChange.Name:
                    break;
            }

            OnSubtreeModified((XObject)sender);
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
            Contract.Requires<ArgumentNullException>(eventType != null);

            var events = obj.Document.Interface<INXDocumentEvent>();
            var event_ = events.CreateEvent<MutationEvent>("MutationEvent");
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
        /// Raises the DOMSubtreeModifed event for the specified object.
        /// </summary>
        /// <param name="obj"></param>
        void OnSubtreeModified(XObject obj)
        {
            Contract.Requires<ArgumentNullException>(obj != null);

            var node = obj as XNode;
            if (node != null)
                OnSubtreeModified(node);

            var attr = obj as XAttribute;
            if (attr != null)
                OnSubtreeModified(attr.Parent);
        }
        /// <summary>
        /// Raises the DOMSubtreeModifed event for the specified node.
        /// </summary>
        /// <param name="obj"></param>

        void OnSubtreeModified(XNode node)
        {
            Contract.Requires<ArgumentNullException>(node != null);

            DispatchEvent(node, Events.DOMSubtreeModified);
        }

    }

}
