using System;
using System.Linq;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides various extension methods for <see cref="NXNode"/> instances.
    /// </summary>
    public static class NXNodeExtensions
    {

        /// <summary>
        /// Gets the <see cref="EvaluationContext"/> provided by the node, or consumed by the node.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static EvaluationContext ResolveEvaluationContext(this XNode node)
        {
            Contract.Requires<ArgumentNullException>(node != null);
            Contract.Requires<ArgumentNullException>(node.Host() != null);

            var ecs = node.Interfaces<IEvaluationContextScope>()
                .Select(i => i.Context)
                .FirstOrDefault(i => i != null);
            if (ecs != null)
                return ecs;

            var nec = node.Interfaces<NodeEvaluationContext>()
                .Select(i => i.Context)
                .FirstOrDefault(i => i != null);
            if (nec != null)
                return nec;

            return null;
        }

    }

}
