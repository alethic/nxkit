using System;
using System.IO;

using NXKit.IO.Media;

namespace NXKit.View.Js
{

    public class ViewModuleInfo
    {

        readonly string name;
        readonly Action<Stream> write;
        readonly MediaRange contentType;
        readonly DateTime lastModifiedTime;
        readonly string etag;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="write"></param>
        /// <param name="contentType"></param>
        public ViewModuleInfo(string name, Action<Stream> write, MediaRange contentType)
        {
            this.name = name ?? throw new ArgumentNullException(nameof(name));
            this.write = write ?? throw new ArgumentNullException(nameof(write));
            this.contentType = contentType ?? throw new ArgumentNullException(nameof(contentType));
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
            if (name is null)
                throw new ArgumentNullException(nameof(name));
            if (writer is null)
                throw new ArgumentNullException(nameof(writer));
            if (contentType is null)
                throw new ArgumentNullException(nameof(contentType));

            this.lastModifiedTime = lastModifiedTime;
            this.etag = etag;
        }

        public string Name
        {
            get { return name; }
        }

        public virtual Action<Stream> Write
        {
            get { return write; }
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
