using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq;

using NXKit.Composition;
using NXKit.Util;

namespace NXKit.Scripting
{

    /// <summary>
    /// Provides <see cref="IScriptObject"/> instances from the composition engine.
    /// </summary>
    [Export(typeof(IScriptObjectProvider))]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Host)]
    public class DefaultScriptObjectProvider :
        IScriptObjectProvider
    {

        readonly IEnumerable<ScriptObjectDescriptor> objects;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="objects"></param>
        [ImportingConstructor]
        public DefaultScriptObjectProvider(
            [ImportMany] IEnumerable<Lazy<IScriptObject, IDictionary<string, object>>> objects)
        {
            Contract.Requires<ArgumentNullException>(objects != null);

            // discover available IScriptObject implementations and resolve their metadata names
            this.objects = objects
                .Select(i => new { Name = (string)i.Metadata.GetOrDefault("Name"), Value = (Func<object>)(() => i.Value) })
                .Where(i => i.Name != null)
                .Select(i => new ScriptObjectDescriptor(i.Name, i.Value))
                .ToArray();
        }

        public IEnumerable<ScriptObjectDescriptor> GetObjects()
        {
            return objects;
        }

    }

}
