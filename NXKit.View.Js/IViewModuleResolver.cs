using System.Collections.Generic;

namespace NXKit.View.Js
{

    /// <summary>
    /// Provides resolution of <see cref="ViewModuleDependency"/> items.
    /// </summary>
    public interface IViewModuleResolver
    {

        /// <summary>
        /// Resolves the given <see cref="ViewModuleWriter"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IEnumerable<ViewModuleInfo> Resolve(string name);

    }

}
