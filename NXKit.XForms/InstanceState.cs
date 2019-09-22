using System;
using System.Xml.Linq;
using System.Xml.Serialization;

using NXKit.Serialization;
using NXKit.Xml;

namespace NXKit.XForms
{

    /// <summary>
    /// Serialable storage for an instance visual's state.
    /// </summary>
    [SerializableAnnotation]
    [XmlRoot("instance")]
    public class InstanceState :
        ISerializableAnnotation
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
            this.model = model ?? throw new ArgumentNullException(nameof(model));
            this.instance = instance ?? throw new ArgumentNullException(nameof(instance));

            Initialize(document);
        }

        /// <summary>
        /// Configures the state for the specified model and instance.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="instance"></param>
        internal void Initialize(XElement model, XElement instance, XDocument document)
        {
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
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            return obj.AnnotationOrCreate<ModelItemState>();
        }

        XElement ISerializableAnnotation.Serialize(AnnotationSerializer serializer)
        {
            return new XElement("instance",
                new XElement("xml",
                    serializer.Serialize(document).Root));
        }

        void ISerializableAnnotation.Deserialize(AnnotationSerializer serializer, XElement element)
        {
            document = serializer.Deserialize(
                new XDocument(
                    element.Element("xml").Elements()));
        }

    }

}
