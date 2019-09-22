using System;
using System.Collections.Immutable;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;

using NXKit.Serialization;

namespace NXKit.DOMEvents
{

    /// <summary>
    /// List of event listeners registered with a particular <see cref="EventTarget"/>.
    /// </summary>
    [SerializableAnnotation]
    public class EventTargetState :
        ISerializableAnnotation
    {

        internal ImmutableHashSet<EventListenerRegistration> registrations;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public EventTargetState()
        {
            this.registrations = ImmutableHashSet<EventListenerRegistration>.Empty;
        }

        XElement ISerializableAnnotation.Serialize(AnnotationSerializer serializer)
        {
            var items = registrations
                .Where(i => i.Listener.GetType().IsPublic)
                .Where(i => i.Listener.GetType().GetConstructor(new Type[0]) != null)
                .ToList();

            if (items.Count > 0)
                return new XElement("listeners", items
                    .GroupBy(i => i.Listener)
                    .Select(i => new XElement("item",
                        SerializeListener(i.Key),
                        i.Select(j => new XElement("registration",
                            new XAttribute("event", j.EventType),
                            new XAttribute("capture", j.Capture))))));

            return null;
        }

        /// <summary>
        /// Returns a <see cref="XElement"/> of a serialized <see cref="IEventListener"/> instance.
        /// </summary>
        /// <param name="listener"></param>
        /// <returns></returns>
        XElement SerializeListener(IEventListener listener)
        {
            if (listener == null)
                throw new ArgumentNullException(nameof(listener));

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

        void ISerializableAnnotation.Deserialize(AnnotationSerializer serializer, XElement element)
        {
            var items = element
                .Elements("item");

            foreach (var itemXml in items)
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
                        registrations = registrations.Add(
                            new EventListenerRegistration(
                                (string)registrationXml.Attribute("event"),
                                listener,
                                (bool)registrationXml.Attribute("capture")));
                }
            }
        }

    }

}
