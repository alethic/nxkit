using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web.UI;

using Telerik.Web.UI;

using NXKit.Util;
using NXKit.Web.UI;

namespace NXKit.XForms.Web.UI
{

    [XFormsXsdType(XmlSchemaConstants.XMLSchema_NS, "string")]
    [XFormsAppearance(Constants.XForms_1_0_NS, "minimal")]
    public class Select1EditableStringMinimal :
        Select1Editable,
        IScriptControl
    {

        RadComboBox ctl;

        Dictionary<XFormsItemVisual, RadComboBoxItem> itemCache =
           new Dictionary<XFormsItemVisual, RadComboBoxItem>();

        Dictionary<XFormsLabelVisual, LabelControl> itemLabelControlCache =
           new Dictionary<XFormsLabelVisual, LabelControl>();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="visual"></param>
        public Select1EditableStringMinimal(View view, XFormsSelect1Visual visual)
            : base(view, visual)
        {
            Contract.Requires<ArgumentNullException>(view != null);
            Contract.Requires<ArgumentNullException>(visual != null);
        }

        public override string TargetID
        {
            get { return ctl.ClientID; }
        }

        protected override void CreateChildControls()
        {
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

        /// <summary>
        /// Rebuilds the combo box item set.
        /// </summary>
        void UpdateRadComboBoxItems()
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
                    var ctlItem = itemCache.GetOrAdd(itemVisual, i => new RadComboBoxItem());
                    ctlItem.DataItem = itemVisual;
                    ctlItem.Value = itemVisual.Selectable.GetValueHashCode().ToString();

                    // add label if available
                    var labelVisual = itemVisual.FindLabelVisual();
                    if (labelVisual != null)
                    {
                        ctlItem.Text = labelVisual.ToText();
                        ctlItem.Controls.AddAt(0, itemLabelControlCache.GetOrAdd(labelVisual, i => new LabelControl(View, labelVisual)));
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

        void ctl_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs args)
        {
            OnValueChanged();
        }

        void ctl_TextChanged(object sender, EventArgs args)
        {
            OnValueChanged();
        }

        void OnValueChanged()
        {
            var selectedItem = (RadComboBoxItem)ctl.SelectedItem;
            if (selectedItem == null)
            {
                BindingUtil.Set(Visual.Binding, ctl.Text);
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

        protected override void OnVisualValueChanged()
        {
            UpdateRadComboBoxItems();
        }

        protected override void OnPreRender(EventArgs args)
        {
            base.OnPreRender(args);
            ScriptManager.GetCurrent(Page).RegisterScriptControl(this);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            ScriptManager.GetCurrent(Page).RegisterScriptDescriptors(this);

            // client-side control element
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "xforms-minimal");
            writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID);
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            ctl.RenderControl(writer);
            writer.RenderEndTag();
        }

        IEnumerable<ScriptDescriptor> IScriptControl.GetScriptDescriptors()
        {
            //var desc = new ScriptControlDescriptor("_NXKit.XForms.Web.UI.Select1EditableStringMinimal", ClientID);
            //desc.AddComponentProperty("view", View.ClientID);
            //desc.AddProperty("modelItemId", Visual.Binding != null ? Visual.Binding.NodeUniqueId : null);
            //desc.AddComponentProperty("radComboBox", ctl.ClientID);
            //yield return desc;
            yield break;
        }

        IEnumerable<ScriptReference> IScriptControl.GetScriptReferences()
        {
            //yield return new ScriptReference("NXKit.XForms.Web.UI.Select1EditableStringMinimal.js", typeof(Select1EditableStringMinimal).Assembly.FullName);
            yield break;
        }

    }

}
