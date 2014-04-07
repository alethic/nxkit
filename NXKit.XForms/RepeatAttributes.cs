using System;
using System.Diagnostics.Contracts;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the standard XForms binding attributes.
    /// </summary>
    public class RepeatAttributes :
        AttributeAccessor
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public RepeatAttributes(NXElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
        }

        /// <summary>
        /// Gets the 'ref' attribute values.
        /// </summary>
        public int StartIndex
        {
            get { return (int?)GetAttribute("startindex") ?? 1; }
        }

        /// <summary>
        /// Gets the 'nodeset' attribute values.
        /// </summary>
        public int? Number
        {
            get { return (int?)GetAttribute("number"); }
        }

    }

}