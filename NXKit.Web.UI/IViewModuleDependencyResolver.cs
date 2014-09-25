using NXKit.View.Js;

namespace NXKit.Web.UI
{

    /// <summary>
    /// Provides resolution of <see cref="ViewModuleDependency"/> items.
    /// </summary>
    public interface IViewModuleDependencyResolver
    {

        /// <summary>
        /// Resolves the given <see cref="ViewModuleDependency"/>.
        /// </summary>
        /// <param name="dependency"></param>
        /// <returns></returns>
        object Resolve(ViewModuleDependency dependency);

    }

}
