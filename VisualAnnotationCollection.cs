using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

using NXKit.Util;

namespace NXKit
{

    public class VisualAnnotationCollection
    {

        readonly Dictionary<Type, VisualAnnotation> annotations;

        [ContractInvariantMethod]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
        void ObjectInvariant()
        {
            Contract.Invariant(annotations != null);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public VisualAnnotationCollection()
        {
            this.annotations = new Dictionary<Type, VisualAnnotation>();
        }

        /// <summary>
        /// Gets an annotation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Get<T>()
            where T : VisualAnnotation
        {
            return (T)annotations.GetOrDefault(typeof(T));
        }

        /// <summary>
        /// Sets an annotation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="annotation"></param>
        public void Set<T>(T annotation)
            where T : VisualAnnotation
        {
            annotations[typeof(T)] = annotation;
        }

    }

}
