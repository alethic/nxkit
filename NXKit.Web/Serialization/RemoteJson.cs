using System;
using System.Linq;
using NXKit.Xml;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NXKit.Web.Serialization
{

    /// <summary>
    /// Provides methods to write a remote object stream.
    /// </summary>
    public static class RemoteJson
    {

        static readonly RemoteJsonSerializer serializer = new RemoteJsonSerializer();

        /// <summary>
        /// Gets the remote interface values from the given <see cref="XElement"/> as a <see cref="JObject"/>.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="element"></param>
        public static void GetJson(JsonWriter writer, XElement element)
        {
            Contract.Requires<ArgumentNullException>(writer != null);
            Contract.Requires<ArgumentNullException>(element != null);

            serializer.Serialize(writer, element);
        }

        /// <summary>
        /// Sets the remote interface values from the given <see cref="JsonReader"/> onto the <see cref="XElement"/>.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="element"></param>
        public static void SetJson(JsonReader reader, XElement element)
        {
            Contract.Requires<ArgumentNullException>(reader != null);
            Contract.Requires<ArgumentNullException>(element != null);

            SetJson(JObject.Load(reader), element);
        }

        /// <summary>
        /// Sets the remote interface values from the given <see cref="JObject"/> onto the <see cref="XElement"/>.
        /// </summary>
        /// <param name="jobject"></param>
        /// <param name="element"></param>
        public static void SetJson(JObject jobject, XElement element)
        {
            Contract.Requires<ArgumentNullException>(jobject != null);
            Contract.Requires<ArgumentNullException>(element != null);

            // each available remote interface on the element
            foreach (var remote in RemoteObjectJsonConverter.GetRemotes(element.Interfaces()))
            {
                // corresponding JObject data
                var jremote = jobject.GetValue(remote.Type.FullName) as JObject;
                if (jremote == null)
                    continue;

                // each available property on the remote interface
                foreach (var property in RemoteObjectJsonConverter.GetRemoteProperties(remote.Type))
                {
                    // properties must be readable and writable
                    if (!property.CanWrite ||
                        !property.CanRead)
                        continue;

                    // corresponding JProperty data
                    var jproperty = jremote.Property(property.Name);
                    if (jproperty == null)
                        continue;

                    // extract incoming value and convert to appropriate property value type
                    var jvalue = jproperty.Value;
                    var type = property.PropertyType;
                    var value = jvalue != null ? jvalue.ToObject(type) : null;

                    // if value has been changed, apply change to remote
                    if (!object.Equals(property.GetValue(remote.Target), value))
                        property.SetValue(remote.Target, value);
                }

                foreach (var method in RemoteObjectJsonConverter.GetRemoteMethods(remote.Type))
                {
                    if (!method.IsPublic)
                        continue;

                    var jmethod = jremote.Property("@" + method.Name);
                    if (jmethod == null)
                        continue;

                    foreach (JObject jinvoke in jmethod.Value<JArray>())
                    {
                        var count = 0;
                        var parameters = method.GetParameters();
                        var invoke = new object[parameters.Length];
                        for (int i = 0; i < invoke.Length; i++)
                        {
                            // submitted JSON parameter value
                            var j = jinvoke[parameters[i].Name];
                            if (j == null)
                                break;

                            // convert JObject to appropriate type
                            var t = parameters[i].ParameterType;
                            var o = j != null ? j.ToObject(t) : null;

                            // successful conversion
                            invoke[i] = o;
                            count = i + 1;
                        }

                        // unsuccessful parameter count, try next method
                        if (count != parameters.Length)
                            continue;

                        // successful; done with invoke object
                        method.Invoke(remote.Target, invoke);
                        break;
                    }
                }
            }
        }

    }

}
