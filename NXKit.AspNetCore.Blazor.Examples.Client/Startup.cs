using Microsoft.AspNetCore.Components.Builder;

namespace NXKit.AspNetCore.Blazor.Examples.Client
{

    public class Startup
    {

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }

    }

}
