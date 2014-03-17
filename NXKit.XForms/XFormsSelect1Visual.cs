using System.Linq;

namespace NXKit.XForms
{

    [Visual("select1")]
    public class XFormsSelect1Visual :
        XFormsSingleNodeBindingVisual,
        ISupportsUiCommonAttributes
    {

        bool selectedItemVisualCached;
        XFormsItemVisual selectedItemVisual;

        [Interactive]
        public bool Incremental
        {
            get { return Module.GetAttributeValue(Element, "incremental") == "true"; }
        }

        [Interactive]
        public XFormsSelection Selection
        {
            get { return Module.GetAttributeValue(Element, "selection") == "open" ? XFormsSelection.Open : XFormsSelection.Closed; }
        }

        protected override void SetValue(object value)
        {
            // deselect current visual
            if (SelectedItemVisual != null &&
                SelectedItemVisual.Selectable != null)
                SelectedItemVisual.Selectable.Deselect(this);

            // clear selected visual state
            selectedItemVisualCached = true;
            selectedItemVisual = null;
            GetState<XFormsSelect1State>().SelectedVisualId = null;

            // set node value
            if (Binding != null &&
                Binding.Node != null)
                base.SetValue(value);
        }

        /// <summary>
        /// Gets the currently selected item visual.
        /// </summary>
        public XFormsItemVisual SelectedItemVisual
        {
            get { return GetSelectedItemVisual(); }
            set { SetSelectedItemVisual(value); }
        }

        /// <summary>
        /// Implements the getter for SelectedItemVisual.
        /// </summary>
        /// <returns></returns>
        XFormsItemVisual GetSelectedItemVisual()
        {
            if (!selectedItemVisualCached)
            {
                foreach (var itemVisual in Descendants().OfType<XFormsItemVisual>())
                {
                    // find selectable visuals underneath item
                    if (itemVisual.Selectable == null)
                        continue;

                    if (itemVisual.Selectable.Selected(this))
                    {
                        selectedItemVisual = itemVisual;
                        break;
                    }
                }

                selectedItemVisualCached = true;
            }

            return selectedItemVisual;
        }

        /// <summary>
        /// Implements the setter for SelectedItemVisual.
        /// </summary>
        /// <param name="visual"></param>
        void SetSelectedItemVisual(XFormsItemVisual visual)
        {
            // deselect current visual
            if (SelectedItemVisual != null &&
                SelectedItemVisual.Selectable != null)
                SelectedItemVisual.Selectable.Deselect(this);

            if (visual != null &&
                visual.Selectable != null)
            {
                // pre-cache
                selectedItemVisualCached = true;
                selectedItemVisual = visual;

                // store selected item
                GetState<XFormsSelect1State>().SelectedVisualId = visual.UniqueId;

                // apply selection
                selectedItemVisual.Selectable.Select(this);
            }
        }

        [Interactive]
        public string SelectedItemVisualId
        {
            get { return SelectedItemVisual != null ? SelectedItemVisual.UniqueId : null; }
            set { SetSelectedItemVisualId(value); }
        }

        /// <summary>
        /// Implements the setter for SelectedItemVisualId.
        /// </summary>
        /// <param name="id"></param>
        void SetSelectedItemVisualId(string id)
        {
            SelectedItemVisual = Descendants()
                .OfType<XFormsItemVisual>()
                .FirstOrDefault(i => i.UniqueId == id);
        }

        public override void Refresh()
        {
            base.Refresh();

            // clear selected item cache
            selectedItemVisual = null;
            selectedItemVisualCached = false;

            // if no item is selected, attempt to find one
            var selectedItemId = GetState<XFormsSelect1State>().SelectedVisualId;
            if (selectedItemId == null)
            {
                // ensure descendant itemsets are refreshed, kind of a hack
                foreach (var itemSetVisual in Descendants().OfType<XFormsItemSetVisual>())
                    itemSetVisual.Refresh();

                foreach (var itemVisual in Descendants().OfType<XFormsItemVisual>())
                {
                    if (itemVisual.Selectable == null)
                        continue;

                    // is this the current selection?
                    if (itemVisual.Selectable.Selected(this))
                    {
                        // pre-cache
                        selectedItemVisualCached = true;
                        selectedItemVisual = itemVisual;

                        // store selected item
                        GetState<XFormsSelect1State>().SelectedVisualId = itemVisual.UniqueId;

                        break;
                    }
                }
            }
        }

    }

}
