using System;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

using NXKit.Xml;

namespace NXKit.DOMEvents
{

    /// <summary>
    /// Provides a <see cref="IEventListener"/> implementation that will be preserved between loads, and dispatch
    /// events to the specified <see cref="MethodInfo"/> on the specified interface, of the given <see
    /// cref="XObject"/>.
    /// </summary>
    [XmlRoot("interface-event-listener")]
    public class InterfaceEventListener :
        IEventListener,
        IXmlSerializable
    {

        /// <summary>
        /// Tests whether the given <see cref="MethodInfo"/> is a valid handler.
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        public static bool IsValidMethodInfo(MethodInfo methodInfo)
        {
            if (methodInfo == null)
                throw new ArgumentNullException(nameof(methodInfo));

            // method must be public
            if (!methodInfo.IsPublic)
                return false;

            // method must be an instance method
            if (methodInfo.IsStatic)
                return false;

            // method must be an implementation
            if (methodInfo.IsAbstract)
                return false;

            // method must be an implementation
            if (methodInfo.IsGenericMethodDefinition)
                return false;

            // zero parameters are supported
            var p = methodInfo.GetParameters();
            if (p.Length == 0)
                return true;

            // first parameter can be an Event
            if (typeof(Event).IsAssignableFrom(p[0].ParameterType))
                return true;

            return false;
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="interfaceType"></param>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        public static InterfaceEventListener Create(XObject handler, Type interfaceType, MethodInfo methodInfo)
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));
            if (interfaceType == null)
                throw new ArgumentNullException(nameof(interfaceType));
            if (methodInfo == null)
                throw new ArgumentNullException(nameof(methodInfo));
            if (IsValidMethodInfo(methodInfo) == false)
                throw new ArgumentException("", nameof(methodInfo));

            return new InterfaceEventListener(handler, interfaceType, methodInfo);
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static InterfaceEventListener Create(Action action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            if (action.Target == null)
                throw new ArgumentException("Action must have target.");
            if (action.Method == null)
                throw new ArgumentException("Action must have method.");
            if (action.Target is ElementExtension == false)
                throw new ArgumentException("Action target must be an ElementExtension.");
            if (IsValidMethodInfo(action.Method) == false)
                throw new ArgumentException("", nameof(action.Method));

            var target = action.Target;
            var method = action.Method;

            return Create(((ElementExtension)target).Element, target.GetType(), method);
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static InterfaceEventListener Create(Action<Event> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            if (action.Target == null)
                throw new ArgumentException("Action must have target.");
            if (action.Method == null)
                throw new ArgumentException("Action must have method.");
            if (action.Target is ElementExtension == false)
                throw new ArgumentException("Action target must be an ElementExtension.");
            if (IsValidMethodInfo(action.Method) == false)
                throw new ArgumentException("", nameof(action.Method));

            var target = action.Target;
            var method = action.Method;

            return Create(((ElementExtension)target).Element, target.GetType(), method);
        }

        /// <summary>
        /// Gets the existing <see cref="InterfaceEventListener"/> registered on the target.
        /// </summary>
        /// <param name="dispatcher"></param>
        /// <param name="eventType"></param>
        /// <param name="capture"></param>
        /// <param name="handler"></param>
        /// <param name="interfaceType"></param>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        public static InterfaceEventListener GetListener(EventTarget dispatcher, string eventType, bool capture, XObject handler, Type interfaceType, MethodInfo methodInfo)
        {
            if (dispatcher == null)
                throw new ArgumentNullException(nameof(dispatcher));
            if (string.IsNullOrWhiteSpace(eventType))
                throw new ArgumentOutOfRangeException(nameof(eventType));
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));
            if (interfaceType == null)
                throw new ArgumentNullException(nameof(interfaceType));
            if (methodInfo == null)
                throw new ArgumentNullException(nameof(methodInfo));
            if (IsValidMethodInfo(methodInfo) == false)
                throw new ArgumentException("", nameof(methodInfo));

            var host = handler.GetContext().Resolve<DocumentEnvironment>().GetHost();
            if (host == null)
                throw new InvalidOperationException();

            // find existing listener
            var listener = Enumerable.Empty<EventListenerRegistration>()
                .Concat(dispatcher.GetRegistrations())
                .OfType<InterfaceEventListener>()
                .Where(i => i.GetHandler(host) == handler)
                .Where(i => i.InterfaceType == interfaceType)
                .FirstOrDefault();

