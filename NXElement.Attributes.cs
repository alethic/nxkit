using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;

namespace NXKit
{

    public partial class NXElement
    {

        internal NXAttribute lastAttr;

        /// <summary>
        /// Returns a collection of attributes of this element.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<NXAttribute> Attributes()
        {
            var attr = lastAttr;
            if (attr != null)
            {
                do
                {
                    yield return attr = attr.next;
                }
                while (attr.parent == this && attr != lastAttr);
            }
        }

        /// <summary>
        /// Returns a collection of attributes of this element with the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IEnumerable<NXAttribute> Attributes(XName name)
        {
            Contract.Requires<ArgumentNullException>(name != null);

            return Attributes().Where(i => i.Name == name);
        }

        /// <summary>
        /// Returns the attribute with the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public NXAttribute Attribute(XName name)
        {
            Contract.Requires<ArgumentNullException>(name != null);

            return Attributes(name).FirstOrDefault();
        }

        /// <summary>
        /// Adds a new attribute to the end of the element.
        /// </summary>
        /// <param name="attribute"></param>
        internal void AppendAttribute(NXAttribute attribute)
        {
            Contract.Requires<ArgumentNullException>(attribute != null);

            // reparent
            attribute.parent = this;

            if (lastAttr != null)
            {
                // insert into list
                attribute.next = lastAttr.next;
                lastAttr.next = attribute;
            }
            else
            {
                // first attribute
                attribute.next = attribute;
            }

            // point to new last attribute
            lastAttr = attribute;
        }

        /// <summary>
        /// Sets the value of an attribute, adds an attribute, or removes an attribute.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetAttributeValue(XName name, object value)
        {
            var attr = Attribute(name);

            if (attr != null)
                // attribute already exists
                if (value != null)
                    // set new value
                    attr.Value = value.ToString();
                else
                    // remove attribute
                    RemoveAttribute(attr);
            else if (value != null)
                // attribute is missing, and we have value
                AppendAttribute(new NXAttribute(name, value.ToString()));
        }

        /// <summary>
        /// Removes the specified attribute.
        /// </summary>
        /// <param name="attribute"></param>
        internal void RemoveAttribute(NXAttribute attribute)
        {
            Contract.Requires<ArgumentNullException>(attribute != null);
            Contract.Requires<InvalidOperationException>(attribute.Parent != this);

            var attr = lastAttr;

            while (true)
            {
                var attr1 = attr.next;
                var attr2 = attr1;
                if (attr1 == attribute)
                    break;

                attr = attr2;
            }

            if (attr != attribute)
            {
                if (lastAttr == attribute)
                    lastAttr = attribute;

                attr.next = attribute.next;
            }
            else
            {
                lastAttr = null;
            }

            attribute.parent = null;
            attribute.next = null;
        }

    }

}
