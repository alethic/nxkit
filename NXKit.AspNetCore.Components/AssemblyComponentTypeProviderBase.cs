using System;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace NXKit.AspNetCore.Components
{

    /// <summary>
    /// Obtains attributed component types from the specified assembly.
    /// </summary>
    public abstract class AssemblyComponentTypeProviderBase : INXComponentTypeProvider
    {

        readonly Assembly assembly;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="assembly"></param>
        protected AssemblyComponentTypeProviderBase(Assembly assembly)
        {
            this.assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
        }

        /// <summary>
        /// Gets the component type for the specified element.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public virtual Type GetComponentType(XElement element)
        {
            return assembly.GetTypes()
                .Where(i => typeof(INXComponent).IsAssignableFrom(i))
                .Where(i => i.GetCustomAttributes<NXComponentAttribute>().Any(j => IsMatch(element, j)))
                .FirstOrDefault(i => i != null);
        }

        bool IsMatch(XElement element, NXComponentAttribute attribute)
        {
            return attribute.Name == null || element.Name == attribute.Name;
        }

    }

}
