using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMModels.RFQModels
{
   public class PACreditDaysApproverModel
    {
        public int CRApprovalId { get; set; }
        public int AuthId { get; set; }
        public string EmployeeNo { get; set; }
        public int CreditdaysId { get; set; }
        public string Createdby { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public bool DeleteFlag { get; set; }
        public string DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }

        public  PAAuthorizationLimitModel PAAuthorizationLimit { get; set; }
    }
}
