using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMModels.RFQModels
{
   public class MPRPAPurchaseTypesModel
    {
        public byte PurchaseTypeId { get; set; }
        public string PurchaseType { get; set; }
        public byte XOrder { get; set; }
        public bool BoolInUse { get; set; }
    }
}
