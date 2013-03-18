using System.Collections.Generic;
using System.Linq;

using NXKit.Web.UI;

namespace NXKit.XForms.Web.UI
{

    public class TriggerControlCollection : VisualControlCollection
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="visual"></param>
        public TriggerControlCollection(FormView view, StructuralVisual visual)
            : base(view, visual)
        {

        }

        protected override IEnumerable<Visual> GetVisuals()
        {
            return View.OpaqueChildren(Visual).OfType<XFormsTriggerVisual>();
        }

    }

}
