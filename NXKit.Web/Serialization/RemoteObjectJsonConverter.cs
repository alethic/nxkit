using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using NXKit.Util;

namespace NXKit.Web.Serialization
{

    /// <summary>
    /// Provides a converter to emit objects marked with <see cref="RemoteAttribute"/> as JSON.
    /// </summary>
    public class RemoteObjectJsonConverter :
        JsonConverter
    {

        /// <summary>
        /// Gets the supported remote interface types of the given <see cref="Object"/>.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal static IEnumerable<RemoteDescriptor> GetRemotes(object obj)
        {
            Contract.Requires<ArgumentNullException>(obj != null);

            return GetRemotes(new[] { obj });
        }

        /// <summary>
        /// Gets the combined supported remote interface types of the given objects.
        /// </summary>
        /// <param name="objects"></param>
        internal static IEnumerable<RemoteDescriptor> GetRemotes(IEnumerable<object> objects)
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
                .Select(i => new RemoteDescriptor(i.Target, i.Type));
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
                .Where(i => i.DeclaringType == type)
                .Where(i => i.GetCustomAttribute<RemoteAttribute>(false) != null)
                .GroupBy(i => i.Name)
                .Select(i => i.First());
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
        /// Null-safe invocation for JToken.FromObject.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        internal static JToken JTokenFromObject(object value, JsonSerializer serializer)
        {
            Contract.Requires<ArgumentNullException>(serializer != null);

            return value != null ? JToken.FromObject(value, serializer) : null;
        }

        /// <summary>
        /// Returns all of the properties for a given <see cref="RemoteDescriptor"/>.
        /// </summary>
        /// <param name="remote"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        internal static IEnumerable<JProperty> InterfaceToProperties(RemoteDescriptor remote, JsonSerializer serializer)
        {
            Contract.Requires<ArgumentNullException>(serializer != null);

            foreach (var property in GetRemoteProperties(remote.Type))
                yield return new JProperty(property.Name,
                    JTokenFromObject(property.GetValue(remote.Target), serializer));
        }

        /// <summary>
        /// Converts the given source object into a <see cref="JObject"/> of compatible interfaces.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        internal static void RemoteToObject(object source, JObject destination, JsonSerializer serializer)
        {
            Contract.Requires<ArgumentNullException>(source != null);
            Contract.Requires<ArgumentNullException>(destination != null);
            Contract.Requires<ArgumentNullException>(serializer != null);

            // append interfaces to object
            destination.Add(
                GetRemotes(source)
                    .Select(i => new JProperty(
                        i.Type.FullName,
                        new JObject(
                            InterfaceToProperties(i, serializer)))));
        }

        /// <summary>
        /// Converts the given source object into a <see cref="JObject"/> of compatible interfaces.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        internal static JObject RemoteToObject(object source, JsonSerializer serializer)
        {
            Contract.Requires<ArgumentNullException>(source != null);
            Contract.Requires<ArgumentNullException>(serializer != null);

            var obj = new JObject();
            RemoteToObject(source, obj, serializer);
            return obj;
        }

        /// <summary>
        /// Converts the given set of source objects into a single <see cref="JObject"/> of compatible interfaces.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        internal static void RemotesToObject(IEnumerable<object> source, JObject destination, JsonSerializer serializer)
        {
            Contract.Requires<ArgumentNullException>(source != null);
            Contract.Requires<ArgumentNullException>(destination != null);
            Contract.Requires<ArgumentNullException>(serializer != null);

            // append interfaces to object
            destination.Add(
                GetRemotes(source)
                    .Select(i => new JProperty(
                        i.Type.FullName,
                        new JObject(
                            InterfaceToProperties(i, serializer)))));
        }

        /// <summary>
        /// Converts the given set of source objects into a single <see cref="JObject"/> of compatible interfaces.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        internal static JObject RemotesToObject(IEnumerable<object> source, JsonSerializer serializer)
        {
            Contract.Requires<ArgumentNullException>(source != null);
            Contract.Requires<ArgumentNullException>(serializer != null);

            var obj = new JObject();
            RemotesToObject(source, obj, serializer);
            return obj;
        }


        public override bool CanConvert(Type objectType)
        {
            return GetRemoteTypes(objectType).Any();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }

        public override sealed void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var jobj = new JObject();
            Apply(value, serializer, jobj);
            jobj.WriteTo(writer);
        }

        protected virtual void Apply(object value, JsonSerializer serializer, JObject obj)
        {
            Contract.Requires<ArgumentNullException>(value != null);
            Contract.Requires<ArgumentNullException>(serializer != null);
            Contract.Requires<ArgumentNullException>(obj != null);

            obj["Type"] = "Object";
            RemoteToObject(value, obj, serializer);
        }

    }

}
