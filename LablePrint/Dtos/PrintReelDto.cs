using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LablePrint.Dtos
{
    public class PrintReelDto
    {
        public string Id { get; set; }
        public string Info { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// 打印编号
        /// </summary>
        public string PrintStr { get; set; }
        public int PrintIndex { get; set; }
        /// <summary>
        /// 料号
        /// </summary>
        public string PartNoId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Qty { get; set; }
        /// <summary>
        /// 供应商
        /// </summary>
        public string Supplier { get; set; }
        /// <summary>
        /// 生产日期
        /// </summary>
        public DateTime MakeDate { get; set; }
        /// <summary>
        /// D/C
        /// </summary>
        public string DateCode { get; set; }
        /// <summary>
        /// 大批次号
        /// </summary>
        public string LotCode { get; set; }
        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchCode { get; set; }

        /// <summary>
        /// 收料单
        /// </summary>
        public string ReceivedReelBillId { get; set; }

        /// <summary>
        /// Po
        /// </summary>
        public string PoId { get; set; }
        /// <summary>
        /// 检验单号
        /// </summary>
        public string IQCCheckId { get; set; }

        public int ReelQty { get; set; }
    }
}
