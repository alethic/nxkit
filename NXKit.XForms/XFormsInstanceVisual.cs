using System.Collections.Generic;
using System.Xml.Linq;

namespace NXKit.XForms
{

    [Visual("instance")]
    public class XFormsInstanceVisual :
        XFormsVisual
    {

        XFormsInstanceVisualState state;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="element"></param>
        public XFormsInstanceVisual(NXElement parent, XElement element)
            : base(parent, element)
        {

        }

        /// <summary>
        /// Gets the instance state associated with this instance visual.
        /// </summary>
        public XFormsInstanceVisualState State
        {
            get { return state ?? (state = GetState<XFormsInstanceVisualState>()); }
        }

        protected override void CreateNodes()
        {
            
        }

    }

}
