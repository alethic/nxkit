using System;
using System.Diagnostics.Contracts;
using Jurassic;
using Jurassic.Library;
using System.Linq;
using NXKit.Diagnostics;

namespace NXKit.Scripting.EcmaScript
{

    public class Console :
        ObjectInstance
    {

        readonly ITraceService trace;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="trace"></param>
        public Console(ScriptEngine engine, ITraceService trace)
            : base(engine)
        {
            Contract.Requires<ArgumentNullException>(engine != null);
            Contract.Requires<ArgumentNullException>(trace != null);

            this.trace = trace;

            PopulateFunctions();
        }

        [JSFunction(Name = "log")]
        public void Log(params object[] items)
        {
            if (items.Length < 1)
                return;

            var fmt = (string)items[0].ToString();
            var arg = items.Skip(1).ToArray();
            trace.Information(fmt, arg);
        }

        [JSFunction(Name = "debug")]
        public void Debug(params object[] items)
        {
            if (items.Length < 1)
                return;

            var fmt = (string)items[0].ToString();
            var arg = items.Skip(1).ToArray();
            trace.Debug(fmt, arg);
        }

        [JSFunction(Name = "info")]
        public void Info(params object[] items)
        {
            if (items.Length < 1)
                return;

            var fmt = (string)items[0].ToString();
            var arg = items.Skip(1).ToArray();
            trace.Information(fmt, arg);
        }

        [JSFunction(Name = "warn")]
        public void Warn(params object[] items)
        {
            if (items.Length < 1)
                return;

            var fmt = (string)items[0].ToString();
            var arg = items.Skip(1).ToArray();
            trace.Warning(fmt, arg);
        }

        [JSFunction(Name = "error")]
        public void Error(params object[] items)
        {
            if (items.Length < 1)
                return;

            var fmt = items[0].ToString();
            var arg = items.Skip(1).ToArray();
            trace.Error(fmt, arg);
        }

        [JSFunction(Name = "assert")]
        public void Assert(bool expression, params object[] items)
        {

        }

        [JSFunction(Name = "clear")]
        public void Clear(params object[] items)
        {

        }

        [JSFunction(Name = "group")]
        public void Group(params object[] items)
        {

        }

        [JSFunction(Name = "groupCollapsed")]
        public void GroupCollapsed(params object[] items)
        {

        }

        [JSFunction(Name = "groupEnd")]
        public void GroupEnd()
        {

        }

        [JSFunction(Name = "time", Flags = JSFunctionFlags.MutatesThisObject)]
        public void Time(string name = "")
        {

        }

        [JSFunction(Name = "timeEnd", Flags = JSFunctionFlags.MutatesThisObject)]
        public void TimeEnd(string name = "")
        {

        }

    }

}
