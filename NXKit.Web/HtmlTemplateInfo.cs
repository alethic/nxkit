using System;
using System.Diagnostics.Contracts;
using System.IO;

namespace NXKit.Web
{

    /// <summary>
    /// Describes an available template.
    /// </summary>
    public class HtmlTemplateInfo
    {

        readonly string name;
        readonly Func<Stream> openFunc;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="openFunc"></param>
        public HtmlTemplateInfo(string name, Func<Stream> openFunc)
        {
            Contract.Requires<ArgumentNullException>(name != null);
            Contract.Requires<ArgumentNullException>(openFunc != null);

            this.name = name;
            this.openFunc = openFunc;
        }

        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// Opens a new copy of the template sources.
        /// </summary>
        /// <returns></returns>
        public Stream Open()
        {
            return openFunc();
        }

    }

}
