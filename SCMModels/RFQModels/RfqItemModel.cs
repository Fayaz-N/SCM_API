using SCMModels.SCMModels;
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
            RFQDocuments = new List<RfqDocumentsModel>();
            rfqterms = new List<RFQTermsModel>();
            mappingrfq = new List<RFQMPRMappingModel>();
        }
        public int RFQItemID { get; set; }
        public int RFQRevisionId { get; set; }
        public  Nullable<int> MRPItemsDetailsID { get; set; }
        public string ItemId { get; set; }
        public double QuotationQty { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public string VendorModelNo { get; set; }
        public string HSNCode { get; set; }
        public string VendorName { get; set; }
        public string DocumentyNo { get; set; }
        public decimal TargetSpend { get; set; }
        public string SaleOrderNo { get; set; }
        public decimal UnitPrice { get; set; }
        public string PaymentTermCode { get; set; }
        public string DocumentNo { get; set; }
        public int RFQItemsId { get; set; }
        public string Department { get; set; }
        public Nullable<byte> DepartmentId { get; set; }
        public Nullable<decimal> CustomDuty { get; set; }
        public Nullable<decimal> CustomDutyAmount { get; set; }
        public Nullable<decimal> FreightPercentage { get; set; }
        public Nullable<decimal> FreightAmount { get; set; }
        public Nullable<decimal> PFPercentage { get; set; }
        public Nullable<decimal> PFAmount { get; set; }
        public Nullable<decimal> TotalTaxAmount { get; set; }
        public Nullable<decimal> IGSTPercentage { get; set; }
        public Nullable<decimal> CGSTPercentage { get; set; }
        public string MfgPartNo { get; set; }
        public string MfgModelNo { get; set; }
        public Nullable<decimal> SGSTPercentage { get; set; }
        public Nullable<decimal> ItemUnitPrice { get; set; }
        public Nullable<decimal> IGSTAmount { get; set; }
        public Nullable<decimal> CGSTAmount { get; set; }
        public Nullable<decimal> SGSTAmount { get; set; }
        public Nullable<bool> taxInclusiveOfDiscount { get; set; }
        public Nullable<decimal> DiscountAmount { get; set; }
        public Nullable<decimal> Discountpercentage { get; set; }
        public Nullable<decimal> NetAmount { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
        public Nullable<decimal> FinalNetAmount { get; set; }
        public string RequestRemarks { get; set; }
        public bool IsDeleted { get; set; }
        public int paid { get; set; }
        public int paitemid { get; set; }
        public RfqRevisionModel RFQRevision { get; set; }
        public List<RFQTermsModel> rfqterms { get; set; }
        public List<RfqItemInfoModel> iteminfo { get; set; }
        public List<RfqCommunicationModel> communication { get; set; }
        public List<RfqDocumentsModel> RFQDocuments { get; set; }
        public List<RFQMPRMappingModel> mappingrfq { get; set; }
        public MPRPADetailsModel mprpa { get; set; }

    }
    public class PADetailsModel
    {
        public int VendorId { get; set; }
        public int RequisitionIds { get; set; }
        public int RevisionId { get; set; }
        public string DocumentNumber { get; set; }
        public string RFQNo { get; set; }
        public int BuyerGroupId { get; set; }
        public string SaleOrderNo { get; set; }
        public int DeptID { get; set; }
        public string EmployeeNo { get; set; }
    }
    public class ItemsViewModel
    {
        public string POItemNo { get; set; }
        public DateTime PODate { get; set; }
        public string PONO { get; set; }
        public string Remarks { get; set; }
        public int paid { get; set; }
        public int paitemid { get; set; }
    }
}
