using Microsoft.AspNet.SignalR;
using NotificationService.Dtos;
using NotificationService.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace NotificationService.Controllers
{
    public class NotificationController : ApiController
    {
        public IHubContext NotificationHub { get; set; }
        public NotificationController()
        {
            NotificationHub = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
        }

        [HttpPost]
        public void Send(NotificationDto notification)
        {
            NotificationHub.Clients.All.Notification(notification);
        }
    }
}
