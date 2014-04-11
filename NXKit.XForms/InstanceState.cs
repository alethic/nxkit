using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

using NXKit.Serialization;
using NXKit.Xml;

namespace NXKit.XForms
{

    /// <summary>
    /// Serialable storage for an instance visual's state.
    /// </summary>
    [XmlRoot("instance")]
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

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            if (reader.MoveToContent() == XmlNodeType.Element &&
                reader.LocalName == "instance")
            {
                if (reader.ReadToDescendant("xml"))
                    document = XNodeAnnotationSerializer.Deserialize(new XDocument(
                        XElement.Load(
                            reader.ReadSubtree(), 
                            LoadOptions.PreserveWhitespace | LoadOptions.SetBaseUri)
                        .Elements()
                        .FirstOrDefault()));
            }
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            if (document != null)
            {
                writer.WriteStartElement("xml");
                writer.WriteNode(XNodeAnnotationSerializer.Serialize(document).CreateReader(), true);
                writer.WriteEndElement();
            }
        }

    }

}
