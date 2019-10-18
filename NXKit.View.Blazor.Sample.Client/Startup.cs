using Microsoft.AspNetCore.Components.Builder;

namespace NXKit.View.Blazor.Sample.Client
{

    public class Startup
    {

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }

    }

}
