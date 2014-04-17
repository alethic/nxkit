﻿using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Xml.Linq;

using NXKit.Util;

namespace NXKit.Serialization
{

    /// <summary>
    /// Defines a serializer capable of serializing a model item into a body.
    /// </summary>
    [ContractClass(typeof(INodeSerializer_Contract))]
    public interface INodeSerializer
    {

        /// <summary>
        /// Returns <c>true</c> if the serializer can serialize the given <see cref="XNode"/> to the given <see cref="MediaRange"/>.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mediaRange"></param>
        /// <returns></returns>
        Priority CanSerialize(XNode node, MediaRange mediaRange);

        /// <summary>
        /// Gets the resulting serialized data stream.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="node"></param>
        /// <param name="mediaType"></param>
        void Serialize(TextWriter writer, XNode node, MediaRange mediaType);

    }

    [ContractClassFor(typeof(INodeSerializer))]
    abstract class INodeSerializer_Contract :
        INodeSerializer
    {

        public Priority CanSerialize(XNode node, MediaRange mediaType)
        {
            Contract.Requires<ArgumentNullException>(node != null);
            Contract.Requires<ArgumentNullException>(mediaType != null);
            throw new NotImplementedException();
        }

        public void Serialize(TextWriter writer, XNode node, MediaRange mediaType)
        {
            Contract.Requires<ArgumentNullException>(node != null);
            Contract.Requires<ArgumentNullException>(writer != null);
            Contract.Requires<ArgumentNullException>(mediaType != null);
            throw new System.NotImplementedException();
        }

    }

}
