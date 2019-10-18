using System;
using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using NUnit.Framework;

using NXKit.XForms.Json.Serialization;

namespace NXKit.XForms.Json.Tests
{

    public class JsonNodeDeserializerTests
    {

        [Test]
        public void Test_simple_property()
        {
            var job = new JObject(
                new JProperty("Property1", "Value"));
            var stm = new MemoryStream();
            using (var wrt = new JsonTextWriter( new StreamWriter(stm)))
                job.WriteTo(wrt);

            var srs = new JsonNodeDeserializer();
            var xml = srs.Deserialize(new StreamReader(new MemoryStream(stm.ToArray())), "application/json");
        }

        [Test]
        public void Test_single_array()
        {
            var job = new JObject(
                new JProperty("Property1", 
                    new JArray("Hello")));
            var stm = new MemoryStream();
            using (var wrt = new JsonTextWriter(new StreamWriter(stm)))
                job.WriteTo(wrt);

            var srs = new JsonNodeDeserializer();
            var xml = srs.Deserialize(new StreamReader(new MemoryStream(stm.ToArray())), "application/json");
        }

        [Test]
        public void Test_longer_array()
        {
            var job = new JObject(
                new JProperty("ArrayProperty",
                    new JArray("Hello", "mother", "how", "are", "you", "doing?")));
            var stm = new MemoryStream();
            using (var wrt = new JsonTextWriter(new StreamWriter(stm)))
                job.WriteTo(wrt);

            var srs = new JsonNodeDeserializer();
            var xml = srs.Deserialize(new StreamReader(new MemoryStream(stm.ToArray())), "application/json");
        }

        [Test]
        public void Test_with_types()
        {
            var job = new JObject(
                new JProperty("ArrayProperty",
                    new JArray("Hello", null, new DateTime(2012, 1, 1), 123, true, false)));
            var stm = new MemoryStream();
            using (var wrt = new JsonTextWriter(new StreamWriter(stm)))
                job.WriteTo(wrt);

            var srs = new JsonNodeDeserializer();
            var xml = srs.Deserialize(new StreamReader(new MemoryStream(stm.ToArray())), "application/json");
        }

        [Test]
        public void Test_deep()
        {
            var job = new JObject(
                new JProperty("Prop1", 1),
                new JProperty("Prop2", 2),
                new JProperty("Prop3", new JObject(
                    new JProperty("Prop1", 1),
                    new JProperty("Prop2", 2),
                    new JProperty("Prop3", new JObject(
                        new JProperty("Prop1", 1),
                        new JProperty("Prop2", 2),
                        new JProperty("Prop3", 3),
                        new JProperty("Prop4", 4),
                        new JProperty("Prop5", 5),
                        new JProperty("Prop6", 6),
                        new JProperty("Prop7", 7))),
                    new JProperty("Prop4", 4))),
                new JProperty("Prop4", 4),
                new JProperty("Prop5", 5));
            var stm = new MemoryStream();
            using (var wrt = new JsonTextWriter(new StreamWriter(stm)))
                job.WriteTo(wrt);

            var srs = new JsonNodeDeserializer();
            var xml = srs.Deserialize(new StreamReader(new MemoryStream(stm.ToArray())), "application/json");
        }

    }

}
