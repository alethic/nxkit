using System;
using System.Diagnostics.Contracts;

namespace NXKit
{

    public class NXObjectEventArgs :
        EventArgs
    {

        readonly NXObject @object;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="object"></param>
        public NXObjectEventArgs(NXObject @object)
        {
            Contract.Requires<ArgumentNullException>(@object != null);

            this.@object = @object;
        }

        public NXObject Object
        {
            get { return @object; }
        }

    }

}
