using System.Linq;
using System.Xml.Linq;

namespace NXKit.XForms
{

    [Element("item")]
    public class ItemElement :
        XFormsElement
    {

        bool selectableCached;
        ISelectableNode selectable;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="xml"></param>
        public ItemElement(XElement xml)
            : base(xml)
        {

        }

        /// <summary>
        /// Gets the <see cref="ISelectable"/> for this item.
        /// </summary>
        public ISelectableNode Selectable
        {
            get
            {
                if (!selectableCached)
                {
                    selectable = Elements().OfType<ISelectableNode>().SingleOrDefault();
                    selectableCached = true;
                }

                return selectable;
            }
        }

    }

}
