using System;
using System.Collections;
using System.Collections.Generic;

using NXKit.Composition;

namespace NXKit
{

    /// <summary>
    /// Wraps a deferred extension lookup.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Export(typeof(ExtensionQuery<>), CompositionScope.Object)]
    public class ExtensionQuery<T> :
        ExtensionQueryInternal,
        IEnumerable<T>
        where T : class, IExtension
    {

        readonly ExtensionProvider<T> provider;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="value"></param>
        public ExtensionQuery(ExtensionProvider<T> provider)
        {
            this.provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public new IEnumerator<T> GetEnumerator()
        {
            return provider.Extensions.GetEnumerator();
        }

        protected override IEnumerator<object> GetGetEnumerator()
        {
            return (IEnumerator<object>)GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }

    public abstract class ExtensionQueryInternal :
        ExtensionQuery
    {

        protected abstract IEnumerator<object> GetGetEnumerator();

        public override IEnumerator<object> GetEnumerator()
        {
            return GetGetEnumerator();
        }

    }

    public abstract class ExtensionQuery :
        IEnumerable<object>
    {

        public abstract IEnumerator<object> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }

}
