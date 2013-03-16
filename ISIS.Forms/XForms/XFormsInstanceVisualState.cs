using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;

using ISIS.Util;

namespace ISIS.Forms.XForms
{

    /// <summary>
    /// Serialable storage for an instance visual's state.
    /// </summary>
    [Serializable]
    public class XFormsInstanceVisualState : ISerializable
    {

        private Tuple<int, XFormsModelItemState>[] deserializedModelItemState;
        
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
            NextItemId = info.GetInt32("NextNodeId");
            InstanceDocument = FormProcessor.StringToXDocument(info.GetString("InstanceDocument"), null);
            InstanceElement = InstanceDocument.Root;
            deserializedModelItemState = (Tuple<int, XFormsModelItemState>[])info.GetValue("ModelItems", typeof(Tuple<int, XFormsModelItemState>[]));
        }

        /// <summary>
        /// Finishes deserialization of the model items.
        /// </summary>
        /// <param name="context"></param>
        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            LoadModelItems(deserializedModelItemState);
        }

        /// <summary>
        /// Records the last auto-assigned node id.
        /// </summary>
        public int NextItemId { get; set; }

        /// <summary>
        /// DOM of instance.
        /// </summary>
        public XDocument InstanceDocument { get; set; }

        /// <summary>
        /// Root node of the DOM of the instance.
        /// </summary>
        public XElement InstanceElement { get; set; }

        /// <summary>
        /// Serializes the instance.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("NextNodeId", NextItemId);
            info.AddValue("InstanceDocument", FormProcessor.XDocumentToString(InstanceDocument));
            info.AddValue("ModelItems", SaveModelItems());
        }

        /// <summary>
        /// Obtains the model item properties for <paramref name="obj"/>.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private XFormsModelItemState GetModelItem(XObject obj)
        {
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
        private Tuple<int,XFormsModelItemState>[] SaveModelItems()
        {
            // calculate index positions for instance data node
            var nodeItems = InstanceElement
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

                saved.Add(new Tuple<int,XFormsModelItemState>(nodeItem.Index, modelItemProperty));
            }

            return saved.ToArray();
        }

        /// <summary>
        /// Associates
        /// </summary>
        /// <param name="state"></param>
        private void LoadModelItems(Tuple<int,XFormsModelItemState>[] state)
        {
            // map incoming state by index
            var stateMap = state.ToDictionary(i => i.Item1, i => i.Item2);

            // calculate index positions for instance data node
            var nodeItems = InstanceElement
                .DescendantsAndSelf()
                .SelectMany(i => i.Attributes().Cast<XObject>().Prepend(i))
                .Where(i => i is XElement || i is XAttribute)
                .Select((i, j) => new { Index = j, Node = i });

            foreach (var nodeItem in nodeItems)
            {
                // find the incoming state for the item
                var itemState = stateMap.ValueOrDefault(nodeItem.Index);
                if (itemState == null)
                    continue;

                // associate state with node
                nodeItem.Node.AddAnnotation(itemState);
            }
        }

    }

}
