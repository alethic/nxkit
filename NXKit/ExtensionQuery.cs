using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace NXKit
{

    /// <summary>
    /// Wraps a deferred extension lookup.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ExtensionQuery<T> :
        ExtensionQueryInternal,
        IEnumerable<T>
    {

        readonly Lazy<IEnumerable<T>> values;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="value"></param>
        public ExtensionQuery(Func<IEnumerable<T>> values)
        {
            Contract.Requires<ArgumentNullException>(values != null);

            this.values = new Lazy<IEnumerable<T>>(() => values());
        }

        public new IEnumerator<T> GetEnumerator()
        {
            Contract.Assume(values.Value != null);

            return values.Value.GetEnumerator();
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
