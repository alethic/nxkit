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
        /// <param name="xml"></param>
        public XFormsInstanceVisual(XElement xml)
            : base(xml)
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
