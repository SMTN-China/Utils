using LightService.Dtos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightService
{
    public class LYLightUtils
    {
        public NLog.ILogger Log { get; set; }
        /// <summary>
        /// 拼接单灯命令
        /// </summary>
        /// <param name="ldms">工作模式,0熄灭,1恒亮,2闪烁</param>
        /// <param name="ldsj">亮灯时间,建议10,单位秒</param>
        /// <param name="mdsj">熄灭时间,建议10,单位秒</param>
        /// <param name="sdcs">闪亮次数,建议10</param>
        /// <param name="zbid">主板ID</param>
        /// <param name="kwh">库位号</param>
        /// <returns>单灯命令字符串</returns>
        public string GetOneLightOrder(int ldms, int ldsj, int mdsj, int sdcs, int zbid, int kwh)
        {
            StringBuilder kuweiarray = new StringBuilder();
            kuweiarray.Append("AA55");
            kuweiarray.Append(Convert.ToInt32(zbid).ToString("x").PadLeft(4, '0'));
            kuweiarray.Append(ldms.ToString().PadLeft(2, '0'));
            int KWH = kwh - 1;
            if (KWH >= 700)
            {
                kuweiarray.Append((KWH - 700).ToString("x").PadLeft(4, '0'));
            }
            else
            {
                kuweiarray.Append(KWH.ToString("x").PadLeft(4, '0'));
            }
            kuweiarray.Append(ldsj.ToString("x").PadLeft(4, '0'));
            kuweiarray.Append(mdsj.ToString("x").PadLeft(4, '0'));
            kuweiarray.Append(sdcs.ToString("x").PadLeft(4, '0'));
            kuweiarray.Append("000100");
            return kuweiarray.ToString();
        }

        public byte[] GetOneLightOrder_RGB(string HW_CODE,string Action,LightColor Color,string Cycle)
        {


            //StringBuilder kuweiarray = new StringBuilder();
            //kuweiarray.Append("AA55");
            //kuweiarray.Append(Convert.ToInt32(zbid).ToString("x").PadLeft(4, '0'));
            //kuweiarray.Append(ldms.ToString().PadLeft(2, '0'));
            //int KWH = kwh - 1;
            //if (KWH >= 700)
            //{
            //    kuweiarray.Append((KWH - 700).ToString("x").PadLeft(4, '0'));
            //}
            //else
            //{
            //    kuweiarray.Append(KWH.ToString("x").PadLeft(4, '0'));
            //}
            //kuweiarray.Append(ldsj.ToString("x").PadLeft(4, '0'));
            //kuweiarray.Append(mdsj.ToString("x").PadLeft(4, '0'));
            //kuweiarray.Append(sdcs.ToString("x").PadLeft(4, '0'));
            //kuweiarray.Append("000100");

            switch (Color)
            {
                case LightColor.Default:
                    break;
                case LightColor.Green:
                    break;
                case LightColor.Red:
                    return reelshelfcontrol_RGB.LEDcontrol.SingleLight(HW_CODE, "R", Action, Int32.Parse(Cycle));
                case LightColor.Blue:
                    return reelshelfcontrol_RGB.LEDcontrol.SingleLight(HW_CODE, "B", Action, Int32.Parse(Cycle));
                case LightColor.White:
                    break;
                case LightColor.Yellow:
                    break;
                case LightColor.Violet:
                    break;
                case LightColor.Cyan:
                    break;
                default:
                    break;
            }
            return reelshelfcontrol_RGB.LEDcontrol.SingleLight(HW_CODE, "G", Action, Int32.Parse(Cycle));
        }



        /// <summary>
        /// 获取灯塔命令
        /// </summary>
        /// <param name="zbid">主板ID</param>
        /// <param name="workingMode">工作模式：0熄灭；1恒亮；2闪亮</param>      
        /// <param name="side">边别1：右 0：左</param>
        /// <returns></returns>
        public string GetLightHouseOrder(int zbid, int workingMode, int side)
        {
            return (((side == 0) ? "AA53" : "AA54") + Convert.ToString(zbid, 16).PadLeft(4, '0').ToUpper() + "000000000000000000" + (workingMode == 2 ? "AAAA" : (workingMode == 1 ? "FFFF" : "0000")) + "00");
        }

        public byte[] GetLightHouseOrder_RGB(string MBID, string Action, LightColor Color)
        {
            //return (((side == 0) ? "AA53" : "AA54") + Convert.ToString(zbid, 16).PadLeft(4, '0').ToUpper() + "000000000000000000" + (workingMode == 2 ? "AAAA" : (workingMode == 1 ? "FFFF" : "0000")) + "00");
            switch (Color)
            {
                case LightColor.Default:
                    break;
                case LightColor.Green:
                    break;
                case LightColor.Red:
                    return reelshelfcontrol_RGB.LEDcontrol.LightHouse(MBID, "R", Action);
                case LightColor.Blue:
                    return reelshelfcontrol_RGB.LEDcontrol.LightHouse(MBID, "B", Action);
                case LightColor.White:
                    break;
                case LightColor.Yellow:
                    break;
                case LightColor.Violet:
                    break;
                case LightColor.Cyan:
                    break;
                default:
                    break;
            }
            return reelshelfcontrol_RGB.LEDcontrol.LightHouse(MBID, "G", Action);
        }


        /// <summary>
        /// 获取多灯命令
        /// </summary>
        /// <param name="dic">灯的集合,第一个int为库位号,第二个LightOrder为枚举,指示当前灯的操作(亮还是灭)</param>
        /// <param name="mainBoardID">主板ID</param>
        /// <returns>多灯命令的字符串</returns>
        public string GetMoreLightsOrder(Dictionary<int, int> dic, int mainBoardID)
        {
            string str = "00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000";
            foreach (var kwh in dic.Keys)
            {
                Group group = QueryGroup(kwh > 700 ? (kwh - 700) : kwh);       //获取灯的详细信息
                string nowBinary = Convert.ToString(Convert.ToInt32(str[group.GroupIndex].ToString(), 16), 2).PadLeft(4, '0');      //当前灯所在组的灯状态二进制组合命令
                char[] arryChar = nowBinary.ToCharArray();      //将二进制组合命令拆分成单灯的命令组               
                arryChar[group.OnGroupIndex] = dic[kwh].ToString()[0];     //将单灯的命令组中当前灯的位置的灯状态改变成命令值                  
                nowBinary = new string(arryChar);       //重新拼接当前灯所在组的灯状态二进制组合命令
                arryChar = str.ToCharArray();       //获取所有的十六进制命令组
                arryChar[group.GroupIndex] = Convert.ToString(Convert.ToInt32(nowBinary, 2), 16)[0];        //将当前灯所在十六进制命令组更新为新的命令组(由新的二进制字符串转化成十六进制而得到)
                str = new string(arryChar).ToUpper();     //重新拼接命令组
            }
            return "55AA" + mainBoardID.ToString("X").PadLeft(4, '0') + str + "88";     //拼接码头编号及其他固定格式
        }

        public byte[] GetMoreLightsOrder_RGB(Dictionary<int, int> dic, int mainBoardID, LightColor Color,string Action)
        {
            //string str = "00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000";
            ArrayList HW_CodeList = new ArrayList();
            foreach (var kwh in dic.Keys)
            {
                HW_CodeList.Add(mainBoardID.ToString().PadLeft(3, '0') + (kwh > 700 ? (kwh - 700).ToString() :
                            kwh.ToString()).PadLeft(3, '0'));
                //Group group = QueryGroup(kwh > 700 ? (kwh - 700) : kwh);       //获取灯的详细信息
                //string nowBinary = Convert.ToString(Convert.ToInt32(str[group.GroupIndex].ToString(), 16), 2).PadLeft(4, '0');      //当前灯所在组的灯状态二进制组合命令
                //char[] arryChar = nowBinary.ToCharArray();      //将二进制组合命令拆分成单灯的命令组               
                //arryChar[group.OnGroupIndex] = dic[kwh].ToString()[0];     //将单灯的命令组中当前灯的位置的灯状态改变成命令值                  
                //nowBinary = new string(arryChar);       //重新拼接当前灯所在组的灯状态二进制组合命令
                //arryChar = str.ToCharArray();       //获取所有的十六进制命令组
                //arryChar[group.GroupIndex] = Convert.ToString(Convert.ToInt32(nowBinary, 2), 16)[0];        //将当前灯所在十六进制命令组更新为新的命令组(由新的二进制字符串转化成十六进制而得到)
                //str = new string(arryChar).ToUpper();     //重新拼接命令组
            }
            //return "55AA" + mainBoardID.ToString("X").PadLeft(4, '0') + str + "88";     //拼接码头编号及其他固定格式
            switch (Color)
            {
                case LightColor.Default:
                    break;
                case LightColor.Green:
                    break;
                case LightColor.Red:
                    return reelshelfcontrol_RGB.LEDcontrol.MultiLight(HW_CodeList, "R", Action);
                case LightColor.Blue:
                    return reelshelfcontrol_RGB.LEDcontrol.MultiLight(HW_CodeList, "B", Action);
                case LightColor.White:
                    break;
                case LightColor.Yellow:
                    break;
                case LightColor.Violet:
                    break;
                case LightColor.Cyan:
                    break;
                default:
                    break;
            }
            return reelshelfcontrol_RGB.LEDcontrol.MultiLight(HW_CodeList, "G", Action);
        }


        /// <summary>
        /// 多灯命令全亮或全灭
        /// </summary>
        /// <param name="zbid">主板id</param>
        /// <param name="lightOrder">指令方式</param>
        /// <returns></returns>
        public string GetMoreLightsOrder(int zbid, int lightWorkType)
        {
            switch (lightWorkType)
            {
                case 1:
                    return "55AA" + zbid.ToString("x").PadLeft(4, '0') + "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF88";
                case 0:
                    return "55AA" + zbid.ToString("x").PadLeft(4, '0') + "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000088";
                default:
                    break;
            }
            return "";
        }

        public byte[] GetMoreLightsOrder_RGB(int zbid, int lightWorkType, LightColor Color)
        {
            switch (lightWorkType)
            {
                case 1:
                    switch (Color)
                    {
                        case LightColor.Default:
                            break;
                        case LightColor.Green:
                            break;
                        case LightColor.Red:
                            return reelshelfcontrol_RGB.LEDcontrol.AllOn(zbid.ToString(), "R");
                        case LightColor.Blue:
                            return reelshelfcontrol_RGB.LEDcontrol.AllOn(zbid.ToString(), "B");
                        case LightColor.White:
                            break;
                        case LightColor.Yellow:
                            break;
                        case LightColor.Violet:
                            break;
                        case LightColor.Cyan:
                            break;
                        default:
                            break;
                    }
                    return reelshelfcontrol_RGB.LEDcontrol.AllOn(zbid.ToString(), "G");
                    //return "55AA" + zbid.ToString("x").PadLeft(4, '0') + "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF88";
                case 0:
                    switch (Color)
                    {
                        case LightColor.Default:
                            break;
                        case LightColor.Green:
                            break;
                        case LightColor.Red:
                            return reelshelfcontrol_RGB.LEDcontrol.AllOff(zbid.ToString(), "R");
                        case LightColor.Blue:
                            return reelshelfcontrol_RGB.LEDcontrol.AllOff(zbid.ToString(), "B");
                        case LightColor.White:
                            break;
                        case LightColor.Yellow:
                            break;
                        case LightColor.Violet:
                            break;
                        case LightColor.Cyan:
                            break;
                        default:
                            break;
                    }
                    Log.Info("多灯亮");
                    return reelshelfcontrol_RGB.LEDcontrol.AllOff(zbid.ToString(), "G");
                    //return "55AA" + zbid.ToString("x").PadLeft(4, '0') + "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000088";
                default:
                    break;
            }
            return new byte[0];

        }

        /// <summary>
        /// 获取当前值详细信息
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        Group QueryGroup(int value)
        {
            Group group = new Group();
            int temp = (value - 1) / 4;
            temp++;
            if (temp % 2 == 0)
                temp = temp - 1;
            else
                temp = temp + 1;
            group.GroupIndex = temp - 1;
            int mod = value % 4 == 0 ? 4 : value % 4;
            group.OnGroupIndex = 4 - mod;
            group.CurCount = value;
            return group;
        }

        class Group
        {

            /// <summary>
            /// 组位置
            /// </summary>
            public int GroupIndex;
            /// <summary>
            /// 在组内的位置
            /// </summary>
            public int OnGroupIndex;
            /// <summary>
            /// 当前值
            /// </summary>
            public int CurCount;
        }
    }
}
