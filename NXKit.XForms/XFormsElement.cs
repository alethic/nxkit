using System;
using System.Xml.Linq;

using NXKit.DOMEvents;

namespace NXKit.XForms
{

    /// <summary>
    /// Base class for XForms <see cref="NXNode"/> types.
    /// </summary>
    public abstract class XFormsElement :
        NXElement
    {

        XFormsModule module;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public XFormsElement()
            : base()
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        public XFormsElement(XElement element)
            : base(element)
        {

        }

        /// <summary>
        /// Gets a reference to the <see cref="XFormsModule"/> instance.
        /// </summary>
        public XFormsModule Module
        {
            get { return module ?? (module = Document.Module<XFormsModule>()); }
        }

        /// <summary>
        /// Unique identifier for the <see cref="NXNode"/> within the current naming scope.
        /// </summary>
        public override string Id
        {
            get { return Document.GetElementId(Xml); }
        }

    }

}
