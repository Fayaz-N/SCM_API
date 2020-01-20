using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMModels.RFQModels
{
   public class RFQMPRMappingModel
    {
        public int RFQMPRMappingId { get; set; }
        public int RfqItemid { get; set; }
        public Nullable<int> RFQSplitItemId { get; set; }
        public int MPRItemDetailsId { get; set; }
        public bool DeleteFlag { get; set; }

        public  MPRItemInfoModel MPRItemInfo { get; set; }
        public  RfqItemModel RFQItems_N { get; set; }
    }
}
