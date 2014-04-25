using System.Diagnostics;

namespace NXKit.IO
{

    /// <summary>
    /// Describes the type of request being issued.
    /// </summary>
    [DebuggerDisplay("{name}")]
    public struct IOMethod
    {

        public static readonly IOMethod None = "";
        public static readonly IOMethod Get = "GET";
        public static readonly IOMethod Put = "PUT";
        public static readonly IOMethod Post = "POST";
        public static readonly IOMethod Delete = "DELETE";

        public static implicit operator string(IOMethod method)
        {
            return method.ToString();
        }

        public static implicit operator IOMethod(string method)
        {
            return new IOMethod(method);
        }


        readonly string name;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        IOMethod(string name)
        {
            this.name = name;
        }

        public override string ToString()
        {
            return name;
        }

    }

}
