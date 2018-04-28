using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Dtos
{
    public class RBBatchReadyMBillDto
    {
        public string Id { get; set; }

        public bool IsActive { get; set; }

        public string ProductId { get; set; }

        public string LineId { get; set; }

        public string WorkBillId { get; set; }

        public int WorkQty { get; set; }

        public string PartNoId { get; set; }

        public int PartNoQty { get; set; }

        public string ReelMoveMethodId { get; set; }

        public string ForCustomerMStorageId { get; set; }

        public string ForSelfMStorageId { get; set; }
    }
}
