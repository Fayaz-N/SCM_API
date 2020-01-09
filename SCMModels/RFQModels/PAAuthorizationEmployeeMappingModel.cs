using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMModels.RFQModels
{
   public class PAAuthorizationEmployeeMappingModel
    {
        public int PAmapid { get; set; }
        public int Authid { get; set; }
        public string Employeeid { get; set; }
        public string Employeename { get; set; }
        public string FunctionalRoleId { get; set; }
        public bool LessBudget { get; set; }
        public bool MoreBudget { get; set; }
        public byte AuthLevel { get; set; }
        public bool DeleteFlag { get; set; }
        public string CreatedBY { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public  PAAuthorizationLimitModel PAAuthorizationLimit { get; set; }
        public PAFunctionalRolesModel mpaiing { get; set; }
    }
}
