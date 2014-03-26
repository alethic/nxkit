using System.Xml.Linq;
using NXKit.DOMEvents;
using NXKit.Util;

namespace NXKit.XForms
{

    [Element("setvalue")]
    public class SetValueElement :
        SingleNodeUIBindingElement,
        IActionElement
    {

        Binding valueBinding;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public SetValueElement(XElement element)
            : base(element)
        {

        }

        public override void Refresh()
        {
            base.Refresh();

            // reset value binding
            valueBinding = null;
            if (Binding != null)
            {
                var valueAttr = Module.GetAttributeValue(Xml, "value");
                if (valueAttr != null)
                {
                    var ec = new EvaluationContext(Binding.Context.Model, Binding.Context.Instance, Binding.Context.ModelItem, 1, 1);
                    valueBinding = new Binding(this, ec, valueAttr);
                }
            }
        }

        public void Handle(Event ev)
        {
            Module.InvokeAction(this);
        }

        public void Invoke()
        {
            // ensure values are up to date
            Refresh();

            if (Binding == null ||
                Binding.ModelItem == null)
                return;

            // default value
            string newValue = null;

            // resolve value from 'value' attribute
            if (valueBinding != null)
                newValue = valueBinding.Value;
            else
            {
                // resolve value from contents
                var content = Xml.Value.TrimToNull();
                if (content != null)
                    newValue = content;
            }

            // default setting
            if (newValue == null)
                newValue = "";

            // instruct model to complete deferred update
            Binding.ModelItem.Value = newValue;
            Binding.ModelItem.Model.State.RecalculateFlag = true;
            Binding.ModelItem.Model.State.RevalidateFlag = true;
            Binding.ModelItem.Model.State.RefreshFlag = true;
        }

    }

}
