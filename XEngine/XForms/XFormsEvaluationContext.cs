using System;
using System.Xml.Linq;

namespace NXKit.XForms
{

    public class XFormsEvaluationContext
    {

        /// <summary>
        /// Manages the life-time of the evaluation context scope.
        /// </summary>
        public sealed class ThreadScope : IDisposable
        {

            internal ThreadScope(XFormsEvaluationContext context)
            {
                XFormsEvaluationContext.current = context;
            }


            public void Dispose()
            {
                XFormsEvaluationContext.current = null;
            }
        }

        [ThreadStatic]
        private static XFormsEvaluationContext current;

        /// <summary>
        /// Gets the currently in-scope evaluation context.
        /// </summary>
        public static XFormsEvaluationContext Current
        {
            get { return current; }
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        internal XFormsEvaluationContext(XFormsModelVisual model, XFormsInstanceVisual instance, XObject node, int position, int size)
        {
            Model = model;
            Instance = instance;
            Node = node;
            Position = position;
            Size = size;
        }

        public XFormsModelVisual Model { get; private set; }

        public XFormsInstanceVisual Instance { get; private set; }

        public XObject Node { get; private set; }

        public int Position { get; private set; }

        public int Size { get; private set; }

        /// <summary>
        /// Puts the context into thread local scope. Dipose of this instance to remove it.
        /// </summary>
        /// <returns></returns>
        public ThreadScope Scope()
        {
            return new ThreadScope(this);
        }

    }

}
