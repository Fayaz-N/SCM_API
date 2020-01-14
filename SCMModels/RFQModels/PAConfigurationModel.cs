using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMModels.RFQModels
{
   public class PAConfigurationModel
    {
        public decimal PAValue { get; set; }
        public int Creditdays { get; set; } 
        public bool Budgetvalue { get; set; }
        public int DeptId { get; set; }
        public int TargetSpend { get; set; }
        public int  UnitPrice { get; set; }
        public bool LessBudget { get; set; }
        public bool MoreBudget { get; set; }
        public string PaymentTermCode { get; set; }
        public List<int> MPRItemDetailsid { get; set; }
    }
}
