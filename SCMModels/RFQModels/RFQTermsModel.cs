using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMModels.RFQModels
{
   public class RFQTermsModel
    {
        public int RfqTermsid { get; set; }
        public int RFQrevisionId { get; set; }
        public int termsid { get; set; }
        public string VendorResponse { get; set; }
        public string Remarks { get; set; }
        public string Terms { get; set; }
        public string TermGroup { get; set; }
        public bool SyncStatus { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<bool> DeleteFlag { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
    }
}
