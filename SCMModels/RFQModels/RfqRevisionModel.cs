using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMModels.RFQModels
{
   public class RfqRevisionModel
    {
        public RfqRevisionModel()
        {
            rfqitem = new List<RfqItemModel>();
            RFQStatus = new List<RFQStatusModel>();
            rfqvendor = new List<RfqVendorTermModel>();
        }
        public int RfqRevisionId { get; set; }
        public int RfqMasterId { get; set; }
        public int RfqRevisionNo { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime RfqValidDate { get; set; }
        public string PackingForwading { get; set; }
        public string ExciseDuty { get; set; }
        public string salesTax { get; set; }
        public string freight { get; set; }
        public string Insurance { get; set; }
        public string CustomsDuty { get; set; }
        public Nullable<int> ShipmentModeId { get; set; }
        public Nullable<int> PaymentTermDays { get; set; }
        public string PaymentTermRemarks { get; set; }
        public string BankGuarantee { get; set; }
        public Nullable<int> DeliveryMinWeeks { get; set; }
        public Nullable<int> DeliveryMaxWeeks { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public RFQMasterModel rfqmaster { get; set; }
        public List<RfqItemModel> rfqitem { get; set; }
        public List<RFQStatusModel> RFQStatus { get; set; }
        public List<RfqVendorTermModel> rfqvendor { get; set; }
    }
}
