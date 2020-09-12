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
        public decimal TargetSpend { get; set; }
        public decimal  UnitPrice { get; set; }
        public Nullable<bool> LessBudget { get; set; }
        public Nullable<bool> MoreBudget { get; set; }
        public string PaymentTermCode { get; set; }
        public List<int> MPRItemDetailsid { get; set; }
        public string MPRItemDetailsid1 { get; set; }
        public int BuyerGroupId { get; set; }
    }
}
