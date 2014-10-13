using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace NXKit.View.Js
{

    [Export(typeof(IViewModuleDependencyProvider))]
    public class ViewModuleDependencyProvider :
        IViewModuleDependencyProvider
    {

        static readonly ViewModuleDependency[] DEPENDENCIES = new[]
        {
            new ViewModuleDependency(ViewModuleType.Script, "nxkit"),
            new ViewModuleDependency(ViewModuleType.Css, "nxkit.css"),
            new ViewModuleDependency(ViewModuleType.Template, "nxkit.html"),
        };

        public IEnumerable<ViewModuleDependency> GetDependencies(XObject obj)
        {
            if (obj.Document.Root == obj ||
                obj.NodeType == XmlNodeType.Text)
                return DEPENDENCIES;
            else
                return Enumerable.Empty<ViewModuleDependency>();
        }

    }

}
