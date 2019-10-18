using System;
using System.Collections;
using System.Linq;

using Autofac;

using Cogito.Autofac;

using Microsoft.Extensions.DependencyInjection;

using NUnit.Framework;

using NXKit.Composition;
using NXKit.Extensions.DependencyInjection;

namespace NXKit.Testing
{

    [TestFixtureSource(typeof(NXKitTestFixtureDataSource))]
    public abstract partial class NXKitTestFixture
    {

        class NXKitTestFixtureDataSource : IEnumerable
        {

            static ICompositionContext CreateAutofacContext()
            {
                var bld = new ContainerBuilder();
                bld.RegisterAllAssemblyModules();
                var cnt = bld.Build();
                return cnt.Resolve<ICompositionContext>();
            }

            static ICompositionContext CreateMicrosoftDiContext()
            {
                var svc = new ServiceCollection();
                svc.AddNXKit();
                var prv = svc.BuildServiceProvider();
                return prv.GetRequiredService<ICompositionContext>();
            }

            static readonly (string, Func<ICompositionContext>)[] data = new (string, Func<ICompositionContext>)[]
            {
                ("Autofac", CreateAutofacContext),
                ("Microsoft", CreateMicrosoftDiContext),
            };

            public IEnumerator GetEnumerator()
            {
                return data.Select(i => GetData(i.Item1, i.Item2)).GetEnumerator();
            }

            TestFixtureData GetData(string name, Func<ICompositionContext> getCompositionContext)
            {
                var c = new NXKitTestFixtureContext(name, getCompositionContext);
                var d = new TestFixtureData(c);
                d.SetArgDisplayNames(c.GetDisplayNames());
                return d;
            }

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public NXKitTestFixture(NXKitTestFixtureContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public NXKitTestFixtureContext Context { get; }

    }

}