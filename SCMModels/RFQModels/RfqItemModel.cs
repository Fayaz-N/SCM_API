using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMModels.RFQModels
{
    public class RfqItemModel
    {
        public RfqItemModel()
        {
            communication = new List<RfqCommunicationModel>();
            iteminfo = new List<RfqItemInfoModel>();
            documents = new List<RfqDocumentsModel>();
        }
        public int RFQItemID { get; set; }
        public int RFQRevisionId { get; set; }
        public int MRPItemsDetailsID { get; set; }
        public double QuotationQty { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public string VendorModelNo { get; set; }
        public string HSNCode { get; set; }
        public Nullable<decimal> CustomDuty { get; set; }
        public Nullable<decimal> FreightPercentage { get; set; }
        public Nullable<decimal> FreightAmount { get; set; }
        public Nullable<decimal> PFPercentage { get; set; }
        public Nullable<decimal> PFAmount { get; set; }
        public Nullable<decimal> IGSTPercentage { get; set; }
        public Nullable<decimal> CGSTPercentage { get; set; }
        public Nullable<decimal> SGSTPercentage { get; set; }
        public Nullable<bool> taxInclusiveOfDiscount { get; set; }
        public string RequsetRemarks { get; set; }
        public bool IsDeleted { get; set; }
        public RfqRevisionModel RFQRevision { get; set; }
        public List<RfqItemInfoModel> iteminfo { get; set; }
        public List<RfqCommunicationModel> communication { get; set; }
        public List<RfqDocumentsModel> documents { get; set; }
    }
}
