using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Dtos
{
    /// <summary>
    /// 整个货架控制
    /// </summary>
    public class AllLight
    {
        /// <summary>
        /// 主板Id
        /// </summary>
        public int MainBoardId { get; set; }
        /// <summary>
        /// 灯颜色
        /// </summary>
        public LightColor LightColor { get; set; }
        /// <summary>
        /// 灯指令
        /// </summary>
        public int LightOrder { get; set; }
    }
    /// <summary>
    /// 灯塔控制
    /// </summary>
    public class HouseLight
    {
        /// <summary>
        /// 主板Id
        /// </summary>
        public int MainBoardId { get; set; }
        /// <summary>
        /// 灯颜色
        /// </summary>
        public LightColor LightColor { get; set; }
        /// <summary>
        /// 灯塔边别
        /// </summary>
        public int HouseLightSide { get; set; }
        /// <summary>
        /// 灯指令
        /// </summary>
        public int LightOrder { get; set; }
    }
    public class LightMsg
    {
        public bool IsOK { get; set; }
        public string Msg { get; set; }
    }

    /// <summary>
    /// 库位灯
    /// </summary>
    public class StorageLight
    {
        /// <summary>
        /// 主板Id
        /// </summary>
        public int MainBoardId { get; set; }
        /// <summary>
        /// 库位Id
        /// </summary>
        public int RackPositionId { get; set; }
        /// <summary>
        /// 灯指令
        /// </summary>
        public int LightOrder { get; set; }
        /// <summary>
        /// 灯颜色
        /// </summary>
        public LightColor LightColor { get; set; }
        /// <summary>
        /// 灯指令
        /// </summary>
        public int ContinuedTime { get; set; }
    }

    /// <summary>
    /// 亮灯指令
    /// </summary>
    public enum LightOrder
    {
        /// <summary>
        /// 亮
        /// </summary>
        LightUp,
        /// <summary>
        /// 灭
        /// </summary>
        LightOff,
        /// <summary>
        /// 闪
        /// </summary>
        LightFlicker
    }
    /// <summary>
    /// 亮灯颜色
    /// </summary>
    public enum LightColor
    {
        Default = 0,
        /// <summary>
        /// 绿
        /// </summary>
        Green,
        /// <summary>
        /// 红
        /// </summary>
        Red,
        /// <summary>
        /// 蓝
        /// </summary>
        Blue,
        /// <summary>
        /// 白
        /// </summary>
        White,
        /// <summary>
        /// 黄
        /// </summary>
        Yellow,
        /// <summary>
        /// 紫
        /// </summary>
        Violet,
        /// <summary>
        /// 青
        /// </summary>
        Cyan
    }
}
