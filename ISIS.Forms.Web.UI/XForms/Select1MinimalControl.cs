using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;

using ISIS.Forms.XForms;

using XForms.Util;

using Telerik.Web.UI;

namespace ISIS.Forms.Web.UI.XForms
{

    public class Select1MinimalControl : VisualControl<XFormsSelect1Visual>, IScriptControl
    {

        private RadComboBox ctl;

        private Dictionary<XFormsItemVisual, RadComboBoxItem> itemCache =
            new Dictionary<XFormsItemVisual, RadComboBoxItem>();

        private Dictionary<XFormsLabelVisual, LabelControl> itemLabelControlCache =
            new Dictionary<XFormsLabelVisual, LabelControl>();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="visual"></param>
        public Select1MinimalControl(FormView view, XFormsSelect1Visual visual)
            : base(view, visual)
        {

        }

        public CommonControlCollection Common { get; private set; }

        protected override void CreateChildControls()
        {
            Controls.Add(Common = new CommonControlCollection(View, Visual));

            ctl = new RadComboBox();
            ctl.ID = "ctl";
            ctl.AutoPostBack = Visual.Incremental;
            ctl.EnableViewState = false;
            ctl.AllowCustomText = Visual.Selection == XFormsSelection.Open;
            ctl.SelectedIndexChanged += ctl_SelectedIndexChanged;
            ctl.TextChanged += ctl_TextChanged;

            UpdateRadComboBoxItems();

            Controls.Add(ctl);
        }

        private void UpdateRadComboBoxItems()
        {
            // track added items
            var items = new LinkedList<RadComboBoxItem>();

            // currently selected item visual
            var selectedItemVisual = Visual.SelectedItemVisual;

            foreach (var itemVisual in Visual.Descendants().OfType<XFormsItemVisual>())
            {
                if (itemVisual.Selectable != null)
                {
                    var relevant = true;

                    // skip non-relevant items
                    if (itemVisual.Selectable is XFormsValueVisual)
                        if (!((XFormsValueVisual)itemVisual.Selectable).Relevant)
                            relevant = false;
                    if (itemVisual.Selectable is XFormsCopyVisual)
                        if (!((XFormsCopyVisual)itemVisual.Selectable).Relevant)
                            relevant = false;

                    // obtain or create new item
                    var ctlItem = itemCache.GetOrCreate(itemVisual, i => new RadComboBoxItem());
                    ctlItem.DataItem = itemVisual;
                    ctlItem.Value = itemVisual.Selectable.GetValueHashCode().ToString();

                    // add label if available
                    var labelVisual = itemVisual.Children.OfType<XFormsLabelVisual>().FirstOrDefault();
                    if (labelVisual != null)
                    {
                        ctlItem.Text = FormHelper.LabelToString(labelVisual);
                        ctlItem.Controls.AddAt(0, itemLabelControlCache.GetOrCreate(labelVisual, i => new LabelControl(View, labelVisual)));
                    }
                    else
                        ctlItem.Text = "unknown";

                    // add back to combobox
                    if (ctl.Items.Contains(ctlItem))
                        ctl.Items.Remove(ctlItem);
                    ctl.Items.Insert(items.Count, ctlItem);

                    // update selection
                    ctlItem.Selected = selectedItemVisual != null && itemVisual == selectedItemVisual;
                    ctlItem.Visible = relevant;

                    // indicate item is still in combo box
                    items.AddLast(ctlItem);
                }
            }

            // remove any items that were not just added
            foreach (var ctlItem in ctl.Items.ToList())
                if (!items.Contains(ctlItem))
                    ctl.Items.Remove(ctlItem);

            // no item selected, but value available, and selection open
            if (Visual.Selection == XFormsSelection.Open && selectedItemVisual == null)
                ctl.Text = Visual.Binding != null ? Visual.Binding.Value : "";
        }

        private void ctl_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs args)
        {
            ValueChanged();
        }

        private void ctl_TextChanged(object sender, EventArgs args)
        {
            ValueChanged();
        }

        private void ValueChanged()
        {
            var selectedItem = (RadComboBoxItem)ctl.SelectedItem;
            if (selectedItem == null)
            {
                // value is user entered
                if (ctl.Text != Visual.Binding.Value)
                    Visual.SetValue(ctl.Text);
                return;
            }

            // look up the item visual
            var itemVisual = (XFormsItemVisual)((RadComboBoxItem)ctl.SelectedItem).DataItem;
            if (itemVisual == null)
                return;

            // no change in selection made
            if (itemVisual == Visual.SelectedItemVisual)
                return;

            // select current item
            if (itemVisual != null)
                Visual.SetSelectedItemVisual(itemVisual);
        }

        protected override void OnPreRender(EventArgs args)
        {
            base.OnPreRender(args);
            ScriptManager.GetCurrent(Page).RegisterScriptControl(this);

            ctl.Enabled = !Visual.ReadOnly;

            // ensure combobox items are refreshed
            UpdateRadComboBoxItems();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            ScriptManager.GetCurrent(Page).RegisterScriptDescriptors(this);

            // client-side control element
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "XForms_Select1_Minimal");
            writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID);
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            if (Common.LabelControl != null)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.For, ctl.ClientID);
                writer.RenderBeginTag(HtmlTextWriterTag.Label);
                Common.LabelControl.RenderControl(writer);
                writer.RenderEndTag();
            }

            ctl.RenderControl(writer);

            writer.RenderEndTag();
        }

        IEnumerable<ScriptDescriptor> IScriptControl.GetScriptDescriptors()
        {
            var desc = new ScriptControlDescriptor("ISIS.Forms.Web.UI.XForms.Select1MinimalControl", ClientID);
            desc.AddComponentProperty("formView", View.ClientID);
            desc.AddProperty("modelItemId", Visual.Binding != null ? Visual.Binding.NodeUniqueId : null);
            desc.AddComponentProperty("radComboBox", ctl.ClientID);
            yield return desc;
        }

        IEnumerable<ScriptReference> IScriptControl.GetScriptReferences()
        {
            yield return new ScriptReference("ISIS.Forms.Web.UI.XForms.Select1MinimalControl.js", typeof(Select1MinimalControl).Assembly.FullName);
        }

    }

}
