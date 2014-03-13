using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Web.UI;

using NXKit.Web.UI;

namespace NXKit.XForms.Web.UI
{

    /// <summary>
    /// Provides an <see cref="IRangeEditableProvider"/> implementation that scans a given <see cref="Assembly"/>.
    /// </summary>
    public class AssemblyRangeEditableProvider :
        IRangeEditableProvider
    {

        readonly Assembly assembly;
        readonly Type[] types;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="assembly"></param>
        public AssemblyRangeEditableProvider(Assembly assembly)
        {
            Contract.Requires<ArgumentNullException>(assembly != null);

            this.assembly = assembly;
            this.types = assembly.GetTypes()
                .Where(i => i.IsClass && !i.IsAbstract && !i.IsGenericTypeDefinition)
                .Where(i => typeof(IRangeEditable).IsAssignableFrom(i))
                .OrderByDescending(i => PriorityAttribute.GetPriority(i))
                .ToArray();
        }

        public Control Create(View view, XFormsRangeVisual visual)
        {
            // scan assembly for marked type
            return assembly.GetTypes()
                .Where(i => typeof(IRangeEditable).IsAssignableFrom(i))
                .Where(i => XFormsXsdTypeAttribute.Predicate(i, visual.Binding))
                .Select(i => CreateEditable(i, view, visual))
                .FirstOrDefault(i => i != null);
        }

        /// <summary>
        /// Attempts to instantiate the given editable type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="view"></param>
        /// <param name="visual"></param>
        /// <returns></returns>
        Control CreateEditable(Type type, View view, Visual visual)
        {
            Contract.Requires<ArgumentNullException>(type != null);
            Contract.Requires<ArgumentNullException>(view != null);
            Contract.Requires<ArgumentNullException>(visual != null);
            Contract.Requires<ArgumentException>(typeof(IRangeEditable).IsAssignableFrom(type));

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
