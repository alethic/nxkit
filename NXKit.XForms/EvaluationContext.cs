using System;
using System.Diagnostics.Contracts;

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

    }

}
