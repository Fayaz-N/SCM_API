using SCMModels.RFQModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMModels.MPRMasterModels
{
   public class MPRRevisionModel
    {
        public int RevisionId { get; set; }
        public int RequisitionId { get; set; }
        public Nullable<byte> DepartmentId { get; set; }
        public string ProjectManager { get; set; }
        public string JobCode { get; set; }
        public string JobName { get; set; }
        public string GEPSApprovalId { get; set; }
        public string SaleOrderNo { get; set; }
        public string LineItemNo { get; set; }
        public string ClientName { get; set; }
        public string PlantLocation { get; set; }
        public Nullable<byte> BuyerGroupId { get; set; }
        public Nullable<decimal> TargetedSpendAmount { get; set; }
        public string TargetedSpendRemarks { get; set; }
        public Nullable<bool> BoolPreferredVendor { get; set; }
        public string JustificationForSinglePreferredVendor { get; set; }
        public Nullable<System.DateTime> DeliveryRequiredBy { get; set; }
        public Nullable<byte> IssuePurposeId { get; set; }
        public string DispatchLocation { get; set; }
        public Nullable<byte> ScopeId { get; set; }
        public Nullable<bool> TrainingRequired { get; set; }
        public Nullable<byte> TrainingManWeeks { get; set; }
        public string TrainingRemarks { get; set; }
        public Nullable<bool> BoolDocumentationApplicable { get; set; }
        public string GuaranteePeriod { get; set; }
        public Nullable<byte> NoOfSetsOfQAP { get; set; }
        public Nullable<bool> InspectionRequired { get; set; }
        public Nullable<byte> InspectionRequiredNew { get; set; }
        public string InspectionComments { get; set; }
        public Nullable<byte> NoOfSetsOfTestCertificates { get; set; }
        public Nullable<byte> ProcurementSourceId { get; set; }
        public Nullable<byte> CustomsDutyId { get; set; }
        public Nullable<byte> ProjectDutyApplicableId { get; set; }
        public Nullable<bool> ExciseDutyReimbursableForBOs { get; set; }
        public Nullable<bool> SalesTaxReimbursableForBOs { get; set; }
        public Nullable<bool> VATReimbursableForBOs { get; set; }
        public Nullable<bool> ServiceTaxReimbursableForBOs { get; set; }
        public Nullable<byte> ExciseDutyReimbursableForBOsNew { get; set; }
        public Nullable<byte> SalesTaxReimbursableForBOsNew { get; set; }
        public Nullable<byte> VATReimbursableForBOsNew { get; set; }
        public Nullable<byte> ServiceTaxReimbursableForBOsNew { get; set; }
        public string Remarks { get; set; }
        public string PreparedBy { get; set; }
        public Nullable<System.DateTime> PreparedOn { get; set; }
        public string CheckedBy { get; set; }
        public Nullable<System.DateTime> CheckedOn { get; set; }
        public string CheckStatus { get; set; }
        public string CheckerRemarks { get; set; }
        public string ApprovedBy { get; set; }
        public Nullable<System.DateTime> ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string ApproverRemarks { get; set; }
        public string SecondApprover { get; set; }
        public Nullable<System.DateTime> SecondApprovedOn { get; set; }
        public string SecondApproversStatus { get; set; }
        public string SecondApproverRemarks { get; set; }
        public string ThirdApprover { get; set; }
        public string ThirdApproverStatus { get; set; }
        public Nullable<System.DateTime> ThirdApproverStatusChangedOn { get; set; }
        public string ThirdApproverRemarks { get; set; }
        public string PurchaseDetailsReadBy { get; set; }
        public Nullable<System.DateTime> PurchaseDetailsReadOn { get; set; }
        public string PurchasePersonnel { get; set; }
        public Nullable<System.DateTime> PODate { get; set; }
        public Nullable<System.DateTime> ExpectedDespatchDate { get; set; }
        public string PurchasePersonnelsComments { get; set; }
        public Nullable<System.DateTime> TechDocsReceivedDate { get; set; }
        public Nullable<System.DateTime> CommercialOfferReceivedDate { get; set; }
        public string OfferDetailsMailedBy { get; set; }
        public Nullable<System.DateTime> OfferDetailsMailedOn { get; set; }
        public Nullable<System.DateTime> OfferDetailsViewedByCheckerOn { get; set; }
        public Nullable<System.DateTime> OfferDetailsViewedByApproverOn { get; set; }
        public Nullable<System.DateTime> MaterialReceiptDate { get; set; }
        public string Remarks1 { get; set; }
        public Nullable<decimal> EstimatedCost { get; set; }
        public Nullable<decimal> PreviousPOPrice { get; set; }
        public Nullable<byte> PurchaseTypeId { get; set; }
        public Nullable<byte> PreferredVendorTypeId { get; set; }
        public Nullable<byte> StatusId { get; set; }
        public Nullable<decimal> PurchaseCost { get; set; }
        public Nullable<byte> RevisionNo { get; set; }
        public bool BoolValidRevision { get; set; }

        public virtual MPRBuyerGroupModel MPRBuyerGroup { get; set; }
       
        //public virtual ICollection<MPRCommunication> MPRCommunications { get; set; }
        public virtual MPRCustomsDutyModel MPRCustomsDuty { get; set; }
        public virtual MPRDepartmentModel MPRDepartment { get; set; }
        //public virtual MPRDetail MPRDetail { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<MPRDocumentation> MPRDocumentations { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<MPRDocument> MPRDocuments { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<MPRIncharge> MPRIncharges { get; set; }
        //public virtual MPRIssuePurpos MPRIssuePurpos { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<MPRItemDetails_X> MPRItemDetails_X { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<MPRItemInfo> MPRItemInfoes { get; set; }
        //public virtual MPRProcurementSource MPRProcurementSource { get; set; }
        //public virtual MPRProjectDutyApplicable MPRProjectDutyApplicable { get; set; }
        //public virtual StandardListYNN StandardListYNN { get; set; }
        //public virtual StandardListYNN StandardListYNN1 { get; set; }
        //public virtual StandardListYNN StandardListYNN2 { get; set; }
        //public virtual StandardListYNN StandardListYNN3 { get; set; }
        //public virtual StandardListYNN StandardListYNN4 { get; set; }
        //public virtual StandardListYNN StandardListYNN5 { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<MPRStatusTrack> MPRStatusTracks { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<MPRTargetedSpendSupportingDoc> MPRTargetedSpendSupportingDocs { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<MPRVendorDetail> MPRVendorDetails { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RFQMasterModel> RFQMasters { get; set; }
    }
}
