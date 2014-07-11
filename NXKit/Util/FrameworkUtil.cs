using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;

namespace NXKit.Util
{

    static class FrameworkUtil
    {

        /// <summary>
        /// Gets the framework name of the current runtime.
        /// </summary>
        /// <returns></returns>
        static string GetTargetFrameworkNameFromEntryAssembly()
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly != null)
                return entryAssembly
                    .GetCustomAttributes<TargetFrameworkAttribute>()
                    .Select(i => i.FrameworkName)
                    .DefaultIfEmpty(null)
                    .FirstOrDefault();

            return null;
        }

        /// <summary>
        /// Returns <c>true</c> if the current framework is compatible with NXKit.
        /// </summary>
        /// <returns></returns>
        public static bool IsCompatibleWithFramework()
        {
            var name = AppDomain.CurrentDomain.SetupInformation.TargetFrameworkName ?? GetTargetFrameworkNameFromEntryAssembly();
            if (string.IsNullOrEmpty(name))
                return false;

            return new FrameworkName(name).Version >= new Version(4, 5);
        }

    }

}
