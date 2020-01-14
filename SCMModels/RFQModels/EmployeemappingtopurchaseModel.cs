using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMModels.RFQModels
{
   public class EmployeemappingtopurchaseModel
    {
        public int Authid { get; set; }
        public string FunctionalRoleId { get; set; }
        public string Employeeid { get; set; }
        public bool LessBudget { get; set; }
        public bool MoreBudget { get; set; }
        public string Name { get; set; }
        public decimal MaxPAValue { get; set; }
        public decimal MinPAValue { get; set; }
        public string AuthorizationType { get; set; }
        public int DeptId { get; set; }
        public int PAmapid { get; set; }
    }
}
