using System;
using System.Diagnostics.Contracts;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the standard XForms binding attributes.
    /// </summary>
    public class NodeBindingAttributes
    {

        readonly NXElement element;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public NodeBindingAttributes(NXElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.element = element;
        }

        /// <summary>
        /// Gets the XForms attribute of the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        string GetAttribute(string name)
        {
            Contract.Requires<ArgumentNullException>(name != null);

            var fq = element.Attribute(Constants.XForms_1_0 + name);
            if (fq != null)
                return (string)fq;

            var ln = element.Name.Namespace == Constants.XForms_1_0 ? element.Attribute(name) : null;
            if (ln != null)
                return (string)ln;

            return null;
        }

        /// <summary>
        /// Gets the 'model' attribute value.
        /// </summary>
        public string Model
        {
            get { return GetAttribute("model"); }
        }

        /// <summary>
        /// Gets the 'bind' attribute value.
        /// </summary>
        public string Bind
        {
            get { return GetAttribute("bind"); }
        }

        /// <summary>
        /// Gets the 'nodeset' attribute values.
        /// </summary>
        public string NodeSet
        {
            get { return GetAttribute("nodeset"); }
        }

        /// <summary>
        /// Gets the 'ref' attribute values.
        /// </summary>
        public string Ref
        {
            get { return GetAttribute("ref"); }
        }

    }

}