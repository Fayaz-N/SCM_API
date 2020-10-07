using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMModels.RFQModels
{
   public class CurrencyMasterModel
    {
        public CurrencyMasterModel()
        {
            currencyHistory = new List<CurrencyHistoryModel>();
        }
        public byte CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencyCode { get; set; }
        public string UpdatedBy { get; set; }
        public System.DateTime updateddate { get; set; }
        public string DeletedBy { get; set; }
        public System.DateTime DeletedDate { get; set; }
        public bool DeleteFlag { get; set; }
        public List<CurrencyHistoryModel> currencyHistory { get; set; }
    }
    public class CurrencyHistoryModel
    {
        public int CurrencyHistoryId { get; set; }
        public byte CurrencyId { get; set; }
        public decimal CurrencyValue { get; set; }
        public System.DateTime EffectiveFrom { get; set; }
        public System.DateTime EffectiveTo { get; set; }
        public string UpdatedBy { get; set; }
        public System.DateTime UpdatedDate { get; set; }
        public bool IsActive { get; set; }
        public string editedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
        public CurrencyMasterModel currencymaster { get; set; }
    }
}
