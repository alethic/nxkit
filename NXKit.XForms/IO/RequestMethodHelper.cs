using System;

namespace NXKit.XForms.IO
{

    public static class RequestMethodHelper
    {

        /// <summary>
        /// Parses a string into a <see cref="RequestMethod"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static RequestMethod Parse(string value)
        {
            switch (value.ToLowerInvariant())
            {
                case "get":
                    return RequestMethod.Get;
                case "put":
                    return RequestMethod.Put;
                case "post":
                    return RequestMethod.Post;
                case "delete":
                    return RequestMethod.Delete;
                case "urlencoded-post":
                    return RequestMethod.UrlEncodedPost;
                case "multipart-post":
                    return RequestMethod.MultipartPost;
                case "formdata-post":
                    return RequestMethod.FormDataPost;
            }

            throw new FormatException();
        }

    }

}
