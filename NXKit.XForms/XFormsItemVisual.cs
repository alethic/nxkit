using System.Linq;
using System.Xml.Linq;

namespace NXKit.XForms
{

    [Visual("item")]
    public class XFormsItemVisual :
        XFormsVisual
    {

        bool selectableCached;
        ISelectableVisual selectable;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="xml"></param>
        public XFormsItemVisual(XElement xml)
            : base(xml)
        {

        }

        /// <summary>
        /// Gets the <see cref="ISelectable"/> for this item.
        /// </summary>
        public ISelectableVisual Selectable
        {
            get
            {
                if (!selectableCached)
                {
                    selectable = Elements.OfType<ISelectableVisual>().SingleOrDefault();
                    selectableCached = true;
                }

                return selectable;
            }
        }

    }

}
