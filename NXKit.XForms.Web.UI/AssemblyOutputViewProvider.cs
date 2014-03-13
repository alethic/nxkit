using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Web.UI;

using NXKit.Web.UI;

namespace NXKit.XForms.Web.UI
{

    /// <summary>
    /// Provides an <see cref="IOutputViewProvider"/> implementation that scans a given <see cref="Assembly"/>.
    /// </summary>
    public class AssemblyOutputViewProvider :
        IOutputViewProvider
    {

        readonly Assembly assembly;
        readonly Type[] types;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="assembly"></param>
        public AssemblyOutputViewProvider(Assembly assembly)
        {
            Contract.Requires<ArgumentNullException>(assembly != null);

            this.assembly = assembly;
            this.types = assembly.GetTypes()
                .Where(i => i.IsClass && !i.IsAbstract && !i.IsGenericTypeDefinition)
                .Where(i => typeof(IOutputView).IsAssignableFrom(i))
                .OrderByDescending(i => PriorityAttribute.GetPriority(i))
                .ToArray();
        }

        public Control Create(View view, XFormsOutputVisual visual)
        {
            // scan assembly for marked type
            return assembly.GetTypes()
                .Where(i => typeof(IOutputView).IsAssignableFrom(i))
                .Where(i => XFormsXsdTypeAttribute.Predicate(i, visual.Binding))
                .Select(i => CreateView(i, view, visual))
                .FirstOrDefault(i => i != null);
        }

        /// <summary>
        /// Attempts to instantiate the given editable type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="view"></param>
        /// <param name="visual"></param>
        /// <returns></returns>
        Control CreateView(Type type, View view, XFormsOutputVisual visual)
        {
            Contract.Requires<ArgumentNullException>(type != null);
            Contract.Requires<ArgumentNullException>(view != null);
            Contract.Requires<ArgumentNullException>(visual != null);
            Contract.Requires<ArgumentException>(typeof(IOutputView).IsAssignableFrom(type));

            var ctor1 = type.GetConstructors()
                .FirstOrDefault(i =>
                    i.GetParameters()[0].ParameterType.IsAssignableFrom(typeof(View)) &&
                    i.GetParameters()[1].ParameterType.IsAssignableFrom(visual.GetType()));
            if (ctor1 != null)
                return (Control)ctor1.Invoke(new object[] { view, visual });

            return null;
        }

    }

}
