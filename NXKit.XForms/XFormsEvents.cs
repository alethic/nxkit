using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

using NXKit.Util;

namespace NXKit.XForms
{

    /// <summary>
    /// XForms processing is defined in terms of events, event handlers, and event responses. XForms uses the events system defined in [DOM2 Events][XML Events], with an event capture phase, arrival of the event at its Target, and finally the event bubbling phase.
    /// </summary>
    public static class XFormsEvents
    {

        /// <summary>
        /// Describes the default properties of an event.
        /// </summary>
        public class EventInfo
        {

            readonly string type;
            readonly bool cancelable;
            readonly bool canBubble;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="type"></param>
            /// <param name="cancelable"></param>
            /// <param name="canBubble"></param>
            public EventInfo(string type, bool cancelable, bool canBubble)
            {
                Contract.Requires<ArgumentNullException>(type != null);

                this.type = type;
                this.cancelable = cancelable;
                this.canBubble = canBubble;
            }

            public string Type
            {
                get { return type; }
            }

            public bool Cancelable
            {
                get { return cancelable; }
            }

            public bool CanBubble
            {
                get { return canBubble; }
            }

        }

        /// <summary>
        /// Maps a series of <see cref="EventInfo"/> instances by their type.
        /// </summary>
        public class EventMap
        {

            readonly Dictionary<string, EventInfo> map;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="events"></param>
            public EventMap(IEnumerable<EventInfo> events)
            {
                Contract.Requires<ArgumentNullException>(events != null);

                this.map = events
                    .ToDictionary(i => i.Type, i => i);
            }

            /// <summary>
            /// Gets the event with the specified name.
            /// </summary>
            /// <param name="type"></param>
            /// <returns></returns>
            public EventInfo this[string type]
            {
                get { return map.GetOrDefault(type); }
            }

        }

        static readonly EventInfo[] initialization_events = new[]{
            new EventInfo(ModelConstruct, false, true),
            new EventInfo(ModelConstructDone, false, true),
            new EventInfo(Ready, false, true),
            new EventInfo(ModelDestruct, false, true),
        };

        static readonly EventInfo[] interaction_events = new[]{
            new EventInfo(Rebuild, true, true),
            new EventInfo(Recalculate, true, true),
            new EventInfo(Revalidate, true, true),
            new EventInfo(Refresh, true, true),
            new EventInfo(Reset, true, true),
            new EventInfo(Previous, true, false),
            new EventInfo(Next, true, false),
            new EventInfo(Focus, true, false),
            new EventInfo(Help, true, true),
            new EventInfo(Hint, true, true),
            new EventInfo(Submit, true, true),
            new EventInfo(SubmitSerialize, false, true),
        };

        static readonly EventInfo[] notification_events = new[]{
            new EventInfo(Insert, false, true),
            new EventInfo(Delete, false, true),
            new EventInfo(ValueChanged, false, true),

            new EventInfo(Valid, false, true),
            new EventInfo(Invalid, false, true),
            new EventInfo(ReadOnly, false, true),
            new EventInfo(ReadWrite, false, true),
            new EventInfo(Required, false, true),
            new EventInfo(Optional, false, true),
            new EventInfo(Enabled, false, true),
            new EventInfo(Disabled, false, true),

            new EventInfo(Select, false, true),
            new EventInfo(Deselect, false, true),
            new EventInfo(InRange, false, true),
            new EventInfo(OutOfRange, false, true),
            new EventInfo(ScrollFirst, false, true),
            new EventInfo(ScrollLast, false, true),
            new EventInfo(SubmitDone, false, true),
        };

        static readonly EventInfo[] error_indications = new[] {
            new EventInfo(BindingError, true, true),
            new EventInfo(ExpressionError, true, true),
            new EventInfo(ActionError, true, true),
            new EventInfo(BindingException, false, true),
            new EventInfo(ComputeException, false, true),
            new EventInfo(VersionException, false, true),
            new EventInfo(LinkException, false, true),
            new EventInfo(OutputError, false, true),
            new EventInfo(SubmitError, false, true),
        };

        /// <summary>
        /// Maps event types to <see cref="EventInfo"/>.
        /// </summary>
        public static readonly EventMap Map = new EventMap(Enumerable.Empty<EventInfo>()
            .Concat(initialization_events)
            .Concat(interaction_events)
            .Concat(notification_events)
            .Concat(error_indications));

        public static readonly string ModelConstruct = "xforms-model-construct";
        public static readonly string ModelConstructDone = "xforms-model-construct-done";
        public static readonly string Ready = "xforms-ready";
        public static readonly string ModelDestruct = "xforms-model-destruct";

        public static readonly string Rebuild = "xforms-rebuild";
        public static readonly string Recalculate = "xforms-recalculate";
        public static readonly string Revalidate = "xforms-revalidate";
        public static readonly string Refresh = "xforms-refresh";
        public static readonly string Reset = "xforms-reset";
        public static readonly string Previous = "xforms-previous";
        public static readonly string Next = "xforms-next";
        public static readonly string Focus = "xforms-focus";
        public static readonly string Help = "xforms-help";
        public static readonly string Hint = "xforms-hint";
        public static readonly string Submit = "xforms-submit";
        public static readonly string SubmitSerialize = "xforms-submit-serialize";

        public static readonly string Insert = "xforms-insert";
        public static readonly string Delete = "xforms-delete";
        public static readonly string ValueChanged = "xforms-value-changed";
        public static readonly string Valid = "xforms-valid";
        public static readonly string Invalid = "xforms-invalid";
        public static readonly string ReadOnly = "xforms-readonly";
        public static readonly string ReadWrite = "xforms-readwrite";
        public static readonly string Required = "xforms-required";
        public static readonly string Optional = "xforms-optional";
        public static readonly string Enabled = "xforms-enabled";
        public static readonly string Disabled = "xforms-disabled";
        public static readonly string Select = "xforms-select";
        public static readonly string Deselect = "xforms-deselect";
        public static readonly string InRange = "xforms-in-range";
        public static readonly string OutOfRange = "xforms-out-of-range";
        public static readonly string ScrollFirst = "xforms-scroll-first";
        public static readonly string ScrollLast = "xforms-scroll-last";
        public static readonly string SubmitDone = "xforms-submit-done";

        public static readonly string BindingError = "xforms-binding-error";
        public static readonly string ExpressionError = "xforms-expression-error";
        public static readonly string ActionError = "xforms-action-error";
        public static readonly string BindingException = "xforms-binding-exception";
        public static readonly string ComputeException = "xforms-compute-exception";
        public static readonly string VersionException = "xforms-version-exception";
        public static readonly string LinkException = "xforms-link-exception";
        public static readonly string OutputError = "xforms-output-error";
        public static readonly string SubmitError = "xforms-submit-error";

    }

}
