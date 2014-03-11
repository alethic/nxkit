using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;

using NXKit.Util;

namespace NXKit.XForms
{

    /// <summary>
    /// Serialable storage for an instance visual's state.
    /// </summary>
    [Serializable]
    public class XFormsInstanceVisualState :
        ISerializable
    {

        XFormsModelVisual model;
        XFormsInstanceVisual instance;

        int nextItemId;
        XDocument document;
        readonly Tuple<int, XFormsModelItemState>[] deserializedModelItemState;

        [ContractInvariantMethod]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
        void ObjectInvariant()
        {
            Contract.Invariant(nextItemId >= 0);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public XFormsInstanceVisualState()
        {

        }

        /// <summary>
        /// Deserializes an instance.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public XFormsInstanceVisualState(SerializationInfo info, StreamingContext context)
        {
            Contract.Requires<ArgumentNullException>(info != null);

            this.nextItemId = info.GetInt32("NextNodeId");
            this.document = info.GetString("Document") != null ? XDocument.Parse(info.GetString("Document")) : null;
            this.deserializedModelItemState = (Tuple<int, XFormsModelItemState>[])info.GetValue("ModelItems", typeof(Tuple<int, XFormsModelItemState>[]));
        }

        /// <summary>
        /// Finishes deserialization of the model items.
        /// </summary>
        /// <param name="context"></param>
        [OnDeserialized]
        void OnDeserialized(StreamingContext context)
        {
            LoadModelItems(deserializedModelItemState);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int AllocateItemId()
        {
            return ++nextItemId;
        }

        /// <summary>
        /// DOM of instance.
        /// </summary>
        public XDocument Document
        {
            get { return document; }
        }

        /// <summary>
        /// Configures the instance document.
        /// </summary>
        void Initialize(XDocument document)
        {
            if (document != null)
            {
                if (model != null)
                    document.AddAnnotation(model);
                if (instance != null)
                    document.AddAnnotation(instance);
            }

            this.document = document;
        }

        /// <summary>
        /// Configures the state for the specified model and instance.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="instance"></param>
        internal void Initialize(XFormsModelVisual model, XFormsInstanceVisual instance)
        {
            Contract.Requires<ArgumentNullException>(model != null);
            Contract.Requires<ArgumentNullException>(instance != null);

            this.model = model;
            this.instance = instance;

            Initialize(document);
        }

        /// <summary>
        /// Configures the state for the specified model and instance.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="instance"></param>
        internal void Initialize(XFormsModelVisual model, XFormsInstanceVisual instance, XDocument document)
        {
            Contract.Requires<ArgumentNullException>(model != null);
            Contract.Requires<ArgumentNullException>(instance != null);

            Initialize(model, instance);
            Initialize(document);
        }

        /// <summary>
        /// Serializes the instance.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("NextNodeId", nextItemId);
            info.AddValue("Document", document != null ? document.ToString(SaveOptions.DisableFormatting) : null);
            info.AddValue("ModelItems", SaveModelItems());
        }

        /// <summary>
        /// Obtains the model item properties for <paramref name="obj"/>.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        XFormsModelItemState GetModelItem(XObject obj)
        {
            Contract.Requires<ArgumentNullException>(obj != null);

            var modelItem = obj.Annotation<XFormsModelItemState>();
            if (modelItem == null)
                obj.AddAnnotation(modelItem = new XFormsModelItemState());

            return modelItem;
        }

        /// <summary>
        /// Persists the model item properties of the given instance into a serializable value.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        Tuple<int, XFormsModelItemState>[] SaveModelItems()
        {
            if (document == null)
                return null;

            // calculate index positions for instance data node
            var nodeItems = document.Root
                .DescendantsAndSelf()
                .SelectMany(i => i.Attributes().Cast<XObject>().Prepend(i))
                .Where(i => i is XElement || i is XAttribute)
                .Select((i, j) => new { Index = j, Node = i });

            // serializable list
            var saved = new List<Tuple<int, XFormsModelItemState>>(20);

            foreach (var nodeItem in nodeItems)
            {
                // look up model item for node
                var modelItemProperty = GetModelItem(nodeItem.Node);
                if (modelItemProperty == null)
                    continue;

                // skip empty model items
                if (modelItemProperty.Id == null &&
                    modelItemProperty.Type == null &&
                    modelItemProperty.ReadOnly == null &&
                    modelItemProperty.Required == null &&
                    modelItemProperty.Relevant == null &&
                    modelItemProperty.Valid == null)
                    continue;

                saved.Add(new Tuple<int, XFormsModelItemState>(nodeItem.Index, modelItemProperty));
            }

            return saved.ToArray();
        }

        /// <summary>
        /// Associates
        /// </summary>
        /// <param name="state"></param>
        void LoadModelItems(Tuple<int, XFormsModelItemState>[] state)
        {
            Contract.Requires<ArgumentNullException>(state != null);

            // map incoming state by index
            var stateMap = state.ToDictionary(i => i.Item1, i => i.Item2);

            // calculate index positions for instance data node
            var nodeItems = Document.Root
                .DescendantsAndSelf()
                .SelectMany(i => i.Attributes().Cast<XObject>().Prepend(i))
                .Where(i => i is XElement || i is XAttribute)
                .Select((i, j) => new { Index = j, Node = i });

            foreach (var nodeItem in nodeItems)
            {
                // find the incoming state for the item
                var itemState = stateMap.GetOrDefault(nodeItem.Index);
                if (itemState == null)
                    continue;

                // associate state with node
                nodeItem.Node.AddAnnotation(itemState);
            }
        }

    }

}
