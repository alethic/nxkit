using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Xml.Linq;

using NXKit.Util;

namespace NXKit.XForms.Serialization
{

    /// <summary>
    /// Defines a deserializer capable of deserializing a body into a <see cref="XNode"/>.
    /// </summary>
    [ContractClass(typeof(INodeDeserializer_Contract))]
    public interface INodeDeserializer
    {

        /// <summary>
        /// Returns <c>true</c> if the deserializer can serialize the given <see cref="MediaRange"/>.
        /// </summary>
        /// <param name="mediaType"></param>
        /// <returns></returns>
        Priority CanDeserialize(MediaRange mediaType);

        /// <summary>
        /// Gets the resulting deserialized node.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="mediaType"></param>
        /// <returns></returns>
        XNode Deserialize(TextReader reader, MediaRange mediaType);

    }

    [ContractClassFor(typeof(INodeDeserializer))]
    abstract class INodeDeserializer_Contract :
        INodeDeserializer
    {

        public Priority CanDeserialize(MediaRange mediaType)
        {
            Contract.Requires<ArgumentNullException>(mediaType != null);
            throw new NotImplementedException();
        }

        public XNode Deserialize(TextReader reader, MediaRange mediaType)
        {
            Contract.Requires<ArgumentNullException>(reader != null);
            Contract.Requires<ArgumentNullException>(mediaType != null);
            throw new NotImplementedException();
        }

    }

}
