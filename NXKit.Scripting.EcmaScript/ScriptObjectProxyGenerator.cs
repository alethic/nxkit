using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

using NXKit.Composition;

namespace NXKit.Scripting.EcmaScript
{

    /// <summary>
    /// Generates <see cref="ScriptObjectInstance"/> classes from given native classes decorated with <see
    /// cref="ScriptFunctionAttribute"/>.
    /// </summary>
    [Export(typeof(ScriptObjectProxyGenerator))]
    public class ScriptObjectProxyGenerator
    {

        static readonly ConcurrentDictionary<Type, Type> proxyTypes = new ConcurrentDictionary<Type, Type>();

        /// <summary>
        /// Gets or generates a <see cref="ScriptObjectInstance"/> from the given target object.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public ScriptObjectInstance GetOrBuildProxy(Jurassic.ScriptEngine engine, object target)
        {
            if (engine == null)
                throw new ArgumentNullException(nameof(engine));
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            return (ScriptObjectInstance)Activator.CreateInstance(
                GetOrBuildProxyType(target.GetType()),
                new[] { engine, target });
        }

        /// <summary>
        /// Gets or generates the proxy type for the given real type.
        /// </summary>
        /// <param name="targetType"></param>
        /// <returns></returns>
        Type GetOrBuildProxyType(Type targetType)
        {
            if (targetType == null)
                throw new ArgumentNullException(nameof(targetType));

            return proxyTypes.GetOrAdd(targetType, BuildType);
        }

        /// <summary>
        /// Generates a new <see cref="TypeBuilder"/> for a new proxy type.
        /// </summary>
        /// <param name="targetType"></param>
        /// <returns></returns>
        Type BuildType(Type targetType)
        {
            if (targetType == null)
                throw new ArgumentNullException(nameof(targetType));

            var assembly = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName("NXKit.Scripting.EcmaScript.Proxy"), AssemblyBuilderAccess.Run);
            var module = assembly.DefineDynamicModule(assembly.GetName().Name);
            var type = module.DefineType(targetType.FullName + "_Proxy", TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.Sealed, typeof(ScriptObjectInstance));

            // hold the object target in a field
            var target = type.DefineField("target", typeof(object), FieldAttributes.Private);

            // define new ctor
            BuildConstructor(type, target);

            // define available methods
            foreach (var method in GetScriptMethods(targetType))
                BuildMethod(type, target, method);

            // finish building type
            return type.CreateType();
        }

        /// <summary>
        /// Returns the methods of the given target <see cref="Type"/> that are marked as script methods.
        /// </summary>
        /// <param name="targetType"></param>
        /// <returns></returns>
        IEnumerable<MethodInfo> GetScriptMethods(Type targetType)
        {
            if (targetType == null)
                throw new ArgumentNullException(nameof(targetType));

            return targetType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(i => i.GetCustomAttribute<ScriptFunctionAttribute>() != null);
        }

        /// <summary>
        /// Emits the IL to implement the constructor.
        /// </summary>
        /// <param name="tb"></param>
        ConstructorBuilder BuildConstructor(TypeBuilder tb, FieldBuilder target)
        {
            if (tb == null)
                throw new ArgumentNullException(nameof(tb));
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            var ctor = tb.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new Type[] { typeof(Jurassic.ScriptEngine), typeof(object) });
            var bctor = typeof(ScriptObjectInstance).GetConstructor(new[] { typeof(Jurassic.ScriptEngine) });

            var il = ctor.GetILGenerator();

            // call base ctor with engine as first parameter
            il.Emit(OpCodes.Ldarg_0); // load this
            il.Emit(OpCodes.Ldarg_1); // load first (engine) parameter
            il.Emit(OpCodes.Call, bctor); // call base ctor

            // store target into target field
            il.Emit(OpCodes.Ldarg_0); // load this
            il.Emit(OpCodes.Ldarg_2); // load second (target) parameter
            il.Emit(OpCodes.Stfld, target);

            // finish ctor
            il.Emit(OpCodes.Ret);

