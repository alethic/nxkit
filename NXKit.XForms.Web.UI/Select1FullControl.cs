using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using NXKit.Util;
using NXKit.Web.UI;
using NXKit.XForms;

namespace NXKit.XForms.Web.UI
{

    public class Select1FullControl : VisualControl<XFormsSelect1Visual>, IScriptControl
    {

        private RadioButtonList ctl;

        private Dictionary<XFormsItemVisual, ListItem> itemCache =
            new Dictionary<XFormsItemVisual, ListItem>();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="visual"></param>
        public Select1FullControl(NXKit.Web.UI.View view, XFormsSelect1Visual visual)
            : base(view, visual)
        {

        }

        protected override void CreateChildControls()
        {
            ctl = new RadioButtonList();
            ctl.ID = "ctl";
            ctl.RepeatLayout = RepeatLayout.OrderedList;
            ctl.AutoPostBack = Visual.Incremental || true;
            ctl.SelectedIndexChanged += ctl_SelectedIndexChanged;

            UpdateRadioButtonItems();

            Controls.Add(ctl);
        }

        private void UpdateRadioButtonItems()
        {
            // track added items
            var items = new LinkedList<ListItem>();

            // currently selected item visual
            var selectedItemVisual = Visual.SelectedItemVisual;

            foreach (var itemVisual in Visual.Descendants().OfType<XFormsItemVisual>())
            {
                if (itemVisual.Selectable != null)
                {
                    // skip non-relevant items
                    if (itemVisual.Selectable is XFormsValueVisual)
                        if (!((XFormsValueVisual)itemVisual.Selectable).Relevant)
                            continue;
                    if (itemVisual.Selectable is XFormsCopyVisual)
                        if (!((XFormsCopyVisual)itemVisual.Selectable).Relevant)
                            continue;

                    // generate new item
                    var ctlItem = itemCache.GetOrCreate(itemVisual, i => new ListItem());
                    ctlItem.Value = itemVisual.Selectable.GetValueHashCode().ToString();

                    // add label if available
                    var labelVisual = itemVisual.FindLabelVisual();
                    if (labelVisual != null)
                        ctlItem.Text = labelVisual.ToText();
                    else
                        ctlItem.Text = "unknown";

                    // add back to combobox
                    if (ctl.Items.Contains(ctlItem))
                        ctl.Items.Remove(ctlItem);
                    ctl.Items.Insert(items.Count, ctlItem);

                    // update selection
                    ctlItem.Selected = selectedItemVisual != null && itemVisual == selectedItemVisual;

                    // indicate item is still in combo box
                    items.AddLast(ctlItem);
                }
            }

            // remove any items that were not just added
            foreach (var ctlItem in ctl.Items.Cast<ListItem>().ToList())
                if (!items.Contains(ctlItem))
                    ctl.Items.Remove(ctlItem);
        }

        private void ctl_SelectedIndexChanged(object sender, EventArgs args)
        {
            var item = ctl.SelectedItem;
            if (item == null)
                return;

            // look up the item visual
            var itemVisual = Visual
                .Descendants()
                .OfType<XFormsItemVisual>()
                .FirstOrDefault(i => i.Selectable != null && i.Selectable.GetValueHashCode().ToString() == item.Value);

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

            // ensure items are refreshed
            UpdateRadioButtonItems();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            ScriptManager.GetCurrent(Page).RegisterScriptDescriptors(this);

            // client-side control element
            writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "XForms_Select1 XForms_Select1_Full");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            base.Render(writer);
            writer.RenderEndTag();
        }

        IEnumerable<ScriptDescriptor> IScriptControl.GetScriptDescriptors()
        {
            var desc = new ScriptControlDescriptor("NXKit.XForms.Web.UI.Select1FullControl", ClientID);
            desc.AddComponentProperty("view", View.ClientID);
            desc.AddProperty("modelItemId", Visual.Binding != null ? Visual.Binding.NodeUniqueId : null);
            yield return desc;
        }

        IEnumerable<ScriptReference> IScriptControl.GetScriptReferences()
        {
            yield return new ScriptReference("NXKit.XForms.Web.UI.Select1FullControl.js", typeof(Select1FullControl).Assembly.FullName);
        }

    }

}
