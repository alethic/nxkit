using System;
using System.Linq;

using Telerik.Web.UI;

using NXKit.XForms;
using NXKit.Web.UI;

namespace NXKit.XForms.Web.UI
{

    public class SelectMinimalControl : VisualControl<XFormsSelectVisual>
    {

        private RadComboBox ctl;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="visual"></param>
        public SelectMinimalControl(FormView view, XFormsSelectVisual visual)
            : base(view, visual)
        {

        }

        protected override void CreateChildControls()
        {
            throw new NotImplementedException();
        }

        private void CreateRadComboBoxItems()
        {
            throw new NotImplementedException();
            //foreach (var itemVisual in Visual.Descendants().OfType<XFormsItemVisual>())
            //{
            //    if (itemVisual.Selectable != null)
            //    {
            //        // generate new item
            //        var ctlItem = new RadComboBoxItem();
            //        ctlItem.DataItem = itemVisual;
            //        ctlItem.Value = itemVisual.UniqueId;

            //        // add label if available
            //        var labelVisual = itemVisual.Children.OfType<XFormsLabelVisual>().FirstOrDefault();
            //        if (labelVisual != null)
            //        {
            //            ctlItem.Text = FormHelper.LabelToString(labelVisual);
            //            ctlItem.Controls.Add(new LabelVisualControl(View, labelVisual));
            //        }
            //        else
            //            ctlItem.Text = "unknown";

            //        if (Visual.SelectedItemVisual == itemVisual)
            //            // is item selected?
            //            ctlItem.Selected = true;

            //        // add item to combo box
            //        ctl.Items.Add(ctlItem);
            //    }
            //}
        }

        private void ctl_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs args)
        {
            //// look up the item visual
            //var itemVisual = (XFormsItemVisual)((RadComboBoxItem)ctl.SelectedItem).DataItem;
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

            //foreach (RadComboBoxItem item in ctl.Items)
            //    item.Selected = (XFormsItemVisual)item.DataItem == selectedItemVisual;
        }

    }

}
