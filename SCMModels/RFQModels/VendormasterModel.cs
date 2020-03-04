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
        public string VuniqueId { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string Emailid { get; set; }
        public string ContactNumber { get; set; }
        public string ContactPerson { get; set; }
        public string OldVendorCode { get; set; }
        public string Street { get; set; }
        public string FaxNo { get; set; }
        public string AuGr { get; set; }
        public string RegionCode { get; set; }
        public string PaymentTermCode { get; set; }
        public string Blocked { get; set; }
        public string PhoneNo { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public Nullable<bool> Deleteflag { get; set; }
        public bool AutoAssignmentofRFQ { get; set; }
        public IList<RFQMasterModel> masters { get; set; }
    }
}
