//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SCMModels.SCMModels
{
    using System;
    using System.Collections.Generic;
    
    public partial class VendorMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VendorMaster()
        {
            this.MPRVendorDetails = new HashSet<MPRVendorDetail>();
            this.RFQMasters = new HashSet<RFQMaster>();
            this.VendorRFQTerms = new HashSet<VendorRFQTerm>();
        }
    
        public int Vendorid { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string Emailid { get; set; }
        public string ContactNo { get; set; }
        public Nullable<bool> Deleteflag { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MPRVendorDetail> MPRVendorDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RFQMaster> RFQMasters { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VendorRFQTerm> VendorRFQTerms { get; set; }
    }
}