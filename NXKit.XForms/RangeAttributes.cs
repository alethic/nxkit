using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the standard XForms binding attributes.
    /// </summary>
    public class RangeAttributes :
        AttributeAccessor
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public RangeAttributes(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
        }

        /// <summary>
        /// Gets the 'start' attribute values.
        /// </summary>
        public int? Start
        {
            get { return (int?)GetAttribute("start"); }
        }

        /// <summary>
        /// Gets the 'end' attribute values.
        /// </summary>
        public int? End
        {
            get { return (int?)GetAttribute("end"); }
        }

        /// <summary>
        /// Gets the 'end' attribute values.
        /// </summary>
        public int? Step
        {
            get { return (int?)GetAttribute("step"); }
        }

    }

}