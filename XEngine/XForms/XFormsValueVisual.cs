using System;
using System.Xml.Linq;

using XForms.Util;

namespace ISIS.Forms.XForms
{

    [VisualTypeDescriptor(Constants.XForms_1_0_NS, "value")]
    public class XFormsValueVisualTypeDescriptor : VisualTypeDescriptor
    {

        public override Visual CreateVisual(IFormProcessor form, StructuralVisual parent, XNode element)
        {
            return new XFormsValueVisual(parent, (XElement)element);
        }

    }

    public class XFormsValueVisual : XFormsSingleNodeBindingVisual, ISelectableVisual
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="element"></param>
        public XFormsValueVisual(StructuralVisual parent, XElement element)
            : base(parent, element)
        {

        }

        /// <summary>
        /// Gets the text value associated with this value visual.
        /// </summary>
        public string InlineContent
        {
            get { return Element.Value.TrimToNull(); }
        }

        /// <summary>
        /// Obtains the simple value to be set.
        /// </summary>
        /// <returns></returns>
        private string GetNewValue()
        {
            if (Binding != null &&
                Binding.Value != null)
                return Binding.Value;
            else if (InlineContent != null)
                return InlineContent;
            else
                return "";
        }

        public void Select(XFormsSingleNodeBindingVisual visual)
        {
            if (visual.Binding == null ||
                visual.Binding.Node == null)
                throw new InvalidOperationException();

            if (Binding != null &&
                Binding.Value == null)
                throw new InvalidOperationException();

            visual.Binding.SetValue(GetNewValue());
        }

        public void Deselect(XFormsSingleNodeBindingVisual visual)
        {

        }

        public bool Selected(XFormsSingleNodeBindingVisual visual)
        {
            if (visual.Binding == null ||
                visual.Binding.Node == null ||
                visual.Binding.Value == null)
                return false;

            // our value matches the current value?
            return visual.Binding.Value == GetNewValue();
        }

        public override void Refresh()
        {
            base.Refresh();

            if (Binding != null &&
                Binding.Value == null)
            {
                // binding, if it exists, must point to a value node
                // DispatchEvent<XFormsBindingExceptionEvent>();
                return;
            }
        }

        public int GetValueHashCode()
        {
            if (Binding != null &&
                Binding.Value == null)
                return 0;

            return GetNewValue().GetHashCode();
        }
    }

}
