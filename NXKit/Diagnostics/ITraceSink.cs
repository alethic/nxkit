using System;
using System.Diagnostics.Contracts;

namespace NXKit.Diagnostics
{

    /// <summary>
    /// Provides an interface that receives log messages.
    /// </summary>
    [ContractClass(typeof(ITraceSink_Contract))]
    public interface ITraceSink
    {

        /// <summary>
        /// Logs the specified object.
        /// </summary>
        /// <param name="data"></param>
        void Debug(object data);

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message"></param>
        void Debug(string message);

        /// <summary>
        /// Logs the specified formatted message.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        void Debug(string format, params object[] args);

        /// <summary>
        /// Logs the specified object.
        /// </summary>
        /// <param name="data"></param>
        void Information(object data);

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message"></param>
        void Information(string message);

        /// <summary>
        /// Logs the specified formatted message.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        void Information(string format, params object[] args);

        /// <summary>
        /// Logs the specified warning object.
        /// </summary>
        /// <param name="data"></param>
        void Warning(object data);

        /// <summary>
        /// Logs the specified warning message.
        /// </summary>
        /// <param name="message"></param>
        void Warning(string message);

        /// <summary>
        /// Logs the specified formatted warning message.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        void Warning(string format, params object[] args);

    }

    [ContractClassFor(typeof(ITraceSink))]
    public abstract class ITraceSink_Contract :
        ITraceSink
    {

        public void Debug(object data)
        {
            Contract.Requires<ArgumentNullException>(data != null);
            throw new NotImplementedException();
        }

        public void Debug(string message)
        {
            Contract.Requires<ArgumentNullException>(message != null);
            throw new NotImplementedException();
        }

        public void Debug(string format, params object[] args)
        {
            Contract.Requires<ArgumentNullException>(format != null);
            Contract.Requires<ArgumentNullException>(args != null);
            throw new NotImplementedException();
        }

        public void Information(object data)
        {
            Contract.Requires<ArgumentNullException>(data != null);
            throw new NotImplementedException();
        }

        public void Information(string message)
        {
            Contract.Requires<ArgumentNullException>(message != null);
            throw new NotImplementedException();
        }

        public void Information(string format, params object[] args)
        {
            Contract.Requires<ArgumentNullException>(format != null);
            Contract.Requires<ArgumentNullException>(args != null);
            throw new NotImplementedException();
        }

        public void Warning(object data)
        {
            Contract.Requires<ArgumentNullException>(data != null);
            throw new NotImplementedException();
        }

        public void Warning(string message)
        {
            Contract.Requires<ArgumentNullException>(message != null);
            throw new NotImplementedException();
        }

        public void Warning(string format, params object[] args)
        {
            Contract.Requires<ArgumentNullException>(format != null);
            Contract.Requires<ArgumentNullException>(args != null);
            throw new NotImplementedException();
        }

    }

}
