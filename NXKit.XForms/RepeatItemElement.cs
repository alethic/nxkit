using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit.XForms
{

    /// <summary>
    /// Serves as a generated item within a repeat.
    /// </summary>
    public class RepeatItemElement :
        XFormsElement,
        IEvaluationContextScope,
        INamingScope,
        IRelevancyScope
    {

        EvaluationContext context;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="xml"></param>
        public RepeatItemElement(XElement xml)
            : base(xml)
        {
            Contract.Requires<ArgumentNullException>(xml != null);
        }

        /// <summary>
        /// 
        /// </summary>
        public override string Id
        {
            get { return "NODE" + Context.ModelItem.Id; }
        }

        /// <summary>
        /// Obtains the evaluation context for this visual.
        /// </summary>
        public EvaluationContext Context
        {
            get { return context; }
        }

        /// <summary>
        /// Sets the context to a new value, should only be used by the repeat container.
        /// </summary>
        /// <param name="ec"></param>
        internal void SetContext(EvaluationContext ec)
        {
            context = ec;
        }

        public bool Relevant
        {
            get { return Context.ModelItem.Relevant; }
        }

    }

}
