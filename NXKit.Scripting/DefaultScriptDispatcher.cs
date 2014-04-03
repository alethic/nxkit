using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq;

namespace NXKit.Scripting
{

    [Export(typeof(IScriptDispatcher))]
    public class DefaultScriptDispatcher :
        IScriptDispatcher
    {

        readonly IEnumerable<IScriptEngine> engines;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="engines"></param>
        [ImportingConstructor]
        public DefaultScriptDispatcher(
            [ImportMany] IEnumerable<IScriptEngine> engines)
        {
            Contract.Requires<ArgumentNullException>(engines != null);

            this.engines = engines;
        }

        public object Execute(string type, string code)
        {
            return engines
                .Where(i => i.CanExecute(type, code))
                .Select(i => i.Execute(code))
                .FirstOrDefault();
        }

    }

}
