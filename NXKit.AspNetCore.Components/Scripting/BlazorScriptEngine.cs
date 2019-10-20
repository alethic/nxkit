using System;

using Microsoft.JSInterop;

using NXKit.Composition;
using NXKit.Diagnostics;
using NXKit.IO.Media;
using NXKit.Scripting;

namespace NXKit.View.Razor.Scripting
{

    /// <summary>
    /// Provides a ECMAScript implementation using the Blazor JS interop.
    /// </summary>
    [Export(typeof(IScriptEngine), CompositionScope.Host)]
    public class BlazorScriptEngine : IScriptEngine, IDisposable
    {

        static readonly MediaRangeList ACCEPT = new MediaRange[]
        {
            "application/ecmascript",
            "application/javascript",
            "text/javascript",
            "application/x-javascript",
        };


        readonly Lazy<IJSRuntime> js;
        readonly ITraceService trace;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="js"></param>
        public BlazorScriptEngine(Lazy<IJSRuntime> js, ITraceService trace)
        {
            this.js = js ?? throw new ArgumentNullException(nameof(js));
            this.trace = trace ?? throw new ArgumentNullException(nameof(trace));
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
                trace.Debug("Executing JavaScript: {Code}", code);
                js.Value.InvokeVoidAsync("NxJsExec", code).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception e)
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
                trace.Debug("Evaluating JavaScript: {Code}", code);
                return js.Value.InvokeAsync<object>("NxJsEval", code).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                throw new ScriptException(e.Message, e);
            }
        }

        public void Load()
        {

        }

        public void Save()
        {

        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

    }

}
