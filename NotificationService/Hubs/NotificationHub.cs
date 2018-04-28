using Microsoft.AspNet.SignalR;
using NotificationService.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Hubs
{
    public class NotificationHub : Hub
    {
        public void Hello()
        {
            Clients.Client(Context.ConnectionId).Hello();
        }

        public static Dictionary<HubClientInfo, string> Users = new Dictionary<HubClientInfo, string>();

        public void Login(HubClientInfo hubInfo)
        {
            var key = Users.Keys.FirstOrDefault(k => k.Name == hubInfo.Name);
            if (key == null)
            {
                Users.Add(hubInfo, Context.ConnectionId);
            }
            else
            {
                Users[key] = Context.ConnectionId;
            }

            Clients.Client(Context.ConnectionId).Hello();
        }

        public void LightOrder(StorageLight[] lo)
        {

            // Clients.All.InvokeAsync("LightOrder", lo);

            foreach (var key in Users.Keys)
            {
                var loN = lo.Where(l => key.Data.Split(';').Select(r => int.Parse(r)).Contains(l.MainBoardId)).Distinct().ToArray();
                if (loN.Length > 0)
                {
                    Clients.Client(Users[key]).LightOrder(loN);
                }
            }

        }

        public void HouseOrder(HouseLight[] ho)
        {

            // Clients.All.InvokeAsync("HouseOrder", ho);

            // 按主板分组
            foreach (var key in Users.Keys)
            {
                var loN = ho.Where(l => key.Data.Split(';').Select(r => int.Parse(r)).Contains(l.MainBoardId)).Distinct().ToArray();
                if (loN.Length > 0)
                {
                    Clients.Client(Users[key]).HouseOrder(loN);
                }
            }
        }

        public void AllLightOrder(AllLight[] ao)
        {

            // Clients.All.InvokeAsync("AllLightOrder", ao);

            // 按主板分组
            foreach (var key in Users.Keys)
            {
                var loN = ao.Where(l => key.Data.Split(';').Select(r => int.Parse(r)).Contains(l.MainBoardId)).Distinct().ToArray();
                if (loN.Length > 0)
                {
                    Clients.Client(Users[key]).AllLightOrder(loN);
                }
            }
        }
    }
}
