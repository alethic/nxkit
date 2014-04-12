using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;
using System.Xml.XPath;

using NXKit.Xml;

namespace NXKit.XForms
{

    /// <summary>
    /// Describes a context against which bindings can be applied.
    /// </summary>
    public class EvaluationContext
    {

        readonly Model model;
        readonly Instance instance;
        readonly ModelItem modelItem;
        readonly int position;
        readonly int size;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="instance"></param>
        /// <param name="modelItem"></param>
        /// <param name="position"></param>
        /// <param name="size"></param>
        internal EvaluationContext(Model model, Instance instance, ModelItem modelItem, int position, int size)
        {
            Contract.Requires<ArgumentNullException>(model != null);
            Contract.Requires<ArgumentNullException>(instance != null);
            Contract.Requires<ArgumentNullException>(modelItem != null);
            Contract.Requires<ArgumentNullException>(position >= 1);
            Contract.Requires<ArgumentNullException>(size >= 1);

            this.model = model;
            this.instance = instance;
            this.modelItem = modelItem;
            this.position = position;
            this.size = size;
        }

        /// <summary>
        /// Model.
        /// </summary>
        public Model Model
        {
            get { return model; }
        }

        /// <summary>
        /// Instance within model.
        /// </summary>
        public Instance Instance
        {
            get { return instance; }
        }

        /// <summary>
        /// Node within instance.
        /// </summary>
        public ModelItem ModelItem
        {
            get { return modelItem; }
        }

        /// <summary>
        /// Position of node in a node set.
        /// </summary>
        public int Position
        {
            get { return position; }
        }

        /// <summary>
        /// Count of nodes in node set.
        /// </summary>
        public int Size
        {
            get { return size; }
        }

        /// <summary>
        /// Evaluates the given XPath expression.
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="expression"></param>
        /// <param name="resultType"></param>
        /// <returns></returns>
        internal object EvaluateXPath(XObject xml, string expression, XPathResultType resultType)
        {
            Contract.Requires<ArgumentNullException>(xml != null);
            Contract.Requires<ArgumentNullException>(xml.Host() != null);
            Contract.Requires<ArgumentNullException>(expression != null);

            var nc = new EvaluationXsltContext(xml, this);
            var nv = modelItem.CreateNavigator();
            var xp = XPathExpression.Compile(expression, nc);
            var nd = nv.Evaluate(xp);

            return ConvertXPath(nd, resultType);
        }

        /// <summary>
        /// Converts an XPath evaluation result into the specified type.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="resultType"></param>
        object ConvertXPath(object result, XPathResultType resultType)
        {
            if (result == null)
                return null;

            switch (resultType)
            {
                case XPathResultType.Number:
                    return Convert.ToDouble(result);
                case XPathResultType.Boolean:
                    return Convert.ToBoolean(result);
                case XPathResultType.String:
                    return Convert.ToString(result);
                default:
                    return result;
            }
        }

    }

}
