using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMModels.RFQModels
{
   public class statuscheckmodel
    {
        public int Sid { get; set; }
        public string StatusMesssage { get; set; }
        public string StatusRole { get; set; }
    }
    public class PAApproverDetailsInputModel
    {
        public int Paid { get; set; }
        public Nullable<DateTime> FromDate { get; set; }
        public Nullable<DateTime> ToDate { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public string DocumentNumber { get; set; }
        public int DepartmentId { get; set; }
        public int BuyerGroupId { get; set; }
        public int VendorId { get; set; }
        public string rfqnumber { get; set; }
    }
}
