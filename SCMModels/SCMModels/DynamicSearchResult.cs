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
        public string GEPSApprovalId { get; set; }
        public string BuyerGroupId { get; set; }
        public string AssignEmployee { get; set; }
        public string MPRStatusId { get; set; }

    }
    public class rfqFilterParams
    {
        public string typeOfFilter { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string RFQNo { get; set; }
        public string venderid { get; set; }
        public string DocumentNo { get; set; }
    }
    public class DataModel
    {
        public List<RFQQuoteView> RFQQuoteViewList { get; set; }
        public List<YILTermsandCondition> TermsList { get; set; }
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

}