using System;
using System.Xml.Linq;
using System.Xml.Serialization;

using NUnit.Framework;

using NXKit.Testing;

namespace NXKit.DOMEvents.Tests
{

    public class EventTargetStateTests : NXKitTestFixture
    {

        public EventTargetStateTests(NXKitTestFixtureContext context) :
            base(context)
        {

        }

        [XmlRoot("test-event-listener")]
        public class TestEventListener :
            IEventListener
        {

            [XmlAttribute("value")]
            public string Value { get; set; }

            public void HandleEvent(Event evt)
            {
                throw new NotImplementedException();
            }

        }

        XDocument Serialize()
        {
            var x = new XDocument();
            var e = new XElement("root");
            x.Add(e);

            var l1 = new TestEventListener();
            var l2 = new TestEventListener();

            using (var w = e.CreateWriter())
            {
                w.WriteWhitespace("");
                var s = new EventTargetState();
                s.registrations.Add(new EventListenerRegistration("event1", l1, false));
                s.registrations.Add(new EventListenerRegistration("event2", l1, true));
                s.registrations.Add(new EventListenerRegistration("event1", l2, false));
                s.registrations.Add(new EventListenerRegistration("event2", l2, true));
                new XmlSerializer(s.GetType()).Serialize(w, s);
            }

            return x;
        }

        [Test]
        public void Test_serialize()
        {
            Serialize();
        }

        [Test]
        public void Test_deserialize()
        {
            var x = Serialize();
            var s = (EventTargetState)new XmlSerializer(typeof(EventTargetState)).Deserialize(x.Root.FirstNode.CreateReader());
        }

    }

}
