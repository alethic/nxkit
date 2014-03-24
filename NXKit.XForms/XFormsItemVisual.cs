using System.Linq;
using System.Xml.Linq;

namespace NXKit.XForms
{

    [Visual("item")]
    public class XFormsItemVisual :
        XFormsVisual
    {

        bool labelVisualCached;
        XFormsLabelVisual labelVisual;
        bool selectableCached;
        ISelectableVisual selectable;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="element"></param>
        public XFormsItemVisual(NXElement parent, XElement element)
            : base(parent, element)
        {

        }

        /// <summary>
        /// Gets the <see cref="XFormsLabelVisual"/> for this item.
        /// </summary>
        public XFormsLabelVisual LabelVisual
        {
            get
            {
                if (!labelVisualCached)
                {
                    labelVisual = Elements.OfType<XFormsLabelVisual>().SingleOrDefault();
                    labelVisualCached = true;
                }

                return labelVisual;
            }
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
