using System.Linq;

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
        /// Gets the <see cref="XFormsLabelVisual"/> for this item.
        /// </summary>
        public XFormsLabelVisual LabelVisual
        {
            get
            {
                if (!labelVisualCached)
                {
                    labelVisual = Visuals.OfType<XFormsLabelVisual>().SingleOrDefault();
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
                    selectable = Visuals.OfType<ISelectableVisual>().SingleOrDefault();
                    selectableCached = true;
                }

                return selectable;
            }
        }

    }

}
