using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Dtos
{
    public class ReelOutLifeDto
    {
        public string Id { get; set; }

        public string PartNoId { get; set; }
        public int Qty { get; set; }
        public string Supplier { get; set; }
        public DateTime MakeDate { get; set; }
        public string DateCode { get; set; }
        public string LotCode { get; set; }
        public string BatchCode { get; set; }

        public bool IsUseed { get; set; }

        public int ExtendShelfLife { get; set; }
        public int ShelfLife { get; set; }


        public string ReadyMBillId { get; set; }
        public string WorkBillDetailedId { get; set; }
        public string ReadyMBillDetailedId { get; set; }
        public string WorkBillId { get; set; }
        public string StorageLocationId { get; set; }
        public string StorageId { get; set; }

        public int? SlotId { get; set; }
        public bool IsActive { get; set; }
    }
}
