using System;
using System.Linq;
using NXKit.DOMEvents;

namespace NXKit.XForms
{

    /// <summary>
    /// Base class for XForms <see cref="Visual"/> types.
    /// </summary>
    public abstract class XFormsVisual : 
        ContentVisual
    {

        /// <summary>
        /// Gets a reference to the <see cref="XFormsModule"/> instance.
        /// </summary>
        public XFormsModule Module
        {
            get { return Document.GetModule<XFormsModule>(); }
        }

        /// <summary>
        /// Unique identifier for the <see cref="Visual"/> within the current naming scope.
        /// </summary>
        public override string Id
        {
            get { return Document.GetElementId(Element); }
        }

        /// <summary>
        /// Dispatches the typed XForms event.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        [Obsolete("Raise events by string type instead.")]
        protected void DispatchEvent<T>()
            where T : XFormsEvent
        {
            var evt = (XFormsEvent)Activator.CreateInstance(typeof(T), new object[] { this });
            if (evt == null)
                throw new NullReferenceException();

            Interface<IEventTarget>().DispatchEvent(evt.Event);
        }

    }

}
