using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Xml.Linq;
using NXKit.Util;

namespace NXKit.XForms
{

    /// <summary>
    /// Defines a deserializer capable of deserializing a body into a <see cref="XNode"/>.
    /// </summary>
    [ContractClass(typeof(ISubmissionDeserializer_Contract))]
    public interface ISubmissionDeserializer
    {

        /// <summary>
        /// Returns <c>true</c> if the deserializer can serialize the given <see cref="MediaRange"/>.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mediaRange"></param>
        /// <returns></returns>
        bool CanDeserialize(MediaRange mediaRange);

        /// <summary>
        /// Gets the resulting deserialized node.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        XNode Deserialize(TextReader reader);

    }

    [ContractClassFor(typeof(ISubmissionDeserializer))]
    abstract class ISubmissionDeserializer_Contract :
        ISubmissionDeserializer
    {

        public bool CanDeserialize(MediaRange mediaRange)
        {
            Contract.Requires<ArgumentNullException>(mediaRange != null);
            throw new NotImplementedException();
        }

        public XNode Deserialize(TextReader reader)
        {
            Contract.Requires<ArgumentNullException>(reader != null);
            throw new NotImplementedException();
        }

    }

}
