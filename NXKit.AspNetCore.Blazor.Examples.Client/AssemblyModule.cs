using Autofac;

using Cogito.Autofac;

namespace NXKit.AspNetCore.Blazor.Examples.Client
{

    public class AssemblyModule : ModuleBase
    {

        protected override void Register(ContainerBuilder builder)
        {
            builder.RegisterFromAttributes(typeof(AssemblyModule).Assembly);
        }

    }

}
