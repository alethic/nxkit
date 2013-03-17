using System;
using System.Linq;
using System.Web.UI.WebControls;

using NXKit.XForms;

namespace NXKit.XForms.Web.UI.XForms
{

    public class SelectFullControl : VisualControl<XFormsSelectVisual>
    {

        private CheckBoxList ctl;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="visual"></param>
        public SelectFullControl(FormView view, XFormsSelectVisual visual)
            : base(view, visual)
        {

        }

        protected override void CreateChildControls()
        {
            ctl = new CheckBoxList();
            ctl.ID = "ctl";
            ctl.AutoPostBack = Visual.Incremental;
            ctl.SelectedIndexChanged += ctl_SelectedIndexChanged;
            
            CreateCheckBoxItems();

            Controls.Add(ctl);
        }

        private void CreateCheckBoxItems()
        {
            foreach (var itemVisual in Visual.Descendants().OfType<XFormsItemVisual>())
            {
                if (itemVisual.Selectable != null)
                {
                    // generate new item
                    var ctlItem = new ListItem();
                    ctlItem.Value = itemVisual.UniqueId;

                    // add label if available
                    var labelVisual = itemVisual.FindLabelVisual();
                    if (labelVisual != null)
                        ctlItem.Text = FormHelper.LabelToString(labelVisual);
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
            //var item = ctl.SelectedItem;
            //if (item == null)
            //    return;

            //// look up the item visual
            //var itemVisual = Visual
            //    .Descendants()
            //    .OfType<XFormsItemVisual>()
            //    .FirstOrDefault(i => i.UniqueId == item.Value);
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

            //foreach (ListItem item in ctl.Items)
            //    item.Selected = item.Value == selectedItemVisual.UniqueId;
        }

    }

}
