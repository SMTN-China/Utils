using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Dtos
{
    public class MPNDto
    {

        public string Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 描述
        /// </summary>

        public string Info { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        public int ShelfLife { get; set; }
        /// <summary>
        /// 层级
        /// </summary>
        public MPNHierarchy MPNHierarchy { get; set; }
        /// <summary>
        /// 等级
        /// </summary>
        public MPNLevel MPNLevel { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public MPNType MPNType { get; set; }

        /// <summary>
        /// 类型等级
        /// </summary>
        public MSDLevel MSDLevel { get; set; }
        /// <summary>
        /// 来料方式
        /// </summary>
        public IncomingMethod IncomingMethod { get; set; }

        /// <summary>
        /// 原包装数量
        /// </summary>
        public int MPQ1 { get; set; }
        public int MPQ2 { get; set; }
        public int MPQ3 { get; set; }
        public int MPQ4 { get; set; }
        public int MPQ5 { get; set; }

        public string CustomerId { get; set; }
        public bool IsActive { get; set; }
    }
    public enum MPNHierarchy
    {
        Product = 0,
        PartNo
    }

    public enum MPNLevel
    {
        A = 0,
        B,
        C
    }

    public enum MPNType
    {
        Common = 0,
        MSD,
        PCB
    }

    public enum MSDLevel
    {
        Level1 = 0,
        Level2,
        Level3,
        Level4,
        Level5,
    }

    public enum IncomingMethod
    {
        ForCustomer = 0,
        ForSelf,
        Other
    }
}
