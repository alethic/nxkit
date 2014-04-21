using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

using NXKit.Util;

namespace NXKit.Net
{

    /// <summary>
    /// Provides for registration of dynamic URI authorities.
    /// </summary>
    static class DynamicUriRegistry
    {

        static Dictionary<Guid, WeakReference<DynamicUriAuthority>> authorities =
            new Dictionary<Guid, WeakReference<DynamicUriAuthority>>();

        /// <summary>
        /// Registers the given authority.
        /// </summary>
        /// <param name="authority"></param>
        internal static void Register(DynamicUriAuthority authority)
        {
            Contract.Requires<ArgumentNullException>(authority != null);

            if (authorities.ContainsKey(authority.Id))
                throw new InvalidOperationException();

            lock (authorities)
                authorities.Add(authority.Id, new WeakReference<DynamicUriAuthority>(authority));
        }

        /// <summary>
        /// Unregisters the given authority.
        /// </summary>
        /// <param name="authority"></param>
        internal static void Unregister(DynamicUriAuthority authority)
        {
            Contract.Requires<ArgumentNullException>(authority != null);

            if (!authorities.ContainsKey(authority.Id))
                throw new InvalidOperationException();
            lock (authorities)
                authorities.Remove(authority.Id);
        }

        /// <summary>
        /// Gets the given authority.
        /// </summary>
        /// <param name="id"></param>
        internal static DynamicUriAuthority Get(Guid id)
        {
            Contract.Requires<ArgumentException>(id != Guid.Empty);

            lock (authorities)
            {
                DynamicUriAuthority a = null;

                var w = authorities.GetOrDefault(id);
                if (w != null)
                    w.TryGetTarget(out a);

                return a;
            }
        }

    }

}
