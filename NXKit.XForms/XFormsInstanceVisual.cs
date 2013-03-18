using System.Collections.Generic;
using System.Xml.Linq;

namespace NXKit.XForms
{

    [Visual("instance")]
    public class XFormsInstanceVisual : XFormsVisual
    {

        XFormsInstanceVisualState state;

        public XFormsInstanceVisualState State
        {
            get { return state ?? (state = GetState<XFormsInstanceVisualState>()); }
        }

        protected override IEnumerable<Visual> CreateChildren()
        {
            // an instance has no visual children
            yield break;
        }

    }

}
