using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Xml.Linq;
using NXKit.Util;

namespace NXKit.XForms
{

    /// <summary>
    /// Defines a serializer capable of serializing a model item into a body.
    /// </summary>
    [ContractClass(typeof(ISubmissionSerializer_Contract))]
    public interface ISubmissionSerializer
    {

        /// <summary>
        /// Returns <c>true</c> if the serializer can serialize the given <see cref="XNode"/> to the given <see cref="MediaRange"/>.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mediaRange"></param>
        /// <returns></returns>
        bool CanSerialize(XNode node, MediaRange mediaRange);

        /// <summary>
        /// Gets the resulting serialized data stream.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="node"></param>
        void Serialize(TextWriter writer, XNode node);

    }

    [ContractClassFor(typeof(ISubmissionSerializer))]
    abstract class ISubmissionSerializer_Contract :
        ISubmissionSerializer
    {

        public bool CanSerialize(XNode node, MediaRange mediaRange)
        {
            Contract.Requires<ArgumentNullException>(node != null);
            Contract.Requires<ArgumentNullException>(mediaRange != null);
            throw new NotImplementedException();
        }

        public void Serialize(TextWriter writer, XNode node)
        {
            Contract.Requires<ArgumentNullException>(node != null);
            Contract.Requires<ArgumentNullException>(writer != null);
            throw new System.NotImplementedException();
        }

    }

}