            return ctor;
        }

        /// <summary>
        /// Builds the given method.
        /// </summary>
        /// <param name="tb"></param>
        /// <param name="target"></param>
        /// <param name="targetMethod"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        MethodBuilder BuildMethod(TypeBuilder tb, FieldBuilder target, MethodInfo targetMethod)
        {
            if (tb == null)
                throw new ArgumentNullException(nameof(tb));
            if (target == null)
                throw new ArgumentNullException(nameof(target));
            if (targetMethod == null)
                throw new ArgumentNullException(nameof(targetMethod));

            // extract parameter types
            var parameterTypes = targetMethod.GetParameters()
                .Select(i => i.ParameterType)
                .ToArray();

            // define new method
            var method = tb.DefineMethod(
                targetMethod.Name,
                MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig,
                targetMethod.ReturnType,
                parameterTypes);

            // obtain attribute
            var attribute = targetMethod.GetCustomAttribute<ScriptFunctionAttribute>();
            if (attribute == null)
                throw new NullReferenceException();

            // add custom attribute
            method.SetCustomAttribute(
                new CustomAttributeBuilder(
                    typeof(Jurassic.Library.JSFunctionAttribute).GetConstructor(Type.EmptyTypes),
                    new object[0],
                    new[] { typeof(Jurassic.Library.JSFunctionAttribute).GetProperty("Name") },
                    new[] { attribute.Name ?? targetMethod.Name }));

            // add params attributes and names
            for (int i = 0; i < parameterTypes.Length; i++)
            {
                var param = targetMethod.GetParameters()[i];
                if (param == null)
                    throw new NullReferenceException();

                if (param.GetCustomAttribute<ParamArrayAttribute>() != null)
                    method.DefineParameter(i + 1, param.Attributes, param.Name)
                        .SetCustomAttribute(
                            new CustomAttributeBuilder(
                                typeof(ParamArrayAttribute).GetConstructor(Type.EmptyTypes),
                                new object[0],
                                new PropertyInfo[0],
                                new object[0]));
            }

            // begin emitting IL
            var il = method.GetILGenerator();
            il.DeclareLocal(typeof(object[]));
            il.DeclareLocal(typeof(Type[]));

            // create a new array to store the parameter values
            il.Emit(OpCodes.Ldc_I4, parameterTypes.Length);
            il.Emit(OpCodes.Newarr, typeof(object));
            il.Emit(OpCodes.Stloc_0);

            // load arguments into the new parameter array
            if (parameterTypes.Length > 0)
            {
                for (int j = 0; j < parameterTypes.Length; j++)
                {
                    il.Emit(OpCodes.Ldloc_0);
                    il.Emit(OpCodes.Ldc_I4, j);
                    il.Emit(OpCodes.Ldarg, j + 1);
                    if (parameterTypes[j].IsValueType)
                        il.Emit(OpCodes.Box, parameterTypes[j]);

                    il.Emit(OpCodes.Stelem_Ref);
                }
            }

            // create an array to store the parameter types
            il.Emit(OpCodes.Ldc_I4, parameterTypes.Length);
            il.Emit(OpCodes.Newarr, typeof(Type));
            il.Emit(OpCodes.Stloc_1); // save array in variable 1

            // load arguments into the new parameter array
            if (parameterTypes.Length > 0)
            {
                for (int j = 0; j < parameterTypes.Length; j++)
                {
                    il.Emit(OpCodes.Ldloc_1);
                    il.Emit(OpCodes.Ldc_I4, j);
                    il.Emit(OpCodes.Ldtoken, parameterTypes[j]);
                    il.Emit(OpCodes.Call, typeof(Type).GetMethod("GetTypeFromHandle", BindingFlags.Public | BindingFlags.Static));
                    il.Emit(OpCodes.Stelem_Ref);
                }
            }

            // begin setup for Invoke
            il.Emit(OpCodes.Ldarg_0); // loads this

            // load target
            il.Emit(OpCodes.Ldarg_0); // loads this
            il.Emit(OpCodes.Ldfld, target);

            // invoke GetMethod helper to retrieve the MethodInfo we are implementing
            il.Emit(OpCodes.Ldarg_0); // loads this
            il.Emit(OpCodes.Ldfld, target);
            il.Emit(OpCodes.Ldstr, targetMethod.Name); // load target method name
            il.Emit(OpCodes.Ldloc_1); // load parameter type array
            il.Emit(OpCodes.Call, typeof(ScriptObjectInstance).GetMethod("GetInvocationMethodInfo", BindingFlags.NonPublic | BindingFlags.Static));

            // invoke the Invoke method
            il.Emit(OpCodes.Ldloc_0); // load parameter arguments
            il.Emit(OpCodes.Call, typeof(ScriptObjectInstance).GetMethod("Invoke", BindingFlags.NonPublic | BindingFlags.Instance));

            // unbox the return type if it is a value type
            if (targetMethod.ReturnType != typeof(void) &&
                targetMethod.ReturnType.IsValueType)
                il.Emit(OpCodes.Unbox, targetMethod.ReturnType);

            // clear return value if target method is void
            if (targetMethod.ReturnType == typeof(void))
                il.Emit(OpCodes.Pop);

            // return with whatever results Invoke resulted in
            il.Emit(OpCodes.Ret);

            return method;
        }

    }

}
