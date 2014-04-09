using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NXKit.Util;

namespace NXKit.Web.IO
{

    /// <summary>
    /// Provides a converter to emit objects marked with <see cref="RemoteAttribute"/> as JSON.
    /// </summary>
    public class RemoteObjectJsonConverter :
        JsonConverter
    {

        internal struct Interface
        {

            internal object target;
            internal Type type;

        }

        /// <summary>
        /// Gets the supported remote interface types of the given <see cref="Object"/>.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal static IEnumerable<Interface> GetRemoteInterfaces(object obj)
        {
            Contract.Requires<ArgumentNullException>(obj != null);

            var xobj = obj as XObject;
            if (xobj != null)
                return GetRemoteInterfaces(xobj);

            return GetRemoteInterfaces(new[] { obj });
        }

        /// <summary>
        /// Gets the combined supported remote interface types of the given objects.
        /// </summary>
        /// <param name="obj"></param>
        internal static IEnumerable<Interface> GetRemoteInterfaces(IEnumerable<object> objects)
        {
            Contract.Requires<ArgumentNullException>(objects != null);

            return objects
                .Where(i => i != null)
                .Select(i => new
                {
                    Target = i,
                    Types = GetRemoteTypes(i),
                })
                .Where(i => i.Types.Any())
                .SelectMany(i => i.Types
                    .Select(j => new
                    {
                        Target = i.Target,
                        Type = j,
                    }))
                .GroupBy(i => i.Type)
                .Select(i => i.First())
                .Select(i => new Interface()
                {
                    target = i.Target,
                    type = i.Type,
                });
        }

        /// <summary>
        /// Gets all the supported remote interface types for the specified object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal static IEnumerable<Type> GetRemoteTypes(object obj)
        {
            Contract.Requires<ArgumentNullException>(obj != null);

            return GetRemoteTypes(obj.GetType());
        }

        /// <summary>
        /// Gets all the supported remote interface types for the specified type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static IEnumerable<Type> GetRemoteTypes(Type type)
        {
            Contract.Requires<ArgumentNullException>(type != null);

            return TypeDescriptor.GetReflectionType(type)
                .GetInterfaces()
                .Concat(TypeDescriptor.GetReflectionType(type)
                    .Recurse(j => j.BaseType))
                .Where(i => i.GetCustomAttribute<RemoteAttribute>(false) != null);
        }

        /// <summary>
        /// Gets the supported remote properties of the given type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static IEnumerable<PropertyInfo> GetRemoteProperties(Type type)
        {
            Contract.Requires<ArgumentNullException>(type != null);

            return TypeDescriptor.GetReflectionType(type)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(j => j.DeclaringType == type)
                .Where(j => j.GetCustomAttribute<RemoteAttribute>(false) != null)
                .GroupBy(j => j.Name)
                .Select(j => j.First());
        }

        /// <summary>
        /// Gets the supported remote methods of the given type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static IEnumerable<MethodInfo> GetRemoteMethods(Type type)
        {
            Contract.Requires<ArgumentNullException>(type != null);

            return TypeDescriptor.GetReflectionType(type)
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(j => j.DeclaringType == type)
                .Where(j => j.GetCustomAttribute<RemoteAttribute>(false) != null)
                .GroupBy(j => j.Name)
                .Select(j => j.First());
        }

        /// <summary>
        /// Returns all of the properties for a given <see cref="Interface"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static IEnumerable<JProperty> InterfaceToProperties(Interface type)
        {
            foreach (var property in GetRemoteProperties(type.type))
                yield return new JProperty(property.Name,
                    property.GetValue(type.target));

            foreach (var method in GetRemoteMethods(type.type))
                yield return new JProperty("@" + method.Name,
                    new JArray());
        }

        /// <summary>
        /// Converts the given source object into a <see cref="JObject"/> of compatible interfaces.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        internal static JObject ToObject(object source)
        {
            Contract.Requires<ArgumentNullException>(source != null);

            return new JObject(
                GetRemoteInterfaces(source)
                    .Select(i => new JProperty(
                        i.type.FullName,
                        new JObject(
                            InterfaceToProperties(i)))));
        }

        /// <summary>
        /// Converts the given set of source objects into a single <see cref="JObject"/> of compatible interfaces.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        internal static JObject ToObject(IEnumerable<object> source)
        {
            Contract.Requires<ArgumentNullException>(source != null);

            return new JObject(
                GetRemoteInterfaces(source)
                    .Select(i => new JProperty(
                        i.type.FullName,
                        new JObject(
                            InterfaceToProperties(i)))));
        }

        public override bool CanConvert(Type objectType)
        {
            return GetRemoteTypes(objectType).Any();
        }

        public override object ReadJson(JsonReader reader, System.Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            ToObject(value).WriteTo(writer);
            writer.WriteEndObject();
        }

    }

}
