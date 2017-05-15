using System;
using System.Threading.Tasks;
using GameOfLife.Hubs;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;

[assembly: OwinStartup(typeof(GameOfLife.Startup))]

namespace GameOfLife
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            var config = new HubConfiguration
            {
                EnableJSONP = true
            };
            app.MapSignalR(config);
        }
    }
}
