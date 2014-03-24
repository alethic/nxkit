﻿using System;
using System.Xml.Linq;

using NXKit.DOMEvents;

namespace NXKit.XForms
{

    /// <summary>
    /// Base class for XForms <see cref="NXNode"/> types.
    /// </summary>
    public abstract class XFormsVisual :
        NXElement
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        public XFormsVisual(XElement element)
            : base(element)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        public XFormsVisual(NXElement parent)
            : base(parent)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="element"></param>
        public XFormsVisual(NXElement parent, XElement element)
            : base(parent, element)
        {

        }

        /// <summary>
        /// Gets a reference to the <see cref="XFormsModule"/> instance.
        /// </summary>
        public XFormsModule Module
        {
            get { return Document.Module<XFormsModule>(); }
        }

        /// <summary>
        /// Unique identifier for the <see cref="NXNode"/> within the current naming scope.
        /// </summary>
        public override string Id
        {
            get { return Document.GetElementId(Xml); }
        }

        /// <summary>
        /// Dispatches the typed XForms event.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        [Obsolete("Raise events by string type instead.")]
        public void DispatchEvent<T>()
            where T : XFormsEvent
        {
            var evt = (XFormsEvent)Activator.CreateInstance(typeof(T), new object[] { this });
            if (evt == null)
                throw new NullReferenceException();

            this.Interface<IEventTarget>().DispatchEvent(evt.Event);
        }

    }

}
