using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMModels.RFQModels
{
   public class VendormasterModel
    {
        public int Vendorid { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string Emailid { get; set; }
        public string ContactNo { get; set; }
        public bool IsDeleted { get; set; }
        public IList<RFQMasterModel> masters { get; set; }
    }
}
