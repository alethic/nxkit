using System;
using System.Diagnostics.Contracts;

namespace NXKit
{

    /// <summary>
    /// Wraps a deferred extension lookup.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Extension<T> :
        ExtensionInternal
    {

        readonly Func<T> value;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="value"></param>
        public Extension(Func<T> value)
        {
            Contract.Requires<ArgumentNullException>(value != null);

            this.value = value;
        }

        /// <summary>
        /// Gets the extension value.
        /// </summary>
        public new T Value
        {
            get { return value(); }
        }

        protected override object GetValue()
        {
            return Value;
        }

    }

    public abstract class ExtensionInternal :
        Extension
    {

        protected abstract object GetValue();

        public override object Value
        {
            get { return GetValue(); }
        }

    }

    public abstract class Extension
    {

        public abstract object Value { get; }

    }

}
