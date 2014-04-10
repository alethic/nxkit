using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit.XForms
{

    /// <summary>
    /// Serves as a generated item within a repeat.
    /// </summary>
    public class RepeatItem
    {

        EvaluationContext context;
        UIBinding uiBinding;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="xml"></param>
        public RepeatItem(XElement xml)
        {
            Contract.Requires<ArgumentNullException>(xml != null);
        }

        /// <summary>
        /// Obtains the evaluation context for this visual.
        /// </summary>
        public EvaluationContext Context
        {
            get { return context; }
            internal set { context = value; }
        }

    }

}
