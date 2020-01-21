using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMModels.RFQModels
{
   public class EmployeModel
    {
        public string EmployeeNo { get; set; }
        public string Name { get; set; }
        public string EMail { get; set; }
        public string BuyerGroupManager { get; set; }
        public string BuyerGroupNo { get; set; }
        public string BGRole { get; set; }
        public string ProjectManager { get; set; }
        public string ProjectMangerNo { get; set; }
        public string PMRole { get; set; }
        public Nullable<byte> DeptID { get; set; }
        public List<PurchaseCreditApproversModel> Approvers { get; set; }
    }
    public class PurchaseCreditApproversModel
    {
        public string ApproverName { get; set; }
        public string AuthorizationType { get; set; }
        public string RoleName { get; set; }
        public string EmployeeNo { get; set; }
        public string RoleId { get; set; }
    }
    public class ProjectManagerModel
    {
        public string EmployeeNo { get; set; }
        public string Name { get; set; }
    }
}
