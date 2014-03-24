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

        public const string ModelConstruct = "xforms-model-construct";
        public const string ModelConstructDone = "xforms-model-construct-done";
        public const string Ready = "xforms-ready";
        public const string ModelDestruct = "xforms-model-destruct";

        public const string Rebuild = "xforms-rebuild";
        public const string Recalculate = "xforms-recalculate";
        public const string Revalidate = "xforms-revalidate";
        public const string Refresh = "xforms-refresh";
        public const string Reset = "xforms-reset";
        public const string Previous = "xforms-previous";
        public const string Next = "xforms-next";
        public const string Focus = "xforms-focus";
        public const string Help = "xforms-help";
        public const string Hint = "xforms-hint";
        public const string Submit = "xforms-submit";
        public const string SubmitSerialize = "xforms-submit-serialize";

        public const string Insert = "xforms-insert";
        public const string Delete = "xforms-delete";
        public const string ValueChanged = "xforms-value-changed";
        public const string Valid = "xforms-valid";
        public const string Invalid = "xforms-invalid";
        public const string ReadOnly = "xforms-readonly";
        public const string ReadWrite = "xforms-readwrite";
        public const string Required = "xforms-required";
        public const string Optional = "xforms-optional";
        public const string Enabled = "xforms-enabled";
        public const string Disabled = "xforms-disabled";
        public const string Select = "xforms-select";
        public const string Deselect = "xforms-deselect";
        public const string InRange = "xforms-in-range";
        public const string OutOfRange = "xforms-out-of-range";
        public const string ScrollFirst = "xforms-scroll-first";
        public const string ScrollLast = "xforms-scroll-last";
        public const string SubmitDone = "xforms-submit-done";

        public const string BindingError = "xforms-binding-error";
        public const string ExpressionError = "xforms-expression-error";
        public const string ActionError = "xforms-action-error";
        public const string BindingException = "xforms-binding-exception";
        public const string ComputeException = "xforms-compute-exception";
        public const string VersionException = "xforms-version-exception";
        public const string LinkException = "xforms-link-exception";
        public const string OutputError = "xforms-output-error";
        public const string SubmitError = "xforms-submit-error";

    }

}
