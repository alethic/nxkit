using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;
using System.Xml.XPath;


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
        /// Initializes a new instance.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="position"></param>
        /// <param name="size"></param>
        internal EvaluationContext(ModelItem item, int position, int size)
            : this(item.Model, item.Instance, item, position, size)
        {
            Contract.Requires<ArgumentNullException>(item != null);
            Contract.Requires<ArgumentNullException>(position >= 1);
            Contract.Requires<ArgumentNullException>(size >= 1);
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
        /// Compiles the given XPath expression against the given 
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public XPathExpression CompileXPath(XObject xml, string expression)
        {
            Contract.Requires<ArgumentNullException>(xml != null);
            Contract.Requires<ArgumentNullException>(expression != null);

            var nc = new EvaluationXsltContext(xml, this);
            var xp = XPathExpression.Compile(expression, nc);

            return xp;
        }

        /// <summary>
        /// Evaluates the given XPath expression.
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="expression"></param>
        /// <param name="resultType"></param>
        /// <returns></returns>
        internal object EvaluateXPath(XObject xml, XPathExpression expression, XPathResultType resultType)
        {
            Contract.Requires<ArgumentNullException>(xml != null);
            Contract.Requires<ArgumentNullException>(expression != null);

            var nv = modelItem.CreateNavigator();
            var nd = nv.Evaluate(expression, new Iterator(position, size, nv));

            return ConvertXPath(nd, resultType);
        }

        /// <summary>
        /// Private <see cref="XPathNodeIterator"/> implementation that exposes the position and size context instances.
        /// </summary>
        class Iterator : XPathNodeIterator
        {

            readonly int position;
            readonly int size;
            readonly XPathNavigator n;

            public Iterator(int position, int size, XPathNavigator n)
            {
                this.position = position;
                this.size = size;
                this.n = n;
            }

            public override XPathNavigator Current
            {
                get { return n; }
            }

            public override int CurrentPosition
            {
                get { return position; }
            }

            public override int Count
            {
                get { return size; }
            }

            public override XPathNodeIterator Clone()
            {
                return new Iterator(position, size, n);
            }

            public override bool MoveNext()
            {
                throw new InvalidOperationException();
            }

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
            Contract.Requires<ArgumentNullException>(expression != null);

            return EvaluateXPath(xml, CompileXPath(xml, expression), resultType);
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
