using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMModels.RFQModels
{
   public class MPRPAPurchaseModesModel
    {
        public byte PurchaseModeId { get; set; }
        public string PurchaseMode { get; set; }
        public byte XOrder { get; set; }
        public bool BoolInUse { get; set; }
    }
}
