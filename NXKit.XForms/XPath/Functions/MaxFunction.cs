using System.Xml.Linq;
using System.Xml.XPath;

using NXKit.Diagnostics;
using NXKit.XPath;

namespace NXKit.XForms.XPath.Functions
{

    [XsltContextFunction("{http://www.w3.org/2002/xforms}max")]
    public class MaxFunction : 
        XPathFunction
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="trace"></param>
        public MaxFunction(ITraceService trace) :
            base(trace)
        {

        }

        public override XPathResultType[] ArgTypes
        {
            get { return new XPathResultType[] { XPathResultType.NodeSet }; }
        }

        public override int Minargs
        {
            get { return 1; }
        }

        public override int Maxargs
        {
            get { return 1; }
        }

        public override XPathResultType ReturnType
        {
            get { return XPathResultType.Number; }
        }

        protected override object Invoke(EvaluationXsltContext context, XPathNavigator navigator, params object[] args)
        {
            var nodes = args[0] as XPathNodeIterator;
            if (nodes == null)
                return double.NaN;

            double current = double.MinValue;
            foreach (var node in nodes)
            {
                double value = double.MinValue;
                if (node is XElement && !double.TryParse(((XElement)node).Value, out value) ||
                    node is XAttribute && !double.TryParse(((XAttribute)node).Value, out value))
                    continue;

                if (current < value)
                    current = value;
            }

            return current != double.MinValue ? current : double.NaN;
        }

    }

}
