using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(realtime_log4net_logs_viewer.Startup))]
namespace realtime_log4net_logs_viewer
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            // SignalR configuration
            app.MapSignalR(new HubConfiguration {
                Resolver = GlobalHost.DependencyResolver,
                EnableDetailedErrors = true
            });

            // start the log receiver
            System.App_Start.Logs.Configuration(app);
        }
    }
}
