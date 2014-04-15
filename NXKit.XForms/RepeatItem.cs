using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.Xml;

namespace NXKit.XForms
{

    /// <summary>
    /// Describes the implicit 'repeat item' group element.
    /// </summary>
    [Interface(typeof(RepeatItemPredicate))]
    [Remote]
    public class RepeatItem :
        ElementExtension,
        IEvaluationContextProvider
    {

        Lazy<RepeatItemState> state;

        [Export(typeof(IInterfacePredicate))]
        public class RepeatItemPredicate :
            InterfacePredicateBase
        {

            public override bool IsMatch(XObject obj, Type type)
            {
                return type == typeof(RepeatItem) && obj.Annotation<RepeatItemState>() != null;
            }

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public RepeatItem(XElement element)
            : base(element)
        {
            Contract.Requires<System.ArgumentNullException>(element != null);

            this.state = new Lazy<RepeatItemState>(() => element.Annotation<RepeatItemState>());
        }

        RepeatItemState State
        {
            get { return state.Value;}
        }

        [Remote]
        public bool IsRepeatItem
        {
            get { return true; }
        }

        [Remote]
        public int Index
        {
            get { return State.Index; }
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

    }

}
