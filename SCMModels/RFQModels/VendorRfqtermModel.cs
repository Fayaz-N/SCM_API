using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMModels.RFQModels
{
   public class VendorRfqtermModel
    {
        public VendorRfqtermModel()
        {
            RFQVendorTerms = new List<RfqVendorTermModel>();
        }
        public int VendorTermsid { get; set; }
        public int TermsCategoryId { get; set; }
        public int VendorID { get; set; }
        public string Terms { get; set; }
        public Nullable<int> Indexno { get; set; }
        public bool deleteFlag { get; set; }
        public List<RfqVendorTermModel> RFQVendorTerms { get; set; }
        //public virtual TermsCategory TermsCategory { get; set; }
        public VendormasterModel VendorMaster { get; set; }
    }
    public class RfqVendorTermModel
    {
        public int RFQTermsid { get; set; }
        public int RFQversionId { get; set; }
        public int VendorTermsid { get; set; }
        public string updatedBY { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public bool DeleteFlag { get; set; }
        public virtual RfqRevisionModel RFQRevision { get; set; }
        public virtual VendorRfqtermModel VendorRFQTerm { get; set; }
    }
}
