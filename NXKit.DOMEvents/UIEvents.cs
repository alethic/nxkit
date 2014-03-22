namespace NXKit.DOMEvents
{

    /// <summary>
    /// The UIEvent interface provides specific contextual information associated with User Interface events.
    /// </summary>
    public static class UIEvents
    {

        /// <summary>
        /// The activate event occurs when an element is activated, for instance, thru a mouse click or a keypress. A numerical argument is provided to give an indication of the type of activation that occurs: 1 for a simple activation (e.g. a simple click or Enter), 2 for hyperactivation (for instance a double click or Shift Enter).
        /// </summary>
        public static readonly string DOMActivate = "DOMActivate";

        /// <summary>
        /// The DOMFocusIn event occurs when an EventTarget receives focus, for instance via a pointing device being moved onto an element or by tabbing navigation to the element. Unlike the HTML event focus, DOMFocusIn can be applied to any focusable EventTarget, not just FORM controls.
        /// </summary>
        public static readonly string DOMFocusIn = "DOMFocusIn";

        /// <summary>
        /// The DOMFocusOut event occurs when a EventTarget loses focus, for instance via a pointing device being moved out of an element or by tabbing navigation out of the element. Unlike the HTML event blur, DOMFocusOut can be applied to any focusable EventTarget, not just FORM controls.
        /// </summary>
        public static readonly string DOMFocusOut = "DOMFocusOut";

    }

}
