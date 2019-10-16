using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using NXKit.Composition;
using NXKit.IO.Media;
using NXKit.XForms.Serialization;

namespace NXKit.XForms.Json.Serialization
{

    [Export(typeof(IModelDeserializer))]
    public class JsonNodeDeserializer :
        IModelDeserializer
    {

        static readonly MediaRange[] MEDIA_RANGE = new MediaRange[]
        {
            "application/json",
            "text/json"
        };

        public Priority CanDeserialize(MediaRange mediaType)
        {
            return MEDIA_RANGE.Any(i => i.Matches(mediaType)) ? Priority.Default : Priority.Ignore;
        }

        /// <summary>
        /// Deserailizes the incoming <see cref="JObject"/> into an XForms data model, given the definition 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="mediaType"></param>
        /// <returns></returns>
        public XDocument Deserialize(TextReader reader, MediaRange mediaType)
        {
            using (var json = new JsonTextReader(reader))
            {
                var token = JToken.ReadFrom(json);
                if (token == null)
                    return null;

                return ToDocument(token);
            }
        }

        /// <summary>
        /// Transforms a <see cref="JToken"/> into a document.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        XDocument ToDocument(JToken token)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            return new XDocument(
                ToRootElement(token));
        }

        /// <summary>
        /// Transfors a <see cref="JToken"/> into a document root element.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        XElement ToRootElement(JToken token)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            return new XElement("json",
                ToContents(token));
        }

        /// <summary>
        /// Transforms a <see cref="JToken"/> into a series of <see cref="XObject"/>s to place on a <see 
        /// cref="XElement"/>.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        IEnumerable<object> ToContents(JToken token)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            if (token is JObject)
                return ToContents((JObject)token);
            else if (token is JArray)
                return ToContents((JArray)token);
            else if (token is JValue)
                return ToContents((JValue)token);
            else
                throw new InvalidOperationException();
        }

        /// <summary>
        /// Transforms a <see cref="JObject"/> into a series of <see cref="XObject"/>s to place on a <see 
        /// cref="XElement"/>.
        /// </summary>
        /// <param name="jobject"></param>
        /// <returns></returns>
        IEnumerable<XNode> ToContents(JObject jobject)
        {
            if (jobject == null)
                throw new ArgumentNullException(nameof(jobject));

            foreach (var property in jobject.Properties())
                if (property.Value is JArray)
                    foreach (var item in ToContents(ToName(property), (JArray)property.Value))
                        yield return item;
                else
                    yield return new XElement(ToName(property), ToContents(property.Value));
        }

        /// <summary>
        /// Transforms a <see cref="JArray"/> into a series of <see cref="XObject"/>s to place on a <see
        /// cref="XElement"/>, given an array property name of <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="jarray"></param>
        /// <returns></returns>
        IEnumerable<XNode> ToContents(XName name, JArray jarray)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (jarray == null)
                throw new ArgumentNullException(nameof(jarray));

            for (int i = 0; i < jarray.Count; i++)
                yield return new XElement(
                    name,
                    i == 0 ? new XAttribute("starts", "array") : null,
                    ToContents(jarray[i]));
        }

        /// <summary>
        /// Transforms a <see cref="JValue"/> into a series of <see cref="XObject"/>s to place on a <see cref="XElement"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        IEnumerable<object> ToContents(JValue value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (value.Type == JTokenType.Integer ||
                value.Type == JTokenType.Float)
                yield return new XAttribute("type", "number");
            if (value.Type == JTokenType.Boolean)
                yield return new XAttribute("type", "boolean");

            if (value.Type == JTokenType.Null)
                yield return new XAttribute("type", "null");
            else
                yield return value.Value;
        }

        /// <summary>
        /// Gets the <see cref="XName"/> to use for the given <see cref="JProperty"/>.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        XName ToName(JProperty property)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));

            return property.Name;
        }

    }

}
