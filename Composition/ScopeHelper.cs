using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

using NXKit.Util;

namespace NXKit.Composition
{

    static class ScopeHelper
    {

        /// <summary>
        /// Returns <c>true</c> if the metadata expresses the appropriate scope.
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        public static bool IsScoped(IDictionary<string, object> metadata, Scope scope)
        {
            Contract.Requires<ArgumentNullException>(metadata != null);

            var data = metadata.GetOrDefault("Scope");
            if (data == null)
                return scope == Scope.Global;

            if (data is Scope)
                return (Scope)data == scope;

            var array = data as IEnumerable<Scope>;
            if (array != null)
                return array.Any(i => i == scope);

            return false;
        }

    }

}
