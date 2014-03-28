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
    [ContractClass(typeof(NodeWriter_Contract))]
    public abstract class NodeWriter :
        IDisposable
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        protected NodeWriter()
        {

        }

        /// <summary>
        /// Writes the given <see cref="NXNode"/> to the underlying output.
        /// </summary>
        /// <param name="node"></param>
        public abstract void Write(NXNode node);

        /// <summary>
        /// Gets the type of the specified <see cref="NXNode"/>.
        /// </summary>
        /// <param name="visual"></param>
        /// <returns></returns>
        protected virtual Type GetNodeType(NXNode visual)
        {
            Contract.Requires<ArgumentNullException>(visual != null);

            return TypeDescriptor.GetReflectionType(visual);
        }

        /// <summary>
        /// Gets the base types of the specified <see cref="NXNode"/>.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected virtual Type[] GetNodeBaseTypes(NXNode node)
        {
            Contract.Requires<ArgumentNullException>(node != null);

            return TypeDescriptor.GetReflectionType(node).BaseType
                .Recurse(i => i.BaseType)
                .TakeWhile(i => typeof(NXNode).IsAssignableFrom(i))
                .ToArray();
        }

        /// <summary>
        /// Gets the properties of the specified <see cref="NXNode"/>.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected PropertyDescriptor[] GetNodeProperties(NXNode node)
        {
            Contract.Requires<ArgumentNullException>(node != null);

            return TypeDescriptor.GetProperties(node)
                .Cast<PropertyDescriptor>()
                .Where(i => i.Attributes.OfType<PublicAttribute>().Any())
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

    [ContractClassFor(typeof(NodeWriter))]
    abstract class NodeWriter_Contract :
        NodeWriter
    {

        public override void Write(NXNode node)
        {
            Contract.Requires<ArgumentNullException>(node != null);
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
