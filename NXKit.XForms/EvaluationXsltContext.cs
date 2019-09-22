using System;
using System.Xml.Linq;
using System.Xml.Xsl;

using NXKit.Xml;
using NXKit.XPath;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides a <see cref="XsltContext"/> for XForms visual operations.
    /// </summary>
    public class EvaluationXsltContext :
        XObjectXsltContext
    {

        readonly EvaluationContext evaluationContext;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="evaluationContext"></param>
        public EvaluationXsltContext(
            IXsltContextFunctionProvider functionProvider,
            XObject xml,
            EvaluationContext evaluationContext)
            : base(functionProvider, xml)
        {
            if (xml == null)
                throw new ArgumentNullException(nameof(xml));

            this.evaluationContext = evaluationContext ?? throw new ArgumentNullException(nameof(evaluationContext));
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="evaluationContext"></param>
        public EvaluationXsltContext(
            XObject xml,
            EvaluationContext evaluationContext)
            : this(xml.Exports().GetExportedValue<IXsltContextFunctionProvider>(), xml, evaluationContext)
        {
            if (xml == null)
                throw new ArgumentNullException(nameof(xml));
            if (evaluationContext == null)
                throw new ArgumentNullException(nameof(evaluationContext));
        }

        /// <summary>
        /// Gets the <see cref="EvaluationContext"/> associated with the XSLT operation.
        /// </summary>
        public EvaluationContext EvaluationContext
        {
            get { return evaluationContext; }
        }

    }

}
