namespace NXKit.Scripting.EcmaScript
{

    [ScriptEngine]
    public class V8ScriptEngine :
        IScriptEngine
    {

        public bool CanExecute(string type, string code)
        {
            return type == "text/javascript";
        }

        public object Execute(string code)
        {
            using (var v8 = new Microsoft.ClearScript.V8.V8ScriptEngine())
            {
                v8.Execute(code);
            }

            return null;
        }


    }

}
