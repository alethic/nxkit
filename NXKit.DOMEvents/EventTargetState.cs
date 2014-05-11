using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

using NXKit.Serialization;

namespace NXKit.DOMEvents
{

    /// <summary>
    /// List of event listeners registered with a particular <see cref="NXNode"/>.
    /// </summary>
    [SerializableAnnotation]
    [XmlRoot("event-target")]
    public class EventTargetState :
        IXmlSerializable
    {

        readonly HashSet<EventTargetListenerItem> listeners;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public EventTargetState()
        {
            this.listeners = new HashSet<EventTargetListenerItem>();
        }

        /// <summary>
        /// Gets the registered listeners.
        /// </summary>
        public ICollection<EventTargetListenerItem> Listeners
        {
            get { return listeners; }
        }

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            if (reader.MoveToContent() == XmlNodeType.Element &&
                reader.LocalName == "event-target")
            {
                if (reader.ReadToDescendant("listeners"))
                {
                    // load listeners tree
                    var listenersXml = XElement.Load(
                        reader.ReadSubtree(),
                        LoadOptions.PreserveWhitespace | LoadOptions.SetBaseUri);

                    foreach (var itemXml in listenersXml.Elements("item"))
                    {
                        var listenerXml = itemXml.Element("listener");
                        if (listenerXml == null)
                            throw new InvalidOperationException();

                        var root = listenerXml.FirstNode;
                        if (root == null)
                            throw new InvalidOperationException();

                        var typeName = (string)listenerXml.Attribute("type");
                        if (string.IsNullOrWhiteSpace(typeName))
                            throw new InvalidOperationException();

                        var type = Type.GetType(typeName);
                        if (type == null)
                            throw new InvalidOperationException();

                        // deserialize listener
                        using (var rdr = root.CreateReader())
                        {
                            var listener = (IEventListener)new XmlSerializer(type).Deserialize(rdr);

                            // register listener with stored events
                            foreach (var registrationXml in itemXml.Elements("registration"))
                                Listeners.Add(new EventTargetListenerItem(
                                    (string)registrationXml.Attribute("event"),
                                    (bool)registrationXml.Attribute("use-capture"),
                                    listener));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns a <see cref="XElement"/> of a serialized <see cref="IEventListener"/> instance.
        /// </summary>
        /// <param name="listener"></param>
        /// <returns></returns>
        XElement SerializeListener(IEventListener listener)
        {
            // generate listener element
            var element = new XElement("listener",
                new XAttribute("type", listener.GetType().FullName + ", " + listener.GetType().Assembly.GetName().Name));

            // serialize listener into element
            using (var w = element.CreateWriter())
            {
                w.WriteWhitespace("");
                new XmlSerializer(listener.GetType())
                    .Serialize(w, listener);
            }

            return element;
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            new XElement("listeners",
                Listeners
                    .Where(i => i.Listener.GetType().IsPublic)
                    .Where(i => i.Listener.GetType().GetConstructor(new Type[0]) != null)
                    .GroupBy(i => i.Listener).Select(i => new XElement("item",
                        SerializeListener(i.Key),
                        i.Select(j => new XElement("registration",
                            new XAttribute("event", j.EventType),
                            new XAttribute("use-capture", j.UseCapture))))))
                .WriteTo(writer);
        }

    }

}
