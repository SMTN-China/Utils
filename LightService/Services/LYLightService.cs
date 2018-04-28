using LightService.Dtos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LightService.Services
{
    public class LYLightService
    {
        public LYLightUtils LightUtils { get; set; }

        public NLog.ILogger Log { get; set; }


        public LYLightService()
        {
            if (LightControl == null)
            {
                LightControl = new Task(() =>
                {
                    while (true)
                    {
                        if (order.Count > 0)
                        {
                            byte[] orderOne = order.Dequeue();
                            try
                            {
                                SendComOrder(orderOne);
                            }
                            catch
                            {
                                Thread.Sleep(1000);
                                SendComOrder(orderOne);
                            }
                        }
                        Thread.Sleep(320);
                    }

                });

                LightControl.Start();
            }
        }

        Task LightControl { get; set; }

        Queue<byte[]> order = new Queue<byte[]>();

        SerialPort Port { get; set; }

        bool SendComOrder(byte[] comOrder)
        {
            try
            {
                Port = new SerialPort(ConfigurationManager.AppSettings["SerialPort"], 19200, Parity.None, 8, StopBits.One);
                if (!Port.IsOpen)
                    Port.Open();
                // byte[] bytes = ChangeToByte(comOrder);
                Port.Write(comOrder, 0, comOrder.Length);
                Port.Close();

                if (ConfigurationManager.AppSettings["WriteDengOK"] == "0")
                {
                    Log.Info(comOrder);
                }
                return true;
            }
            catch (Exception ee)
            {
                Log.Error(ee);
                return false;
            }

        }
        byte[] ChangeToByte(string data)
        {
            data = data.Replace(" ", "");
            return Enumerable.Range(0, data.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(data.Substring(x, 2), 16))
                             .ToArray();
        }


        public void HouseOrder(List<HouseLight> houseLights)
        {
            try
            {
                houseLights = houseLights.GroupBy(r => new
                {
                    r.HouseLightSide,
                    r.LightOrder,
                    r.MainBoardId,
                    r.LightColor
                }).Select(r => new HouseLight
                {
                    HouseLightSide = r.Key.HouseLightSide,
                    MainBoardId = r.Key.MainBoardId,
                    LightOrder = r.Key.LightOrder,
                    LightColor = r.Key.LightColor
                }).ToList();


                houseLights.ForEach(s =>
                {
                    if (s.LightColor > 0)
                    {
                        order.Enqueue(LightUtils.GetLightHouseOrder_RGB(s.MainBoardId.ToString(), s.LightOrder.ToString(), s.LightColor));
                    }
                    else
                    {
                        order.Enqueue(ChangeToByte(LightUtils.GetLightHouseOrder(s.MainBoardId, s.LightOrder, s.HouseLightSide)));
                    }
                }
                );
            }
            catch (Exception ee)
            {
                Log.Error(ee);
            }
        }


        public void AllLightOrder(List<AllLight> allLightOrders)
        {
            try
            {
                allLightOrders = allLightOrders.GroupBy(r => new
                {
                    r.LightOrder,
                    r.MainBoardId,
                    r.LightColor
                }).Select(r => new AllLight
                {
                    MainBoardId = r.Key.MainBoardId,
                    LightOrder = r.Key.LightOrder,
                    LightColor = r.Key.LightColor

                }).ToList();
                allLightOrders.ForEach(s =>
                {
                    if (s.LightColor > 0)
                    {
                        order.Enqueue(LightUtils.GetMoreLightsOrder_RGB(s.MainBoardId, s.LightOrder, s.LightColor));
                    }
                    else
                    {
                        order.Enqueue(ChangeToByte(LightUtils.GetMoreLightsOrder(s.MainBoardId, s.LightOrder)));
                    }
                });
            }
            catch (Exception ee)
            {
                Log.Error(ee);
            }
        }

        public void LightOrder(List<StorageLight> lights)
        {
            try
            {
                lights = lights.GroupBy(r => new
                {
                    r.LightOrder,
                    r.ContinuedTime,
                    r.RackPositionId,
                    r.MainBoardId,
                    r.LightColor
                }).Select(r => new StorageLight
                {
                    RackPositionId = r.Key.RackPositionId,
                    ContinuedTime = r.Key.ContinuedTime,
                    MainBoardId = r.Key.MainBoardId,
                    LightOrder = r.Key.LightOrder,
                    LightColor = r.Key.LightColor
                }).ToList();
                if (lights.Count == 1)
                {
                    if (lights[0].LightColor > 0)
                    {
                        order.Enqueue(LightUtils.GetOneLightOrder_RGB(lights[0].MainBoardId.ToString().PadLeft(3, '0') + lights[0].RackPositionId.ToString().PadLeft(3, '0'), lights[0].LightOrder.ToString(), lights[0].LightColor, "5"));
                    }
                    else
                    {
                        order.Enqueue(ChangeToByte(LightUtils.GetOneLightOrder(lights[0].LightOrder, 10, 10, lights[0].ContinuedTime, lights[0].MainBoardId, lights[0].RackPositionId)));
                    }
                }
                else
                {
                    #region 多灯

                    //全部闪灯
                    List<StorageLight> lightsFlash = lights.FindAll(s => s.LightOrder == 2);
                    //全部亮灭灯
                    List<StorageLight> lightsNoFlash = lights.FindAll(s => s.LightOrder == 1 || s.LightOrder == 0);

                    foreach (var rack in lightsNoFlash.GroupBy(s => new { s.MainBoardId, s.LightColor }))
                    {
                        Dictionary<int, int> dic = new Dictionary<int, int>();
                        foreach (var library in rack)
                        {
                            if (dic.Keys.Contains(library.RackPositionId))
                                dic[library.RackPositionId] = library.LightOrder;
                            else
                                dic.Add(library.RackPositionId, library.LightOrder);
                        }

                        //添加多灯命令
                        if (rack.Key.LightColor == 0)
                        {
                            order.Enqueue(ChangeToByte(LightUtils.GetMoreLightsOrder(dic, rack.Key.MainBoardId)));
                        }
                        else
                        {
                            order.Enqueue(LightUtils.GetMoreLightsOrder_RGB(dic, rack.Key.MainBoardId, rack.Key.LightColor, "1"));
                        }
                    }

                    //添加闪灯命令
                    lightsFlash.ForEach(s =>
                    {
                        if (s.LightColor == 0)
                        {
                            order.Enqueue(ChangeToByte(LightUtils.GetOneLightOrder(s.LightOrder, 10, 10, s.ContinuedTime, s.MainBoardId, s.RackPositionId))
                                                    );
                        }
                        else
                        {
                            order.Enqueue(LightUtils.GetOneLightOrder_RGB(s.MainBoardId.ToString().PadLeft(3, '0') + s.RackPositionId.ToString().PadLeft(3, '0'), s.LightOrder.ToString(), s.LightColor, "5"));
                        }

                    });


                    #endregion
                }

            }
            catch (Exception ee)
            {
                Log.Error(ee);
            }
        }
    }
}
