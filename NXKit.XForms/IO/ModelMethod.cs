using System;

namespace NXKit.XForms.IO
{

    /// <summary>
    /// Describes the type of request being issued.
    /// </summary>
    public struct ModelMethod
    {

        public static readonly ModelMethod Get = "get";
        public static readonly ModelMethod Put = "put";
        public static readonly ModelMethod Post = "post";
        public static readonly ModelMethod Delete = "delete";
        public static readonly ModelMethod MultipartPost = "multipart-post";
        public static readonly ModelMethod FormDataPost = "form-data-post";
        public static readonly ModelMethod UrlEncodedPost = "urlencoded-post";

        public static implicit operator string(ModelMethod method)
        {
            return method.ToString();
        }

        public static implicit operator ModelMethod(string method)
        {
            return new ModelMethod(method);
        }


        readonly string name;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        ModelMethod(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("", nameof(name));

            this.name = name;
        }

        public override string ToString()
        {
            return name;
        }

    }

}
