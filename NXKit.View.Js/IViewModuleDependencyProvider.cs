using System.Collections.Generic;
using System.Xml.Linq;

namespace NXKit.View.Js
{

    /// <summary>
    /// Provides <see cref="ViewModuleDependency"/>s for elements.
    /// </summary>
    public interface IViewModuleDependencyProvider
    {

        IEnumerable<ViewModuleDependency> GetDependencies(XObject obj);

    }

}
