using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMModels.MPRMasterModels
{
   public class GlobalGroupEmployeeModel
    {
        public short GlobalGroupId { get; set; }
        public string EmployeeNo { get; set; }
        public System.DateTime UpdatedOn { get; set; }
        public virtual GlobalGroupModel GlobalGroup { get; set; }
    }
}
