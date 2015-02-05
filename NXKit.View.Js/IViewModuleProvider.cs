using System.Linq;

namespace NXKit.View.Js
{

    /// <summary>
    /// Provides resolution of <see cref="ViewModuleDependency"/> items.
    /// </summary>
    public interface IViewModuleProvider
    {

        /// <summary>
        /// Resolves the given <see cref="ViewModuleWriter"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IQueryable<ViewModuleInfo> GetViewModules();

    }

}
