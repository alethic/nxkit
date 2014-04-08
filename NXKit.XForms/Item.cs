using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;

namespace NXKit.XForms
{

    [NXElementInterface("{http://www.w3.org/2002/xforms}item")]
    public class Item :
        ISelectable
    {

        readonly XElement element;
        readonly Lazy<ISelectableValue> value;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Item(XElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.element = element;
            this.value = new Lazy<ISelectableValue>(() => element
                .Descendants()
                .SelectMany(i => i.Interfaces<ISelectableValue>())
                .FirstOrDefault());
        }

        /// <summary>
        /// Gets a unique identifier for the selectable.
        /// </summary>
        public string Id
        {
            get { return GetId(); }
        }

        /// <summary>
        /// Implements the getter for Id.
        /// </summary>
        /// <returns></returns>
        string GetId()
        {
            var state = element.AnnotationOrCreate<ItemState>();
            if (state.id == null)
                state.id = Guid.NewGuid().ToString("N");

            return state.id;
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
