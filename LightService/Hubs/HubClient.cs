using LightService.Dtos;
using LightService.Hubs;
using LightService.Services;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LightService
{
    public class HubClient
    {
        HubConnection hubConnection { get; set; }
        IHubProxy hubProxy { get; set; }

        public LYLightService LYLightService { get; set; }

        public NLog.ILogger Log { get; set; }


        Task thread { get; set; }


        public HubClient()
        {
            thread = new Task(() =>
            {
                while (true)
                {
                    try
                    {
                        if (hubConnection == null)
                        {
                            hubConnection = new HubConnection(ConfigurationManager.AppSettings["SignalRService"]);

                            hubProxy = hubConnection.CreateHubProxy("NotificationHub");

                            hubProxy.On("LightOrder", new Action<StorageLight[]>(sl =>
                            {


                                // 获取立于货架集合
                                var shelfLY = sl.Where(l => l.MainBoardId.ToString().Length < 4);

                                // 货架亮灯
                                if (shelfLY.Count() > 0)
                                {

                                    LYLightService.LightOrder(shelfLY.ToList());
                                }
                            }));

                            hubProxy.On("HouseOrder", new Action<HouseLight[]>(hl =>
                            {


                                // 获取立于货架集合
                                var shelfLY = hl.Where(l => l.MainBoardId.ToString().Length < 4);

                                // 货架亮灯
                                if (shelfLY.Count() > 0)
                                {
                                    LYLightService.HouseOrder(shelfLY.ToList());
                                }
                            }));

                            hubProxy.On("AllLightOrder", new Action<AllLight[]>(al =>
                            {

                                // 获取立于货架集合
                                var shelfLY = al.Where(l => l.MainBoardId.ToString().Length < 4);

                                // 货架亮灯
                                if (shelfLY.Count() > 0)
                                {
                                    LYLightService.AllLightOrder(shelfLY.ToList());
                                }
                            }));

                            hubProxy.On("Hello", () =>
                             {
                                 Log.Info("登陆成功,收到服务器回应!");
                             });

                            hubConnection.Start().Wait();

                            hubProxy.Invoke("Login", new HubClientInfo()
                            {
                                Group = "LightService",
                                Name = GetMacAddress(),
                                Data = ConfigurationManager.AppSettings["MainBoards"]
                            }).Wait();

                        }

                        if (hubConnection.State == ConnectionState.Disconnected)
                        {
                            hubConnection.Start().Wait();
                            hubProxy.Invoke("Login", new HubClientInfo()
                            {
                                Group = "LightService",
                                Name = GetMacAddress(),
                                Data = ConfigurationManager.AppSettings["MainBoards"]
                            }).Wait();
                        }
                    }
                    catch
                    {
                    }

                    Thread.Sleep(1000);
                }
            });

        }
        public void Login()
        {
            thread.Start();
        }

        public static string GetMacAddress()
        {
            try
            {
                string strMac = string.Empty;
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        strMac = mo["MacAddress"].ToString();
                    }
                }
                moc = null;
                mc = null;
                return strMac;
            }
            catch
            {
                return "unknown";
            }
        }
    }
}
