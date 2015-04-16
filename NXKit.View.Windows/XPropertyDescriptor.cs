using System;
using System.ComponentModel;
using System.Xml.Linq;

namespace NXKit.View.Windows
{

    abstract class XPropertyDescriptor<T, TProperty> :
        PropertyDescriptor
        where T : XObject
    {

        public override Type ComponentType
        {
            get { return typeof(T); }
        }

        public override bool IsReadOnly
        {
            get { return true; }
        }

        public override Type PropertyType
        {
            get { return typeof(TProperty); }
        }

        public override bool SupportsChangeEvents
        {
            get { return true; }
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        public XPropertyDescriptor(string name)
            : base(name, null)
        {

        }

        public override void AddValueChanged(object component, EventHandler handler)
        {
            bool valueChangedHandler = base.GetValueChangedHandler(component) != null;
            base.AddValueChanged(component, handler);
            if (valueChangedHandler)
            {
                return;
            }
            T t = (T)(component as T);
            if (t != null && base.GetValueChangedHandler(component) != null)
            {
                XPropertyDescriptor<T, TProperty> xPropertyDescriptor = this;
                t.Changing += new EventHandler<XObjectChangeEventArgs>(xPropertyDescriptor.OnChanging);
                XPropertyDescriptor<T, TProperty> xPropertyDescriptor1 = this;
                t.Changed += new EventHandler<XObjectChangeEventArgs>(xPropertyDescriptor1.OnChanged);
            }
        }

        public override bool CanResetValue(object component)
        {
            return false;
        }

        protected virtual void OnChanged(object sender, XObjectChangeEventArgs args)
        {

        }

        protected virtual void OnChanging(object sender, XObjectChangeEventArgs args)
        {

        }

        public override void RemoveValueChanged(object component, EventHandler handler)
        {
            base.RemoveValueChanged(component, handler);
            T t = (T)(component as T);
            if (t != null && base.GetValueChangedHandler(component) == null)
            {
                XPropertyDescriptor<T, TProperty> xPropertyDescriptor = this;
                t.Changing -= new EventHandler<XObjectChangeEventArgs>(xPropertyDescriptor.OnChanging);
                XPropertyDescriptor<T, TProperty> xPropertyDescriptor1 = this;
                t.Changed -= new EventHandler<XObjectChangeEventArgs>(xPropertyDescriptor1.OnChanged);
            }
        }

        public override void ResetValue(object component)
        {

        }

        public override void SetValue(object component, object value)
        {

        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }

    }

}
