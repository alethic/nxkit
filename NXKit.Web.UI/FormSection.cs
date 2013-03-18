using System;
using System.Collections.Generic;
using System.Linq;

namespace NXKit.Web.UI
{

    public class FormSection : FormNavigation
    {

        private IEnumerable<FormNavigation> children;
        private Dictionary<Visual, FormNavigation> childrenCache = new Dictionary<Visual, FormNavigation>();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        internal FormSection(FormSection parent, INavigationSectionVisual visual)
            : base(parent, visual)
        {
            Visual.ChildrenInvalidated += Visual_ChildrenInvalidated;
        }

        /// <summary>
        /// Invoked when the Visual's children are invalidated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void Visual_ChildrenInvalidated(object sender, EventArgs args)
        {
            children = null;
        }

        /// <summary>
        /// Gets the children navigation items of this item.
        /// </summary>
        public IEnumerable<FormNavigation> Children
        {
            get { return children ?? (children = CreateChildren()); }
        }

        /// <summary>
        /// Generates the children list, in proper visual order.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<FormNavigation> CreateChildren()
        {
            return CreateChildren(Visual);
        }

        /// <summary>
        /// Recursive implementation of CreateChildren.
        /// </summary>
        /// <param name="visual"></param>
        /// <returns></returns>
        private IEnumerable<FormNavigation> CreateChildren(IStructuralVisual visual)
        {
            foreach (var child in visual.Children.OfType<StructuralVisual>())
                foreach (var nav in FormNavigation.CreateNavigations(this, child))
                    yield return nav;
        }

        public override bool Relevant
        {
            get { return Visual.Relevant && Descendants(false).Any(i => i.Relevant); }
        }

    }

}
