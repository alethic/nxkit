using System;
using System.Web;

using NXKit.View.Js;

namespace NXKit.Web.UI
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
        Action<HttpResponse> Resolve(string name);

    }

}
