using System;
using System.Diagnostics.Contracts;
using System.Linq;

namespace NXKit.XForms
{

    [NXElementInterface("{http://www.w3.org/2002/xforms}item")]
    public class Item :
        ISelectable
    {

        readonly NXElement element;
        readonly Lazy<ISelectableValue> value;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Item(NXElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.element = element;
            this.value = new Lazy<ISelectableValue>(() => element
                .Descendants()
                .OfType<NXElement>()
                .SelectMany(i => i.Interfaces<ISelectableValue>())
                .FirstOrDefault());
        }

        public string Id
        {
            get { return element.UniqueId; }
        }

        public void Select(UIBinding ui)
        {
            if (value.Value != null)
                value.Value.Select(ui);
        }

        public void Deselect(UIBinding ui)
        {
            if (value.Value != null)
                value.Value.Deselect(ui);
        }

        public bool IsSelected(UIBinding ui)
        {
            if (value.Value != null)
                return value.Value.IsSelected(ui);

            return false;
        }

        public int GetValueHashCode()
        {
            if (value.Value != null)
                return value.Value.GetHashCode();

            return 0;
        }

    }

}
