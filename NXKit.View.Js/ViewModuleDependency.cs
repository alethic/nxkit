using System;
using System.Linq;

using Newtonsoft.Json;

namespace NXKit.View.Js
{

    /// <summary>
    /// Describes a dependency on a node.
    /// </summary>
    [JsonConverter(typeof(ViewModuleDependencyJsonConverter))]
    public class ViewModuleDependency
    {

        /// <summary>
        /// Returns the string version of the <see cref="ViewModuleType"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        static string TypeToString(ViewModuleType type)
        {
            switch (type)
            {
                case ViewModuleType.Script:
                    return "nx-js!";
                case ViewModuleType.Css:
                    return "nx-css!";
                case ViewModuleType.Template:
                    return "nx-html!";
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Parses the <see cref="ViewModuleType"/> from the given string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        static ViewModuleType ParseType(string value)
        {
            if (value.StartsWith("nx-js!"))
                return ViewModuleType.Script;
            if (value.StartsWith("nx-css!"))
                return ViewModuleType.Css;
            if (value.StartsWith("nx-html!"))
                return ViewModuleType.Template;

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Parses the given string instance into a <see cref="ViewModuleDependency"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ViewModuleDependency Parse(string value)
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));

            if (!string.IsNullOrWhiteSpace(value))
            {
                var type = ParseType(value);
                var name = value.Split('!').Last();
                return new ViewModuleDependency(type, name);
            }
            else
                return null;
        }

        readonly ViewModuleType type;
        readonly string name;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        public ViewModuleDependency(ViewModuleType type, string name)
        {
            this.type = type;
            this.name = name;
        }

        /// <summary>
        /// Specifies the type of dependency.
        /// </summary>
        public ViewModuleType Type
        {
            get { return type; }
        }

        /// <summary>
        /// Specifies the name of the dependency.
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        public override string ToString()
        {
            return TypeToString(type) + name;
        }

    }

}
