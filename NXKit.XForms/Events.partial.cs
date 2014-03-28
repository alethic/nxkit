
using NXKit.DOMEvents;

namespace NXKit.XForms
{

    public partial class Events
    {

        static readonly EventInfo[] EVENT_INFO = new EventInfo[]
        {
            new EventInfo("xforms-model-construct", true, false),
            new EventInfo("xforms-model-construct-done", true, false),
            new EventInfo("xforms-ready", true, false),
            new EventInfo("xforms-model-destruct", false, false),
            new EventInfo("xforms-rebuild", true, true),
            new EventInfo("xforms-recalculate", true, true),
            new EventInfo("xforms-revalidate", true, true),
            new EventInfo("xforms-refresh", true, true),
            new EventInfo("xforms-reset", true, true),
            new EventInfo("xforms-next", false, true),
            new EventInfo("xforms-previous", false, true),
            new EventInfo("xforms-focus", false, true),
            new EventInfo("xforms-help", true, true),
            new EventInfo("xforms-hint", true, true),
            new EventInfo("xforms-submit", true, true),
            new EventInfo("xforms-submit-serialize", true, true),
            new EventInfo("xforms-dialog-show", true, true),
            new EventInfo("xforms-dialog-hide", true, true),
            new EventInfo("xforms-insert", true, false),
            new EventInfo("xforms-delete", true, false),
            new EventInfo("xforms-value-changed", true, false),
            new EventInfo("xforms-valid", true, false),
            new EventInfo("xforms-invalid", true, false),
            new EventInfo("xforms-readonly", true, false),
            new EventInfo("xforms-readwrite", true, false),
            new EventInfo("xforms-required", true, false),
            new EventInfo("xforms-optional", true, false),
            new EventInfo("xforms-enabled", true, false),
            new EventInfo("xforms-disabled", true, false),
            new EventInfo("xforms-select", true, false),
            new EventInfo("xforms-deselect", true, false),
            new EventInfo("xforms-in-range", true, false),
            new EventInfo("xforms-out-of-range", true, false),
            new EventInfo("xforms-scroll-first", true, false),
            new EventInfo("xforms-scroll-last", true, false),
            new EventInfo("xforms-submit-done", true, false),
            new EventInfo("xforms-dialog-shown", true, false),
            new EventInfo("xforms-dialog-hidden", true, false),
            new EventInfo("xforms-binding-error", true, true),
            new EventInfo("xforms-expression-error", true, true),
            new EventInfo("xforms-action-error", true, true),
            new EventInfo("xforms-binding-exception", true, false),
            new EventInfo("xforms-compute-exception", true, false),
            new EventInfo("xforms-version-exception", true, false),
            new EventInfo("xforms-link-exception", true, false),
            new EventInfo("xforms-output-error", true, false),
            new EventInfo("xforms-submit-error", true, false),
        };

        public const string ModelConstruct = "xforms-model-construct";
        public const string ModelConstructDone = "xforms-model-construct-done";
        public const string Ready = "xforms-ready";
        public const string ModelDestruct = "xforms-model-destruct";
        public const string Rebuild = "xforms-rebuild";
        public const string Recalculate = "xforms-recalculate";
        public const string Revalidate = "xforms-revalidate";
        public const string Refresh = "xforms-refresh";
        public const string Reset = "xforms-reset";
        public const string Next = "xforms-next";
        public const string Previous = "xforms-previous";
        public const string Focus = "xforms-focus";
        public const string Help = "xforms-help";
        public const string Hint = "xforms-hint";
        public const string Submit = "xforms-submit";
        public const string SubmitSerialize = "xforms-submit-serialize";
        public const string DialogShow = "xforms-dialog-show";
        public const string DialogHide = "xforms-dialog-hide";
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
        public const string DialogShown = "xforms-dialog-shown";
        public const string DialogHidden = "xforms-dialog-hidden";
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