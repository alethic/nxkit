using System;
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

            var ecs = node.InterfaceOrDefault<IEvaluationContextScope>();
            if (ecs != null &&
                ecs.Context != null)
                return ecs.Context;

            var nec = node.InterfaceOrDefault<NodeEvaluationContext>();
            if (nec != null &&
                nec.Context != null)
                return nec.Context;

            return null;
        }

    }

}
