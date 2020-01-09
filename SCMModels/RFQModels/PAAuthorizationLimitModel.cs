using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMModels.RFQModels
{
   public class PAAuthorizationLimitModel
    {
        public PAAuthorizationLimitModel()
        {
            PAAuthorizationEmployeeMappings = new List<PAAuthorizationEmployeeMappingModel>();
            PACreditDaysApprovers = new List<PACreditDaysApproverModel>();
        }
        public int Authid { get; set; }
        public int DeptId { get; set; }
        public decimal MinPAValue { get; set; }
        public decimal MaxPAValue { get; set; }
        public string AuthorizationType { get; set; }
        public bool DeleteFlag { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string DeletedBY { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public  List<PAAuthorizationEmployeeMappingModel> PAAuthorizationEmployeeMappings { get; set; }
        public  List<PACreditDaysApproverModel> PACreditDaysApprovers { get; set; }
    }
}
