using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCMModels.MPRMasterModels;

namespace SCMModels.RFQModels
{
   public class MPRPADetailsModel
    {
        public MPRPADetailsModel()
        {
            purchasemodes = new MPRPAPurchaseModesModel();
            purchasetypes = new MPRPAPurchaseTypesModel();
            department = new MPRDepartmentModel();
            buyergroup = new MPRBuyerGroupModel();
            Item = new List<RfqItemModel>();
            ApproversList = new List<MPRPAApproversModel>();
            RfqTerms = new List<RFQTermsModel>();
            ItemInfo = new List<RfqItemInfoModel>();
            TermId = new List<int>();
            documents = new List<PADocumentsmodel>();
        }
        public int PAId { get; set; }
        public string RequestedBy { get; set; }
        public Nullable<System.DateTime> RequestedOn { get; set; }
        public Nullable<byte> DepartmentID { get; set; }
        public Nullable<byte> BuyerGroupId { get; set; }
        public Nullable<byte> PurchaseTypeId { get; set; }
        public Nullable<byte> PurchaseModeId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectCode { get; set; }
        public Nullable<decimal> TargetedSpendAmount { get; set; }
        public Nullable<decimal> PurchaseCost { get; set; }
        public string PackagingForwarding { get; set; }
        public string Taxes { get; set; }
        public string Freight { get; set; }
        public string Insurance { get; set; }
        public string PAStatus { get; set; }
        public string DeliveryCondition { get; set; }
        public string ShipmentMode { get; set; }
        public string PaymentTerms { get; set; }
        public Nullable<short> CreditDays { get; set; }
        public string Warranty { get; set; }
        public string BankGuarantee { get; set; }
        public string LDPenaltyTerms { get; set; }
        public string SpecialInstructions { get; set; }
        public string FactorsForImports { get; set; }
        public string SpecialRemarks { get; set; }
        public string SuppliersReference { get; set; }
        public int VendorId { get; set; }
        public List<int> TermId { get; set; }
        public string BuyerGroupManager { get; set; }
        public string BuyerGroupNo { get; set; }
        public string BGRole { get; set; }
        public string PMRole { get; set; }
        public string ProjectManager { get; set; }
        public string ProjectMangerNo { get; set; }
        public string LoginEmployee { get; set; }
        public MPRPAPurchaseModesModel purchasemodes { get; set; }
        public MPRPAPurchaseTypesModel purchasetypes { get; set; }
        public MPRBuyerGroupModel buyergroup { get; set; }
        public MPRDepartmentModel department { get; set; }
        public List<PADocumentsmodel> documents { get; set; }
        public List<RfqItemModel> Item { get; set; }
        public List<MPRPAApproversModel> ApproversList { get; set; }
        public List<RFQTermsModel> RfqTerms { get; set; }
        public List<RfqItemInfoModel> ItemInfo { get; set; }
    }
    public class PADocumentsmodel
    {
        public string filename { get; set; }
        public string path { get; set; }
        public Nullable<System.DateTime> uploadeddate { get; set; }
    }
    public class padeletemodel
    {
        public int PAId { get; set; }
        public string employeeno { get; set; }
    }
    public class painputmodel
    {
        public int PAId { get; set; }
        public string mprno { get; set; }
        public string employeeno { get; set; }
        public DateTime padate { get; set; }
    }
}
