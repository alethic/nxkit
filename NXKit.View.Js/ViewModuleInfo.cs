using System;
using System.Diagnostics.Contracts;
using System.IO;

using NXKit.IO.Media;

namespace NXKit.View.Js
{

    public class ViewModuleInfo
    {

        readonly string name;
        readonly Action<Stream> writer;
        readonly MediaRange contentType;
        readonly DateTime lastModifiedTime;
        readonly string etag;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="writer"></param>
        /// <param name="contentType"></param>
        public ViewModuleInfo(string name, Action<Stream> writer, MediaRange contentType)
        {
            Contract.Requires<ArgumentNullException>(name != null);
            Contract.Requires<ArgumentNullException>(writer != null);
            Contract.Requires<ArgumentNullException>(contentType != null);

            this.name = name;
            this.writer = writer;
            this.contentType = contentType;
            this.lastModifiedTime = DateTime.UtcNow;
            this.etag = null;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="contentType"></param>
        /// <param name="lastModifiedTime"></param>
        /// <param name="etag"></param>
        public ViewModuleInfo(string name, Action<Stream> writer, MediaRange contentType, DateTime lastModifiedTime, string etag)
            : this(name, writer, contentType)
        {
            Contract.Requires<ArgumentNullException>(name != null);
            Contract.Requires<ArgumentNullException>(writer != null);
            Contract.Requires<ArgumentNullException>(contentType != null);

            this.lastModifiedTime = lastModifiedTime;
            this.etag = etag;
        }

        public string Name
        {
            get { return name; }
        }

        public virtual Action<Stream> Writer
        {
            get { return writer; }
        }

        public MediaRange ContentType
        {
            get { return contentType; }
        }

        public virtual DateTime LastModifiedTime
        {
            get { return lastModifiedTime; }
        }

        public virtual string ETag
        {
            get { return etag; }
        }

    }

}
