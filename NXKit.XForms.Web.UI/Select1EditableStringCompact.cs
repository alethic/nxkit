using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web.UI;

using Telerik.Web.UI;

using NXKit.Web.UI;

namespace NXKit.XForms.Web.UI
{

    [XFormsXsdType(XmlSchemaConstants.XMLSchema_NS, "string")]
    [XFormsAppearance(Constants.XForms_1_0_NS, "compact")]
    public class Select1EditableStringCompact :
        Select1Editable
    {

        RadListBox ctl;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="visual"></param>
        public Select1EditableStringCompact(View view, XFormsSelect1Visual visual)
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
            ctl = new RadListBox();
            ctl.ID = "ctl";
            ctl.AutoPostBack = Visual.Incremental;
            ctl.SelectedIndexChanged += ctl_SelectedIndexChanged;

            UpdateListBoxItems();

            Controls.Add(ctl);
        }

        void UpdateListBoxItems()
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
                        ctlItem.Text = labelVisual.ToText();
                        ctlItem.Controls.Add(new LabelControl(View, labelVisual));
                    }
                    else
                        ctlItem.Text = "unknown";

                    if (Visual.SelectedItemVisual == itemVisual)
                        // is item selected?
                        ctlItem.Selected = true;

                    // add item to combo box
                    ctl.Items.Add(ctlItem);
                }
            }
        }

        void ctl_SelectedIndexChanged(object sender, EventArgs args)
        {
            // look up the item visual
            var itemVisual = (XFormsItemVisual)((RadListBoxItem)ctl.SelectedItem).DataItem;
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
            var selectedItemVisual = Visual.SelectedItemVisual;
            foreach (RadListBoxItem item in ctl.Items)
                item.Selected = selectedItemVisual != null && (XFormsItemVisual)item.DataItem == selectedItemVisual;
        }

        protected override void OnVisualReadOnlyOrReadWrite()
        {
            ctl.Enabled = !Visual.ReadOnly;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            // client-side control element
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "xforms-compact");
            writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID);
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            ctl.RenderControl(writer);
            writer.RenderEndTag();
        }

    }

}
