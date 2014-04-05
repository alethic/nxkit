using System;
using System.Diagnostics.Contracts;

namespace NXKit.Util
{

    public class AuthorityUri :
        DisposableUri
    {

        readonly DynamicUriAuthority authority;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="authority"></param>
        internal AuthorityUri(DynamicUriAuthority authority)
            : base(authority.BaseUri)
        {
            Contract.Requires<ArgumentNullException>(authority != null);

            this.authority = authority;
        }
        public override void Dispose()
        {
            authority.Dispose();
        }

    }

}
