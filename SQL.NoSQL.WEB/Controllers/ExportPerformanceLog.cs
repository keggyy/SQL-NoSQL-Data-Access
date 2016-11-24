using Microsoft.AspNet.SignalR;

namespace SQL.NoSQL.WEB.Controllers
{
    public class ExportPerformanceLog : Hub
    {


        public static void Send(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            var context = GlobalHost.ConnectionManager.GetHubContext<ExportPerformanceLog>();
            context.Clients.All.broadcastMessage(name, message);
        }

    }
}