using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMModels.RFQModels
{
   public class RFQStatusModel
    {
        public int RfqStatusId { get; set; }
        public int RfqRevisionId { get; set; }
        public Nullable<int> StatusId { get; set; }
        public string Remarks { get; set; }
        public string updatedby { get; set; }
        public Nullable<System.DateTime> updatedDate { get; set; }
        public bool DeleteFlag { get; set; }

        public virtual RfqRevisionModel RFQRevision { get; set; }
    }

}
