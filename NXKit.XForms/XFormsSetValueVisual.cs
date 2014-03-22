using NXKit.DOM2.Events;
using NXKit.Util;

namespace NXKit.XForms
{

    [Visual("setvalue")]
    public class XFormsSetValueVisual : 
        XFormsSingleNodeBindingVisual, 
        IActionVisual
    {

        XFormsBinding valueBinding;

        public override void Refresh()
        {
            base.Refresh();

            // reset value binding
            valueBinding = null;
            if (Binding != null)
            {
                var valueAttr = Module.GetAttributeValue(Element, "value");
                if (valueAttr != null)
                {
                    var ec = new XFormsEvaluationContext(Binding.Context.Model, Binding.Context.Instance, Binding.Context.Node, 1, 1);
                    valueBinding = new XFormsBinding(Document, this, ec, valueAttr);
                }
            }
        }

        public void Handle(IEvent ev)
        {
            Module.InvokeAction(this);
        }

        public void Invoke()
        {
            // ensure values are up to date
            Refresh();

            if (Binding == null ||
                Binding.Node == null)
                return;

            // default value
            string newValue = null;

            // resolve value from 'value' attribute
            if (valueBinding != null)
                newValue = valueBinding.Value;
            else
            {
                // resolve value from contents
                var content = Element.Value.TrimToNull();
                if (content != null)
                    newValue = content;
            }

            // default setting
            if (newValue == null)
                newValue = "";

            // set node value
            Module.SetModelItemValue(Binding.Context, Binding.Node, newValue);

            // instruct model to complete deferred update
            Binding.Context.Model.State.RecalculateFlag = true;
            Binding.Context.Model.State.RevalidateFlag = true;
            Binding.Context.Model.State.RefreshFlag = true;
        }

    }

}
