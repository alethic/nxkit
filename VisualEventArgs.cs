using System;
using System.Diagnostics.Contracts;

namespace NXKit
{

    public class VisualEventArgs :
        EventArgs
    {

        readonly Visual visual;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="visual"></param>
        public VisualEventArgs(Visual visual)
        {
            Contract.Requires<ArgumentNullException>(visual != null);

            this.visual = visual;
        }

        public Visual Visual
        {
            get { return visual; }
        }

    }

}
