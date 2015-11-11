using System;
using System.Diagnostics.Contracts;
using System.Reflection;

using Jurassic.Library;

namespace NXKit.Scripting.EcmaScript
{

    /// <summary>
    /// Wraps a .NET object for exposure to the Jurassic scripting language. Extended by dynamic proxy classes that add
    /// the supported properties.
    /// </summary>
    public abstract class ScriptObjectInstance :
        ObjectInstance
    {

        /// <summary>
        /// Gets the <see cref="MethodInfo"/> of the given target object that should be invoked by the given argument values.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="methodName"></param>
        /// <param name="parameterTypes"></param>
        /// <returns></returns>
        protected static MethodInfo GetInvocationMethodInfo(object target, string methodName, Type[] parameterTypes)
        {
            Contract.Requires<ArgumentNullException>(target != null);
            Contract.Requires<ArgumentNullException>(methodName != null);
            Contract.Requires<ArgumentNullException>(parameterTypes != null);

            return target.GetType().GetMethod(
                methodName,
                BindingFlags.Public | BindingFlags.Instance,
                null,
                parameterTypes,
                null);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="engine"></param>
        public ScriptObjectInstance(Jurassic.ScriptEngine engine)
            : base(engine)
        {
            PopulateFields();
            PopulateFunctions();
        }

        /// <summary>
        /// Invoked to invoke a given method on the real instance.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        protected object Invoke(object target, MethodInfo method, params object[] args)
        {
            Contract.Requires<ArgumentNullException>(target != null);
            Contract.Requires<ArgumentNullException>(method != null);
            Contract.Requires<ArgumentNullException>(args != null);

            return method.Invoke(target, args);
        }

    }

}
