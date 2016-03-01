using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Newtonsoft.Json.Linq;
using Owin;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace System.App_Start
{
    public static class Logs
    {
        /// <summary>
        /// This startup configuration is executed slightly latter than Global.asax
        /// </summary>
        public static void Configuration(IAppBuilder app) {
            ThreadPool.QueueUserWorkItem(delegate {
                try {
                    var sender = new IPEndPoint(IPAddress.Any, 0);

                    var listenPort = int.Parse(ConfigurationManager.AppSettings["LogsReceiverPort"]);

                    using (var client = new UdpClient(listenPort)) {
                        disposeWhenShutdown(app, client);

                        while (true) {
                            var buffer = client.Receive(ref sender);

                            var stringLoggingEvent = System.Text.Encoding.Default.GetString(buffer);

                            sendSignalR(stringLoggingEvent);
                        }
                    }
                }
                catch (Exception ex) {
                    log4net.LogManager.GetLogger(System.Reflection
                        .MethodBase.GetCurrentMethod().DeclaringType).Error(ex);
                }
            }, null);
        }

        private static void sendSignalR(string stringLoggingEvent) {
            var hub = GlobalHost.ConnectionManager.GetHubContext<SignalrAppenderHub>();

            if (hub != null) {
                dynamic loggingEvent = JObject.Parse(stringLoggingEvent);

                hub.Clients.All.onLoggedEvent(new {
                    loggingEvent.TimeStamp,
                    Level = loggingEvent.Level.Name,
                    Hostname = loggingEvent.Properties["log4net:HostName"],
                    loggingEvent.LoggerName,
                    loggingEvent.ThreadName,
                    loggingEvent.Identity,
                    loggingEvent.Domain,
                    loggingEvent.UserName,
                    loggingEvent.Message,
                    loggingEvent.ExceptionString
                });
            }
        }

        private static void disposeWhenShutdown(IAppBuilder app, UdpClient client) {
            var context = new OwinContext(app.Properties);

            var token = context.Get<CancellationToken>("host.OnAppDisposing");

            if (token != CancellationToken.None) {
                token.Register(() => {
                    client.Close();
                });
            }
        }
    }

    public class SignalrAppenderHub : Hub { }
}