using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq;

using NXKit.Composition;
using NXKit.Diagnostics;

namespace NXKit.Scripting
{

    [ScriptObject("console")]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Host)]
    public class Console :
        IScriptObject
    {

        readonly ITraceService trace;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="trace"></param>
        [ImportingConstructor]
        public Console(ITraceService trace)
        {
            Contract.Requires<ArgumentNullException>(trace != null);

            this.trace = trace;
        }

        [ScriptFunction("log")]
        public void Log(params object[] items)
        {
            Contract.Requires<ArgumentNullException>(items != null);

            if (items.Length < 1)
                return;

            var fmt = (string)items[0].ToString();
            var arg = items.Skip(1).ToArray();
            trace.Information(fmt, arg);
        }

        [ScriptFunction("debug")]
        public void Debug(params object[] items)
        {
            Contract.Requires<ArgumentNullException>(items != null);

            if (items.Length < 1)
                return;

            var fmt = (string)items[0].ToString();
            var arg = items.Skip(1).ToArray();
            trace.Debug(fmt, arg);
        }

        [ScriptFunction("info")]
        public void Info(params object[] items)
        {
            Contract.Requires<ArgumentNullException>(items != null);

            if (items.Length < 1)
                return;

            var fmt = (string)items[0].ToString();
            var arg = items.Skip(1).ToArray();
            trace.Information(fmt, arg);
        }

        [ScriptFunction("warn")]
        public void Warn(params object[] items)
        {
            Contract.Requires<ArgumentNullException>(items != null);

            if (items.Length < 1)
                return;

            var fmt = (string)items[0].ToString();
            var arg = items.Skip(1).ToArray();
            trace.Warning(fmt, arg);
        }

        [ScriptFunction("error")]
        public void Error(params object[] items)
        {
            Contract.Requires<ArgumentNullException>(items != null);

            if (items.Length < 1)
                return;

            var fmt = items[0].ToString();
            var arg = items.Skip(1).ToArray();
            trace.Error(fmt, arg);
        }

        [ScriptFunction("assert")]
        public void Assert(bool expression, params object[] items)
        {
            Contract.Requires<ArgumentNullException>(items != null);
        }

        [ScriptFunction("clear")]
        public void Clear(params object[] items)
        {
            Contract.Requires<ArgumentNullException>(items != null);
        }

        [ScriptFunction("group")]
        public void Group(params object[] items)
        {
            Contract.Requires<ArgumentNullException>(items != null);
        }

        [ScriptFunction("groupCollapsed")]
        public void GroupCollapsed(params object[] items)
        {
            Contract.Requires<ArgumentNullException>(items != null);
        }

        [ScriptFunction("groupEnd")]
        public void GroupEnd()
        {

        }

        [ScriptFunction("time")]
        public void Time(string name = "")
        {

        }

        [ScriptFunction("timeEnd")]
        public void TimeEnd(string name = "")
        {

        }

    }

}
