using System.Collections.Generic;
using System.Linq;

using ISIS.Forms.Layout;
using ISIS.Forms.XForms;

namespace ISIS.Forms.Web.UI
{

    public abstract class FormNavigation
    {

        /// <summary>
        /// Returns the set of <see cref="FormNavigation"/>s for the given visual.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="visual"></param>
        /// <returns></returns>
        internal static IEnumerable<FormNavigation> CreateNavigations(FormSection parent, StructuralVisual visual)
        {
            if (visual is CategoryVisual)
                yield return new FormSection(parent, (CategoryVisual)visual);
            else if (visual is PageVisual)
                yield return new FormPage(parent, (PageVisual)visual);
            else
                foreach (var childVisual in visual.Children.OfType<StructuralVisual>())
                    foreach (var child in CreateNavigations(parent, childVisual))
                        yield return child;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        internal FormNavigation(FormSection parent, XFormsGroupVisual visual)
        {
            Parent = parent;
            Visual = visual;
        }

        /// <summary>
        /// Returns the <see cref="FormNavigation"/> under which this item exists.
        /// </summary>
        public FormSection Parent { get; private set; }

        /// <summary>
        /// Gets the visual associated with this <see cref="FormNavigation"/>.
        /// </summary>
        public XFormsGroupVisual Visual { get; private set; }

        /// <summary>
        /// Gets whether or not the navigation item is relevant.
        /// </summary>
        public abstract bool Relevant { get;  }

        /// <summary>
        /// Gets the unique id of the <see cref="Visual"/> associated with this <see cref="FormNavigation"/>.
        /// </summary>
        public string Id
        {
            get { return Visual.UniqueId; }
        }

        /// <summary>
        /// Gets a display label to be used to show this navigation.
        /// </summary>
        public string Label
        {
            get { return FormHelper.ControlLabelToString(Visual); }
        }

        /// <summary>
        /// Yields each descendant visual.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<FormNavigation> Descendants(bool includeSelf)
        {
            if (includeSelf)
                yield return this;

            if (this is FormSection)
                foreach (var child in ((FormSection)this).Children)
                    foreach (var child2 in child.Descendants(true))
                        yield return child2;
        }

    }

}
