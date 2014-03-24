using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using NXKit.Util;

namespace NXKit.Web
{

    /// <summary>
    /// Provides a writer that produces output from <see cref="NXNode"/> instances.
    /// </summary>
    [ContractClass(typeof(VisualWriter_Contract))]
    public abstract class VisualWriter :
        IDisposable
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        protected VisualWriter()
        {

        }

        /// <summary>
        /// Writes the given <see cref="NXNode"/> to the underlying output.
        /// </summary>
        /// <param name="visual"></param>
        public abstract void Write(NXNode visual);

        /// <summary>
        /// Gets the type of the specified <see cref="NXNode"/>.
        /// </summary>
        /// <param name="visual"></param>
        /// <returns></returns>
        protected virtual Type GetVisualType(NXNode visual)
        {
            Contract.Requires<ArgumentNullException>(visual != null);

            return TypeDescriptor.GetReflectionType(visual);
        }

        /// <summary>
        /// Gets the base types of the specified <see cref="NXNode"/>.
        /// </summary>
        /// <param name="visual"></param>
        /// <returns></returns>
        protected virtual Type[] GetVisualBaseTypes(NXNode visual)
        {
            Contract.Requires<ArgumentNullException>(visual != null);

            return TypeDescriptor.GetReflectionType(visual).BaseType
                .Recurse(i => i.BaseType)
                .TakeWhile(i => typeof(NXNode).IsAssignableFrom(i))
                .ToArray();
        }

        /// <summary>
        /// Gets the properties of the specified <see cref="NXNode"/>.
        /// </summary>
        /// <param name="visual"></param>
        /// <returns></returns>
        protected PropertyDescriptor[] GetVisualProperties(NXNode visual)
        {
            Contract.Requires<ArgumentNullException>(visual != null);

            return TypeDescriptor.GetProperties(visual)
                .Cast<PropertyDescriptor>()
                .Where(i => i.Attributes.OfType<InteractiveAttribute>().Any())
                .ToArray();
        }

        /// <summary>
        /// Flushes whatever is in the buffer to the underlying streams.
        /// </summary>
        public abstract void Flush();

        /// <summary>
        /// Closes this writer and its underlying stream.
        /// </summary>
        public abstract void Close();

        /// <summary>
        /// Closes this writer and its underlying stream.
        /// </summary>
        public abstract void Dispose();

    }

    [ContractClassFor(typeof(VisualWriter))]
    abstract class VisualWriter_Contract :
        VisualWriter
    {

        public override void Write(NXNode visual)
        {
            Contract.Requires<ArgumentNullException>(visual != null);
            throw new NotImplementedException();
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override void Close()
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }

    }

}
