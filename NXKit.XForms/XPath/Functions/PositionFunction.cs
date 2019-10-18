﻿using System.Xml.XPath;

using NXKit.Diagnostics;
using NXKit.XPath;

namespace NXKit.XForms.XPath.Functions
{

    [XsltContextFunction("{http://www.w3.org/2002/xforms}position")]
    public class PositionFunction :
        XPathFunction
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="trace"></param>
        public PositionFunction(ITraceService trace) :
            base(trace)
        {

        }

        public override XPathResultType[] ArgTypes
        {
            get { return new XPathResultType[0]; }
        }

        public override int Minargs
        {
            get { return 0; }
        }

        public override int Maxargs
        {
            get { return 0; }
        }

        public override XPathResultType ReturnType
        {
            get { return XPathResultType.Number; }
        }

        protected override object Invoke(EvaluationXsltContext context, XPathNavigator navigator, params object[] args)
        {
            return context.EvaluationContext.Position;
        }

    }

}
