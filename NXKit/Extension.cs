using System;
using System.Diagnostics;
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

        readonly Lazy<T> value;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="value"></param>
        public Extension(Func<T> value)
        {
            Contract.Requires<ArgumentNullException>(value != null);

            this.value = new Lazy<T>(() => value());
        }

        /// <summary>
        /// Gets the extension value.
        /// </summary>
        [DebuggerHidden]
        public new T Value
        {
            get { return value.Value; }
        }

        [DebuggerHidden]
        protected override object GetValue()
        {
            return Value;
        }

    }

    public abstract class ExtensionInternal :
        Extension
    {

        protected abstract object GetValue();

        [DebuggerHidden]
        public override object Value
        {
            get { return GetValue(); }
        }

    }

    public abstract class Extension
    {

        [DebuggerHidden]
        public abstract object Value { get; }

    }

}