            return listener;
        }

        /// <summary>
        /// Gets the existing <see cref="InterfaceEventListener"/> for the given <see cref="Action"/>.
        /// </summary>
        /// <param name="dispatcher"></param>
        /// <param name="eventType"></param>
        /// <param name="capture"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static InterfaceEventListener GetListener(EventTarget dispatcher, string eventType, bool capture, Action action)
        {
            if (dispatcher == null)
                throw new ArgumentNullException(nameof(dispatcher));
            if (string.IsNullOrWhiteSpace(eventType))
                throw new ArgumentOutOfRangeException( nameof(eventType));
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            if (action.Target == null)
                throw new ArgumentException("Action must have target.");
            if (action.Method == null)
                throw new ArgumentException("Action must have method.");
            if (action.Target is ElementExtension == false)
                throw new ArgumentException("Action target must be an ElementExtension.");
            if (IsValidMethodInfo(action.Method) == false)
                throw new ArgumentException("", nameof(action.Method));

            var handler = action.Target;
            var method = action.Method;

            return GetListener(dispatcher, eventType, capture, ((ElementExtension)handler).Element, handler.GetType(), method);
        }

        /// <summary>
        /// Gets the existing <see cref="InterfaceEventListener"/> for the given <see cref="Action"/>.
        /// </summary>
        /// <param name="dispatcher"></param>
        /// <param name="eventType"></param>
        /// <param name="capture"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static InterfaceEventListener GetListener(EventTarget dispatcher, string eventType, bool capture, Action<Event> action)
        {
            if (dispatcher == null)
                throw new ArgumentNullException(nameof(dispatcher));
            if (string.IsNullOrWhiteSpace(eventType))
                throw new ArgumentOutOfRangeException(nameof(eventType));
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            if (action.Target == null)
                throw new ArgumentException("Action must have target.");
            if (action.Method == null)
                throw new ArgumentException("Action must have method.");
            if (action.Target is ElementExtension == false)
                throw new ArgumentException("Action target must be an ElementExtension.");
            if (IsValidMethodInfo(action.Method) == false)
                throw new ArgumentException("", nameof(action.Method));

            var handler = action.Target;
            var method = action.Method;

            return GetListener(dispatcher, eventType, capture, ((ElementExtension)handler).Element, handler.GetType(), method);
        }

        private int handlerId;
        private Type interfaceType;
        private MethodInfo methodInfo;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public InterfaceEventListener()
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="interfaceType"></param>
        /// <param name="methodInfo"></param>
        private InterfaceEventListener(XObject handler, Type interfaceType, MethodInfo methodInfo)
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));
            if (IsValidMethodInfo(methodInfo) == false)
                throw new ArgumentException("", nameof(methodInfo));

            this.handlerId = handler.GetObjectId();
            this.interfaceType = interfaceType ?? throw new ArgumentNullException(nameof(interfaceType));
            this.methodInfo = methodInfo ?? throw new ArgumentNullException(nameof(methodInfo));
        }

        /// <summary>
        /// Gets the <see cref="XObject"/> of the interface type that handles the event.
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public XObject GetHandler(Document host)
        {
            if (host == null)
                throw new ArgumentNullException(nameof(host));

            return host.Xml.ResolveObjectId(handlerId);
        }

        /// <summary>
        /// Gets the interface type that handles the event.
        /// </summary>
        public Type InterfaceType => interfaceType;

        /// <summary>
        /// Gets the <see cref="MethodInfo"/> that handles the event.
        /// </summary>
        public MethodInfo MethodInfo => methodInfo;

        public void HandleEvent(Event evt)
        {
            // resolve target object
            var handler = evt.Host.Xml.ResolveObjectId(handlerId);
            if (handler == null)
                return;

            // resolve each interface
            foreach (var h in handler.Interfaces(interfaceType))
            {
                if (methodInfo.DeclaringType.IsInstanceOfType(h) &&
                    methodInfo.GetParameters().Length == 0)
                {
                    methodInfo.Invoke(h, new object[0]);
                    continue;
                }

                if (methodInfo.DeclaringType.IsInstanceOfType(h) &&
                    methodInfo.GetParameters().Length == 1)
                {
                    methodInfo.Invoke(h, new object[] { evt });
                    continue;
                }
            }
        }

        public override bool Equals(object obj)
        {
            var other = obj as InterfaceEventListener;
            if (other == null)
                return false;

            return
                object.Equals(handlerId, other.handlerId) &&
                object.Equals(interfaceType, other.interfaceType) &&
                object.Equals(methodInfo, other.methodInfo);
        }

        public override int GetHashCode()
        {
            return
                handlerId.GetHashCode() ^
                interfaceType.GetHashCode() ^
                methodInfo.GetHashCode();
        }

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            if (reader.MoveToContent() == XmlNodeType.Element &&
                reader.LocalName == "interface-event-listener")
            {
                handlerId = int.Parse(reader.GetAttribute("handler-id"));

                var interfaceName = reader.GetAttribute("interface");
                if (string.IsNullOrWhiteSpace(interfaceName))
                    throw new InvalidOperationException();

                interfaceType = Type.GetType(interfaceName);
                if (interfaceType == null)
                    throw new InvalidOperationException();

                var methodTypeName = reader.GetAttribute("method-type") ?? interfaceName;
                if (string.IsNullOrWhiteSpace(methodTypeName))
                    throw new InvalidOperationException();

                var methodType = Type.GetType(methodTypeName);
                if (methodType == null)
                    throw new InvalidOperationException();

                var methodName = reader.GetAttribute("method");
                if (string.IsNullOrWhiteSpace(methodName))
                    throw new InvalidOperationException();

                methodInfo = methodType.GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance);
                if (methodInfo == null)
                    throw new InvalidOperationException();
            }
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("handler-id", handlerId.ToString());
            writer.WriteAttributeString("interface", interfaceType.FullName + ", " + interfaceType.Assembly.GetName().Name);
            if (methodInfo.DeclaringType != interfaceType)
                writer.WriteAttributeString("method-type", methodInfo.DeclaringType.FullName + ", " + methodInfo.DeclaringType.Assembly.GetName().Name);
            writer.WriteAttributeString("method", methodInfo.Name);
        }

    }

}
