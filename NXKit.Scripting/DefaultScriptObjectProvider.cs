using System;
using System.Collections.Generic;
using System.Linq;

using NXKit.Composition;

namespace NXKit.Scripting
{

    /// <summary>
    /// Provides <see cref="IScriptObject"/> instances from the composition engine.
    /// </summary>
    [Export(typeof(IScriptObjectProvider), CompositionScope.Host)]
    public class DefaultScriptObjectProvider :
        IScriptObjectProvider
    {

        readonly IEnumerable<ScriptObjectDescriptor> objects;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="objects"></param>
        public DefaultScriptObjectProvider(
            IEnumerable<IExport<IScriptObject, IScriptObjectMetadata>> objects)
        {
            if (objects == null)
                throw new ArgumentNullException(nameof(objects));

            // discover available IScriptObject implementations and resolve their metadata names
            this.objects = objects
                .Select(i => new { Name = i.Metadata.Name, Value = (Func<object>)(() => i.Value) })
                .Where(i => !string.IsNullOrWhiteSpace(i.Name))
                .Select(i => new ScriptObjectDescriptor(i.Name, i.Value))
                .ToArray();
        }

        public IEnumerable<ScriptObjectDescriptor> GetObjects()
        {
            return objects;
        }

    }

}
