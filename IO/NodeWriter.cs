using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

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
        /// Writes the given <see cref="XNode"/> to the underlying output.
        /// </summary>
        /// <param name="node"></param>
        public abstract void Write(XNode node);

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

        public override void Write(XNode node)
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
