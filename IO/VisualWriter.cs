using System;
using System.Diagnostics.Contracts;

namespace NXKit.Web
{

    /// <summary>
    /// Provides a writer that produces output from <see cref="Visual"/> instances.
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
        /// Writes the given <see cref="Visual"/> to the underlying output.
        /// </summary>
        /// <param name="visual"></param>
        public abstract void Write(Visual visual);

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

        public override void Write(Visual visual)
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
