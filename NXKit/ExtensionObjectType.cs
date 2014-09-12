using System;

namespace NXKit
{

    [Flags]
    public enum ExtensionObjectType
    {

        /// <summary>
        /// Extension applies to <see cref="XDocument"/> nodes.
        /// </summary>
        Document = 1,

        /// <summary>
        /// Extension applies to <see cref="XElement"/> nodes.
        /// </summary>
        Element = 2,

        /// <summary>
        /// Extension applies to <see cref="XText"/> nodes.
        /// </summary>
        Text = 4,

        /// <summary>
        /// Extension applies to <see cref="XAttribute"/> nodes.
        /// </summary>
        Attribute = 8,

    }

}
