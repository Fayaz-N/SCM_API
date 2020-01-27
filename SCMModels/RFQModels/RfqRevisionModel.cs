using SCMModels.SCMModels;
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
            RFQTerms = new List<RFQTermsModel>();
            mprIncharges = new List<MPRIncharge>();
            rfqCommunications = new List<RFQCommunication>();

        }
        public int RfqRevisionId { get; set; }
        public int RfqMasterId { get; set; }
        public int RfqRevisionNo { get; set; }
        public string RFQType { get; set; }
        public DateTime QuoteValidFrom { get; set; }
        public DateTime QuoteValidTo { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime RfqValidDate { get; set; }
        public string PackingForwading { get; set; }
        public string ExciseDuty { get; set; }
        public string salesTax { get; set; }
        public string freight { get; set; }
        public string Insurance { get; set; }
        public string CustomsDuty { get; set; }
        public string VendorName { get; set; }
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
        public List<RFQTermsModel> RFQTerms { get; set; }
        public List<MPRIncharge> mprIncharges { get; set; }
        public List<RFQCommunication> rfqCommunications { get; set; }
    }
}
