using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Xml.Linq;

using NXKit.IO.Media;

namespace NXKit.XForms.Serialization
{

    /// <summary>
    /// Defines a deserializer capable of deserializing a body into a <see cref="XNode"/>.
    /// </summary>
    [ContractClass(typeof(IModelDeserializer_Contract))]
    public interface IModelDeserializer
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
        XDocument Deserialize(TextReader reader, MediaRange mediaType);

    }

    [ContractClassFor(typeof(IModelDeserializer))]
    abstract class IModelDeserializer_Contract :
        IModelDeserializer
    {

        public Priority CanDeserialize(MediaRange mediaType)
        {
            Contract.Requires<ArgumentNullException>(mediaType != null);
            throw new NotImplementedException();
        }

        public XDocument Deserialize(TextReader reader, MediaRange mediaType)
        {
            Contract.Requires<ArgumentNullException>(reader != null);
            Contract.Requires<ArgumentNullException>(mediaType != null);
            throw new NotImplementedException();
        }

    }

}
