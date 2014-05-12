using System;

namespace NXKit.DOM
{

    public class DOMException :
        Exception
    {

        public const ushort INDEX_SIZE_ERR = 1;
        public const ushort DOMSTRING_SIZE_ERR = 2; // historical
        public const ushort HIERARCHY_REQUEST_ERR = 3;
        public const ushort WRONG_DOCUMENT_ERR = 4;
        public const ushort INVALID_CHARACTER_ERR = 5;
        public const ushort NO_DATA_ALLOWED_ERR = 6; // historical
        public const ushort NO_MODIFICATION_ALLOWED_ERR = 7;
        public const ushort NOT_FOUND_ERR = 8;
        public const ushort NOT_SUPPORTED_ERR = 9;
        public const ushort INUSE_ATTRIBUTE_ERR = 10; // historical
        public const ushort INVALID_STATE_ERR = 11;
        public const ushort SYNTAX_ERR = 12;
        public const ushort INVALID_MODIFICATION_ERR = 13;
        public const ushort NAMESPACE_ERR = 14;
        public const ushort INVALID_ACCESS_ERR = 15;
        public const ushort VALIDATION_ERR = 16; // historical
        public const ushort TYPE_MISMATCH_ERR = 17; // historical; use JavaScript's TypeError instead
        public const ushort SECURITY_ERR = 18;
        public const ushort NETWORK_ERR = 19;
        public const ushort ABORT_ERR = 20;
        public const ushort URL_MISMATCH_ERR = 21;
        public const ushort QUOTA_EXCEEDED_ERR = 22;
        public const ushort TIMEOUT_ERR = 23;
        public const ushort INVALID_NODE_TYPE_ERR = 24;
        public const ushort DATA_CLONE_ERR = 25;

        readonly ushort code;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public DOMException()
        {
            this.code = 0;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public DOMException(ushort code)
        {
            this.code = 0;
        }

        public ushort Code
        {
            get { return code; }
        }

    }

}
