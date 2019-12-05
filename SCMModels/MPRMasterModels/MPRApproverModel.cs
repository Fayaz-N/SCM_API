using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMModels.MPRMasterModels
{
   public class MPRApproverModel
    {
        public string EmployeeNo { get; set; }
        public bool BoolActive { get; set; }
        public string DeactivatedBy { get; set; }
        public Nullable<System.DateTime> DeactivatedOn { get; set; }

        public  MPRApproverModel MPRApprovers1 { get; set; }
        public  MPRApproverModel MPRApprover1 { get; set; }
    }
}
