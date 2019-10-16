using System;
using System.Linq;
using System.Reflection;

using NXKit.View.Server.Serialization;

namespace NXKit
{

    public class RemoteDescriptor
    {

        readonly object target;
        readonly Type type;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="type"></param>
        public RemoteDescriptor(object target, Type type)
        {
            this.target = target ?? throw new ArgumentNullException(nameof(target));
            this.type = type ?? throw new ArgumentNullException(nameof(type));
        }

        /// <summary>
        /// Gets the target of the extension.
        /// </summary>
        public object Target
        {
            get { return target; }
        }

        /// <summary>
        /// Gets the type of the extension.
        /// </summary>
        public Type Type
        {
            get { return type; }
        }

        /// <summary>
        /// Gets the <see cref="MethodInfo"/> of the given name for this <see cref="RemoteDescriptor"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public PropertyInfo GetProperty(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentOutOfRangeException(nameof(name));

            return RemoteObjectJsonConverter.GetRemoteProperties(Type)
                .FirstOrDefault(i => i.Name == name);
        }

        /// <summary>
        /// Gets the <see cref="MethodInfo"/> of the given name for this <see cref="RemoteDescriptor"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public MethodInfo GetMethod(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentOutOfRangeException(nameof(name));

            return RemoteObjectJsonConverter.GetRemoteMethods(Type)
                .FirstOrDefault(i => i.Name == name);
        }

    }

}
