using System;
using System.Linq;

using Autofac;

using Cogito.Reflection;

namespace NXKit.Autofac
{

    /// <summary>
    /// Registers all the available NXKit assemblies.
    /// </summary>
    public class CompositionModule : Module
    {

        protected override void Load(ContainerBuilder builder)
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));

            builder.RegisterNXKitAssemblies(SafeAssemblyLoader.LoadAll().ToArray());
        }

    }

}
