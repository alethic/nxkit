using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit.XForms
{

    /// <summary>
    /// Describes the implicit 'repeat item' group element.
    /// </summary>
    [Interface(typeof(RepeatItemPredicate))]
    [Remote]
    public class RepeatItem :
        ElementExtension
    {

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
        }

        [Remote]
        public bool IsRepeatItem
        {
            get { return true; }
        }

    }

}
