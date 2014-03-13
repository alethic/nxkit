using System.Xml.Linq;

namespace NXKit.XForms
{

    /// <summary>
    /// Describes a context against which bindings can be applied.
    /// </summary>
    public class XFormsEvaluationContext
    {

        readonly XFormsModelVisual model;
        readonly XFormsInstanceVisual instance;
        readonly XObject node;
        readonly int position;
        readonly int size;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="instance"></param>
        /// <param name="node"></param>
        /// <param name="position"></param>
        /// <param name="size"></param>
        internal XFormsEvaluationContext(XFormsModelVisual model, XFormsInstanceVisual instance, XObject node, int position, int size)
        {
            this.model = model;
            this.instance = instance;
            this.node = node;
            this.position = position;
            this.size = size;
        }

        /// <summary>
        /// Model.
        /// </summary>
        public XFormsModelVisual Model
        {
            get { return model; }
        }

        /// <summary>
        /// Instance within model.
        /// </summary>
        public XFormsInstanceVisual Instance
        {
            get { return instance; }
        }

        /// <summary>
        /// Node within instance.
        /// </summary>
        public XObject Node
        {
            get { return node; }
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

    }

}
