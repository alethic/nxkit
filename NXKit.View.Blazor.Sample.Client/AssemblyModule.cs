using Autofac;

using Cogito.Autofac;

namespace NXKit.View.Blazor.Sample.Client
{

    public class AssemblyModule : ModuleBase
    {

        protected override void Register(ContainerBuilder builder)
        {
            builder.RegisterFromAttributes(typeof(AssemblyModule).Assembly);
        }

    }

}
