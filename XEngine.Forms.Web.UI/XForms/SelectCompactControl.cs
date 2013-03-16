using System;
using System.Linq;

using Telerik.Web.UI;

using XEngine.Forms.XForms;

namespace XEngine.Forms.Web.UI.XForms
{

    public class SelectCompactControl : VisualControl<XFormsSelectVisual>
    {

        private RadListBox ctl;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="visual"></param>
        public SelectCompactControl(FormView view, XFormsSelectVisual visual)
            : base(view, visual)
        {

        }

        protected override void CreateChildControls()
        {
            ctl = new RadListBox();
            ctl.ID = "ctl";
            ctl.AutoPostBack = Visual.Incremental;
            ctl.SelectionMode = ListBoxSelectionMode.Multiple;
            ctl.SelectedIndexChanged += ctl_SelectedIndexChanged;

            CreateRadListBoxItems();

            Controls.Add(ctl);
        }

        private void CreateRadListBoxItems()
        {
            foreach (var itemVisual in Visual.Descendants().OfType<XFormsItemVisual>())
            {
                if (itemVisual.Selectable != null)
                {
                    // generate new item
                    var ctlItem = new RadListBoxItem();
                    ctlItem.DataItem = itemVisual;
                    ctlItem.Value = itemVisual.UniqueId;

                    // add label if available
                    var labelVisual = itemVisual.FindLabelVisual();
                    if (labelVisual != null)
                    {
                        ctlItem.Text = FormHelper.LabelToString(labelVisual);
                        ctlItem.Controls.Add(new LabelControl(View, labelVisual));
                    }
                    else
                        ctlItem.Text = "unknown";

                    //if (Visual.SelectedItemVisual == itemVisual)
                    //    // is item selected?
                    //    ctlItem.Selected = true;

                    // add item to combo box
                    ctl.Items.Add(ctlItem);
                }
            }
        }

        private void ctl_SelectedIndexChanged(object sender, EventArgs args)
        {
            //// look up the item visual
            //var itemVisual = (XFormsItemVisual)((RadListBoxItem)ctl.SelectedItem).DataItem;
            //if (itemVisual == null)
            //    return;

            //// no change in selection made
            //if (itemVisual == Visual.SelectedItemVisual)
            //    return;

            //// select current item
            //if (itemVisual != null)
            //    Visual.SetSelectedItemVisual(itemVisual);
        }

        protected override void OnPreRender(EventArgs args)
        {
            //base.OnPreRender(args);

            //var selectedItemVisual = Visual.SelectedItemVisual;

            //foreach (RadListBoxItem item in ctl.Items)
            //    item.Selected = (XFormsItemVisual)item.DataItem == selectedItemVisual;
        }

    }

}
