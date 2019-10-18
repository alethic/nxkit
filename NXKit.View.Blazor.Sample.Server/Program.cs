using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace NXKit.View.Blazor.Sample.Server
{

    public static class Program
    {

        public static void Main(string[] args) => WebHost.CreateDefaultBuilder(args)
            .UseConfiguration(new ConfigurationBuilder()
                .AddCommandLine(args)
                .Build())
            .UseStartup<Startup>()
            .Build()
            .Run();

    }

}
