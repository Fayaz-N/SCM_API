using SCMModels.SCMModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMModels
{
	public class DynamicSearchResult
	{
		public string connectionString { get; set; }
		public string columnNames { get; set; }
		public string columnValues { get; set; }
		public string tableName { get; set; }
		public string updateCondition { get; set; }
		public string searchCondition { get; set; }
		public string sortBy { get; set; }
		public string query { get; set; }
	}
	public class searchParams
	{
		public string tableName { get; set; }
		public string fieldName { get; set; }
		public string fieldId { get; set; }
	}
	public class MPRStatusUpdate
	{
		public int RevisionId { get; set; }
		public int StatusId { get; set; }
		public string status { get; set; }
		public string typeOfuser { get; set; }
		public string Remarks { get; set; }
		public int RequisitionId { get; set; }
		public string PreparedBy { get; set; }

		public Nullable<byte> BuyerGroupId { get; set; }
		public List<MPR_Assignment> MPRAssignments { get; set; }
	}
	public class mprFilterParams
	{
		public string ListType { get; set; }
		public string DocumentNo { get; set; }
		public string DocumentDescription { get; set; }
		public string FromDate { get; set; }
		public string ToDate { get; set; }
		public string Status { get; set; }
		public string PreparedBy { get; set; }
		public string CheckedBy { get; set; }
		public string ApprovedBy { get; set; }
		public string SecOrThirdApprover { get; set; }

		public string IssuePurposeId { get; set; }
		public string JobCode { get; set; }
		public string ItemDescription { get; set; }
		public string DepartmentId { get; set; }
		public string ORgDepartmentid { get; set; }
		public string GEPSApprovalId { get; set; }
		public string BuyerGroupId { get; set; }
		public string AssignEmployee { get; set; }
		public string MPRStatusId { get; set; }
		public string PurchaseTypeId { get; set; }
		public List<string> mprStatusListId { get; set; }
		public string PONO { get; set; }
		public string PAID { get; set; }
		public string typeOfUser { get; set; }
	}
	public class rfqFilterParams
	{
		public string RFQType { get; set; }
		public string typeOfFilter { get; set; }
		public string FromDate { get; set; }
		public string ToDate { get; set; }
		public string RFQNo { get; set; }
		public string venderid { get; set; }
		public string DocumentNo { get; set; }
	}
	public class DataModel
	{
		public List<RFQQuoteView> RFQQuoteViewList { get; set; }
		public List<YILTermsandCondition> TermsList { get; set; }
		public List<MPRRFQDocument> mprfqDocs { get; set; }
	}
	public class EmployeeModel
	{
		public string EmployeeNo { get; set; }
		public string Name { get; set; }
		public string EMail { get; set; }
		public Nullable<short> OrgDepartmentId { get; set; }
		public string OrgDepartmentName { get; set; }
		public Nullable<System.DateTime> DOL { get; set; }
		public Nullable<int> RoleId { get; set; }
		public string accessToken { get; set; }
	}

	public class sendMailObj
	{
		public string Message { get; set; }
		public Boolean IncludeUrl { get; set; }
		public Boolean IncludeCredentials { get; set; }
	}

	public class DeleteMpr
	{
		public string DeletedRemarks { get; set; }
		public string Deletedby { get; set; }
		public int RevisionId { get; set; }
	}

	public class MPRRFQDocument
	{
		public int MprDocId { get; set; }
		public int RevisionId { get; set; }
		public Nullable<int> ItemDetailsId { get; set; }
		public string DocumentName { get; set; }
		public string UploadedBy { get; set; }
		public System.DateTime UplaodedDate { get; set; }
		public bool CanShareWithVendor { get; set; }
		public string Path { get; set; }
		public byte DocumentTypeid { get; set; }
		public int VendorId { get; set; }
		public bool Deleteflag { get; set; }


	}
	public class tokuchuFilterParams
	{
		public string FromDate { get; set; }
		public string ToDate { get; set; }
		public string Paid { get; set; }
		public string PreparedBY { get; set; }
		public string VerifiedBy { get; set; }

	}
	public class materialUpdate
	{
		public int Itemdetailsid { get; set; }
		public Nullable<int> RevisionId { get; set; }
		public string Itemid { get; set; }
		public int RFQItemsId { get; set; }
	}

	public class vendorRegfilters
	{
		public string FromDate { get; set; }
		public string ToDate { get; set; }
		public string VendorName { get; set; }
		public string IntiatedBy { get; set; }
		public string CheckedBy { get; set; }
		public string ApprovedBy { get; set; }
		public string VerifiedBy { get; set; }
		public string SecondApprover { get; set; }
		public string ThirdApprover { get; set; }
		public string CheckerStatus { get; set; }
		public string ApprovalStatus { get; set; }
		public string VerifiedStatus { get; set; }
	}

	public partial class VendorRegApprovalProcessData
	{
		public int ProceesId { get; set; }
		public string VendorName { get; set; }
		public Nullable<int> Vendorid { get; set; }
		public string VendorEmailId { get; set; }
		public string IntiatedBy { get; set; }
		public Nullable<System.DateTime> IntiatedOn { get; set; }
		public string CheckedBy { get; set; }
		public Nullable<System.DateTime> CheckedOn { get; set; }
		public string CheckerStatus { get; set; }
		public string CheckerRemarks { get; set; }
		public string ApprovedBy { get; set; }
		public Nullable<System.DateTime> ApprovedOn { get; set; }
		public string ApprovalStatus { get; set; }
		public string ApproverRemarks { get; set; }
		public string Verifier1 { get; set; }
		public string Verifier2 { get; set; }
		public string VerifiedBy { get; set; }
		public Nullable<System.DateTime> VerifiedOn { get; set; }
		public string VerifiedStatus { get; set; }
		public string VerifierRemarks { get; set; }
		public string VendorNoInSAP { get; set; }
		public string Street { get; set; }
		public string City { get; set; }
		public Nullable<int> PaymentTermId { get; set; }
		public string PaymentTerms { get; set; }
		
		public Nullable<bool> Onetimevendor { get; set; }
		public Nullable<bool> EvaluationRequired { get; set; }
		public Nullable<bool> PerformanceVerificationRequired { get; set; }
		public Nullable<bool> VendorType { get; set; }
		public Nullable<bool> IsExistVendor { get; set; }
		public string ChangesFor { get; set; }

	}

	public partial class VendorRegistrationModel
	{
		public Nullable<int> VendorId { get; set; }
		public string VendorCode { get; set; }
		public int UniqueId { get; set; }
		public Boolean Onetimevendor { get; set; }
		public Boolean EvaluationRequired { get; set; }
		public bool PerformanceVerificationRequired { get; set; }
		public bool MSMERequired { get; set; }

		public string VendorNoInSAP { get; set; }
		public DateTime? RequestedOn { get; set; }
		public string VendorName { get; set; }
		public string VendorAddress { get; set; }
		public string PostalCode { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string LocalBranchOffice { get; set; }
		public string PhoneAndExtn { get; set; }
		public string Fax { get; set; }
		public string ContactPerson { get; set; }
		public string Phone { get; set; }
		public string ContactPersonForSales { get; set; }
		public string PhoneNumberForSales { get; set; }
		public string EmailIdForSales { get; set; }
		public string AltEmailidForSales { get; set; }
		public string ContactPersonForOperations { get; set; }
		public string PhoneNumberForOperations { get; set; }
		public string EmailIdForOperations { get; set; }
		public string AltEmailidForOperations { get; set; }
		public string ContactPersonForLogistics { get; set; }
		public string PhoneNumberForLogistics { get; set; }
		public string EmailIdForLogistics { get; set; }
		public string AltEmailidForLogistics { get; set; }
		public string ContactPersonForAccounts { get; set; }
		public string PhoneNumberForAccounts { get; set; }
		public string EmailIdForAccounts { get; set; }
		public string AltEmailidForAccounts { get; set; }
		public string GSTNo { get; set; }
		public int? NatureofBusiness { get; set; }
		public string SpecifyNatureOfBusiness { get; set; }
		public string PANNo { get; set; }
		public string CINNo { get; set; }
		public string TanNo { get; set; }
		public string PaymentTerms { get; set; }
		public Nullable<int> PaymentTermId { get; set; }
		public string Street { get; set; }

		public string Location { get; set; }
		public string BusinessArea { get; set; }
		public string Mobile { get; set; }
		public string Email { get; set; }
		public string AltEmail { get; set; }
		public string contPhone { get; set; }
		public string BankDetails { get; set; }
		public string BankerName { get; set; }
		public string LocationOrBranch { get; set; }
		public string AccNo { get; set; }
		public string IFSCCode { get; set; }
		public string IncoTerms { get; set; }
		public string AccountHolderName { get; set; }
		public string PhysicalPath { get; set; }
		public int DocumentationTypeId { get; set; }
		public int StateId { get; set; }
		public string fileattach1 { get; set; }
		public string SwiftCode { get; set; }
		public Nullable<int> CurrencyId { get; set; }
		public string CurrencyName { get; set; }
		public Nullable<bool> VendorType { get; set; }
		public string Country { get; set; }
		public List<VendorRegisterDocumentDetail> DocDetailsLists = new List<VendorRegisterDocumentDetail>();


	}
}