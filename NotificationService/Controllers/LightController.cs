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
    /// <summary>
    /// 亮灯控制
    /// </summary>
    public class LightController : ApiController
    {
        public IHubContext HubContext { get; set; }

        public LightController()
        {
            HubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
        }

        /// <summary>
        /// 小灯 单灯或者多灯
        /// </summary>
        /// <param name="storageLight">灯指令合集</param>
        [HttpPost]
        public void LightOrder(List<StorageLight> storageLight)
        {
            foreach (var key in NotificationHub.Users.Keys)
            {
                var loN = storageLight.Where(l => key.Data.Split(';').Select(r => int.Parse(r)).Contains(l.MainBoardId)).Distinct().ToArray();
                if (loN.Length > 0)
                {
                    HubContext.Clients.Client(NotificationHub.Users[key]).LightOrder(loN);
                }
            }
        }

        /// <summary>
        /// 灯塔
        /// </summary>
        /// <param name="houseLights">灯指令</param>
        [HttpPost]
        public void HouseOrder(List<HouseLight> houseLights)
        {
            foreach (var key in NotificationHub.Users.Keys)
            {
                var loN = houseLights.Where(l => key.Data.Split(';').Select(r => int.Parse(r)).Contains(l.MainBoardId)).Distinct().ToArray();
                if (loN.Length > 0)
                {
                    HubContext.Clients.Client(NotificationHub.Users[key]).HouseOrder(loN);
                }
            }
        }

        /// <summary>
        /// 整个货架控制
        /// </summary>
        /// <param name="allLightOrders">灯指令</param>
        [HttpPost]
        public void AllLightOrder(List<AllLight> allLightOrders)
        {
            foreach (var key in NotificationHub.Users.Keys)
            {
                var loN = allLightOrders.Where(l => key.Data.Split(';').Select(r => int.Parse(r)).Contains(l.MainBoardId)).Distinct().ToArray();
                if (loN.Length > 0)
                {
                    HubContext.Clients.Client(NotificationHub.Users[key]).AllLightOrder(loN);
                }
            }
        }
    }
}
