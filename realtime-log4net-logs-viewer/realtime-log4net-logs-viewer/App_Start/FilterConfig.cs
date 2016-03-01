using System.Web;
using System.Web.Mvc;

namespace realtime_log4net_logs_viewer
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
