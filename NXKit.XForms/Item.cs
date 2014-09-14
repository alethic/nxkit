using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;

using NXKit.Composition;
using NXKit.Xml;

namespace NXKit.XForms
{

    [Extension("{http://www.w3.org/2002/xforms}item")]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class Item :
        ElementExtension,
        ISelectable
    {

        readonly Lazy<ISelectableValue> value;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        [ImportingConstructor]
        public Item(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.value = new Lazy<ISelectableValue>(() => Element
                .Descendants()
                .SelectMany(i => i.Interfaces<ISelectableValue>())
                .FirstOrDefault());
        }

        /// <summary>
        /// Gets a unique identifier for the selectable.
        /// </summary>
        public int Id
        {
            get { return Element.GetObjectId(); }
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
