using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace NXKit.Web.UI
{

    /// <summary>
    /// Describes a point in the view which can be navigated to.
    /// </summary>
    public abstract class FormNavigation
    {

        /// <summary>
        /// Returns the set of <see cref="FormNavigation"/>s for the given visual.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="visual"></param>
        /// <returns></returns>
        internal static IEnumerable<FormNavigation> CreateNavigations(FormSection parent, ContentVisual visual)
        {
            Contract.Requires<ArgumentNullException>(visual != null);

            if (visual is INavigationSectionVisual)
                yield return new FormSection(parent, (INavigationSectionVisual)visual);
            else if (visual is INavigationPageVisual)
                yield return new FormPage(parent, (INavigationPageVisual)visual);
            else
                foreach (var childVisual in visual.Children.OfType<ContentVisual>())
                    foreach (var child in CreateNavigations(parent, childVisual))
                        yield return child;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        internal FormNavigation(FormSection parent, INavigationVisual visual)
        {
            Contract.Requires<ArgumentNullException>(visual != null);

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
        public INavigationVisual Visual { get; private set; }

        /// <summary>
        /// Gets whether or not the navigation item is relevant.
        /// </summary>
        public virtual bool Relevant
        {
            get { return Visual.Relevant; }
        }

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
            get { return Visual.Label; }
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

        public override bool Equals(object obj)
        {
            var n = obj as FormNavigation;
            return n != null ? n.Id == Id : false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

    }

}
