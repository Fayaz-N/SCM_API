using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMModels.RFQModels
{
   public class UnitMasterModel
    {
        public byte UnitID { get; set; }
        public string UnitName { get; set; }
        public Nullable<bool> DeleteFlag { get; set; }
    }
}
