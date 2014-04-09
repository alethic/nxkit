using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;
using NXKit.IO;
using NXKit.Util;

namespace NXKit.XForms
{

    /// <summary>
    /// Serialable storage for an instance visual's state.
    /// </summary>
    public class InstanceState :
        IXmlSerializable
    {

        XElement model;
        XElement instance;
        XDocument document;

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
                    document.AddAnnotation(model.Interface<Model>());
                if (instance != null)
                    document.AddAnnotation(instance.Interface<Instance>());
            }

            this.document = document;
        }

        /// <summary>
        /// Configures the state for the specified model and instance.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="instance"></param>
        internal void Initialize(XElement model, XElement instance)
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
        internal void Initialize(XElement model, XElement instance, XDocument document)
        {
            Contract.Requires<ArgumentNullException>(model != null);
            Contract.Requires<ArgumentNullException>(instance != null);

            Initialize(model, instance);
            Initialize(document);
        }

        /// <summary>
        /// Obtains the model item properties for <paramref name="obj"/>.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        ModelItemState GetModelItem(XObject obj)
        {
            Contract.Requires<ArgumentNullException>(obj != null);

            return obj.AnnotationOrCreate<ModelItemState>();
        }

        /// <summary>
        /// Persists the model item properties of the given instance into a serializable value.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        Tuple<int, ModelItemState>[] SaveModelItems()
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
            var saved = new List<Tuple<int, ModelItemState>>(20);

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

                saved.Add(new Tuple<int, ModelItemState>(nodeItem.Index, modelItemProperty));
            }

            return saved.ToArray();
        }

        /// <summary>
        /// Associates
        /// </summary>
        /// <param name="state"></param>
        void LoadModelItems(Tuple<int, ModelItemState>[] state)
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


        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            if (Document != null)
            {
                writer.WriteStartElement("Document");
                writer.WriteNode(new XDocumentAnnotationReader(Document), true);
                writer.WriteEndElement();
            }
        }

    }

}
