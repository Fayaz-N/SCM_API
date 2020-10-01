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
    public class ReportInputModel
    {
        public int BuyerGroupId { get; set; }
        public string Fromdate { get; set; }
        public string Todate { get; set; }
        public string BuyerGroup { get; set; }
        public int RequisitionId { get; set; }
        public int revisionId { get; set; }
        public string jobcode { get; set; }
        public int Issuepurposeid { get; set; }
        public string preparedby { get; set; }
        public string Checked { get; set; }
        public string ApprovedBy { get; set; }
        public string checkerstatus { get; set; }
        public string finalApproverstatus { get; set; }
        public string DocumentNo { get; set; }
        public int DepartmentId { get; set; }
        public string Department { get; set; }
        public int totalcount { get; set; }
        public string status { get; set; }
        public bool ShowAllrevisions { get; set; }
        public string ProjectManager { get; set; }
    }

    public class ReportFilterModel
    {
        public ReportFilterModel()
        {
            mprprepares = new List<Mprprepare>();
            mprcheckedby = new List<MprCheckers>();
            mprApprovedby = new List<MprApprovers>();
            purposetype = new List<IssuepurposeType>();
            jobcode = new List<jobcodes>();
        }
        public List<jobcodes> jobcode { get; set; }
        public DateTime Fromdate { get; set; }
        public DateTime Todate { get; set; }
        public List<Mprprepare> mprprepares { get; set; }
        public List<MprCheckers> mprcheckedby { get; set; }
        public List<MprApprovers> mprApprovedby { get; set; }
        public List<IssuepurposeType> purposetype { get; set; }
    }
    public class IssuepurposeType
    {
        public string Issuepurpose { get; set; }
        public int Issuepurposeid { get; set; }
    }
    public class Mprprepare
    {
        public string Preparedby { get; set; }
        public string preparedname { get; set; }
    }
    public class MprCheckers
    {
        public string checker { get; set; }
        public string checkername { get; set; }
    }
    public class MprApprovers
    {
        public string approvedby { get; set; }
        public string approvername { get; set; }
    }
    public class jobcodes
    {
        public string Jobcode { get; set; }
        public int ID { get; set; }
    }
}
