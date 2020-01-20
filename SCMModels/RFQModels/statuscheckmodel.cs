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
    }
    public class PAApproverDetailsInputModel
    {
        public int Paid { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
    }
}
