using System;

using Autofac;

namespace NXKit.Autofac
{

    /// <summary>
    /// Registers all the available NXKit assemblies.
    /// </summary>
    public class NXKitModule : Module
    {

        protected override void Load(ContainerBuilder builder)
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));

            if (builder.Properties.ContainsKey(typeof(NXKitModule).FullName) == false)
            {
                builder.RegisterNXKit();
                builder.Properties[typeof(NXKitModule).FullName] = true;
            }
        }

    }

}
