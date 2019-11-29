using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMModels.RFQModels
{
   public class RFQMasterModel
    {
        public RFQMasterModel()
        {
            this.Revision = new List<RfqRevisionModel>();
        }
        public int RfqMasterId { get; set; }
        public string RfqNo { get; set; }
        public Nullable<int> RfqUniqueNo { get; set; }
        public int MPRRevisionId { get; set; }
        public int VendorId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public VendormasterModel Vendor { get; set; }
        public List<RfqRevisionModel> Revision { get; set; }
    }
    public class RFQMasterDataModel
    {
        public RFQMasterDataModel()
        {
            RFQlist = new List<RFQMasterModel>();
        }
        public List<RFQMasterModel> RFQlist { get; set; }
    }
}
