using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMModels.RFQModels
{
   public class EmployeemappingtocreditModel
    {
        public int CRApprovalId { get; set; }
        public int Authid { get; set; }
        public decimal MinPAValue { get; set; }
        public decimal MaxPAValue { get; set; }
        public int DeptId { get; set; }
        public string EmployeeNo { get; set; }
        public string Name { get; set; }
        public string AuthorizationType { get; set; }
        public byte CreditdaysId { get; set; }
        public byte MinDays { get; set; }
        public byte MaxDays { get; set; }
    }
}
