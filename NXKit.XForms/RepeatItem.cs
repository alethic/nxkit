using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.DOMEvents;
using NXKit.Xml;

namespace NXKit.XForms
{

    /// <summary>
    /// Describes the implicit 'repeat item' group element.
    /// </summary>
    [Interface(typeof(RepeatItemPredicate), PredicateType = typeof(RepeatItemPredicate))]
    [Remote]
    public class RepeatItem :
        ElementExtension,
        IEvaluationContextProvider,
        IEventListener
    {

        public class RepeatItemPredicate :
            ExtensionPredicateBase
        {

            public override bool IsMatch(XObject obj, Type type)
            {
                return type == typeof(RepeatItem) && obj.Annotation<RepeatItemState>() != null;
            }

        }

        readonly RepeatItemState state;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public RepeatItem(XElement element)
            : base(element)
        {
            Contract.Requires<System.ArgumentNullException>(element != null);

            this.state = element.AnnotationOrCreate<RepeatItemState>();
        }

        [Remote]
        public bool IsRepeatItem
        {
            get { return true; }
        }

        [Remote]
        public int Index
        {
            get { return state.Index; }
        }

        /// <summary>
        /// Sets the repeat index to this item.
        /// </summary>
        public void SetIndex()
        {
            var repeat = Element.Parent;
            if (repeat == null ||
                repeat.Name != Constants.XForms_1_0 + "repeat")
                throw new InvalidOperationException();

            // set the repeat index to ourselves
            repeat.Interface<Repeat>().Index = Index;
        }

        EvaluationContext GetContext()
        {
            var repeat = Element.Parent;
            if (repeat == null ||
                repeat.Name != Constants.XForms_1_0 + "repeat")
                throw new InvalidOperationException();

            // get the item context for this specific item
            return repeat.Interface<Repeat>().GetItemContext(Element);
        }

        EvaluationContext IEvaluationContextProvider.Context
        {
            get { return GetContext(); }
        }

        void IEventListener.HandleEvent(Event evt)
        {
            if (evt.Type == DOMEvents.Events.DOMFocusIn)
                SetIndex();
        }

    }

}
