using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

using Jurassic;

using NXKit.Composition;
using NXKit.IO.Media;
using NXKit.Xml;

namespace NXKit.Scripting.EcmaScript
{

    /// <summary>
    /// Provides a ECMAScript implementation using the Jurassic JavaScript engine.
    /// </summary>
    [Export(typeof(IScriptEngine))]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Host)]
    public class JurassicScriptEngine :
        IScriptEngine,
        IDisposable
    {

        static readonly MediaRangeList ACCEPT = new MediaRange[]
        {
            "application/ecmascript",
            "application/javascript",
            "text/javascript",
            "application/x-javascript",
        };


        readonly Func<Document> host;
        readonly JurassicScriptEngineState state;
        readonly ScriptObjectDescriptor[] objects;
        readonly ScriptObjectProxyGenerator generator;
        Jurassic.ScriptEngine engine;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="objects"></param>
        [ImportingConstructor]
        public JurassicScriptEngine(
            Func<Document> host,
            [ImportMany] IEnumerable<IScriptObjectProvider> objects,
            ScriptObjectProxyGenerator generator)
        {
            Contract.Requires<ArgumentNullException>(host != null);
            Contract.Requires<ArgumentNullException>(objects != null);
            Contract.Requires<ArgumentNullException>(generator != null);

            this.host = host;
            this.objects = objects.SelectMany(i => i.GetObjects()).ToArray();
            this.state = host().Xml.AnnotationOrCreate<JurassicScriptEngineState>();
            this.generator = generator;
        }

        /// <summary>
        /// Gets the engine instance, or creates a new one.
        /// </summary>
        /// <returns></returns>
        Jurassic.ScriptEngine GetEngine()
        {
            if (engine != null)
                return engine;

            engine = new ScriptEngine();
            ConfigureJurassic();
            return engine;
        }

        /// <summary>
        /// Configures the Jurassic engine.
        /// </summary>
        void ConfigureJurassic()
        {
            // configure script engine
            engine.EnableExposedClrTypes = true;

            // register globally available script objects
            foreach (var i in objects)
                engine.SetGlobalValue(i.Name, generator.GetOrBuildProxy(engine, i.Value));
        }

        /// <summary>
        /// Deconfigures the Jurassic engine.
        /// </summary>
        void DeconfigureJurassic()
        {
            // unregister globally available script objects
            foreach (var i in objects)
                engine.SetGlobalValue(i.Name, "");
        }

        public bool CanExecute(string type, string code)
        {
            return ACCEPT.Matches(type);
        }

        public void Execute(string type, string code)
        {
            if (!CanExecute(type, code))
                throw new InvalidOperationException();

            try
            {
                GetEngine().Execute(code);
            }
            catch (JavaScriptException e)
            {
                throw new ScriptException(e.Message, e);
            }
        }

        public object Evaluate(string type, string code)
        {
            if (!CanExecute(type, code))
                throw new InvalidOperationException();

            try
            {
                return GetEngine().Evaluate(code);
            }
            catch (JavaScriptException e)
            {
                throw new ScriptException(e.Message, e);
            }
        }

        public void Load()
        {
            if (state.state != null &&
                state.state.Length > 0)
            {
                // deserialize engine from previous state
                var m = new MemoryStream(state.state);
                var b = new BinaryFormatter();
                engine = (Jurassic.ScriptEngine)b.Deserialize(m);

                // set up engine
                ConfigureJurassic();
            }
        }

        public void Save()
        {
            if (engine != null)
            {
                // unset up engine
                DeconfigureJurassic();

                // serialize to annotation
                var b = new BinaryFormatter();
                var m = new MemoryStream();
                b.Serialize(m, engine);
                state.state = m.ToArray();

                // set up engine
                ConfigureJurassic();
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        ~JurassicScriptEngine()
        {
            Dispose();
        }

    }

}
