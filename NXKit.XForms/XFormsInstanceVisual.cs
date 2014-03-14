using System.Collections.Generic;

namespace NXKit.XForms
{

    [Visual("instance")]
    public class XFormsInstanceVisual :
        XFormsVisual
    {

        XFormsInstanceVisualState state;

        /// <summary>
        /// Gets the instance state associated with this instance visual.
        /// </summary>
        public XFormsInstanceVisualState State
        {
            get { return state ?? (state = GetState<XFormsInstanceVisualState>()); }
        }

        protected override IEnumerable<Visual> CreateVisuals()
        {
            // an instance has no visual children
            yield break;
        }

    }

}
