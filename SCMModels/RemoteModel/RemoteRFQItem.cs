//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SCMModels.RemoteModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class RemoteRFQItem
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RemoteRFQItem()
        {
            this.RemoteRFQCommunications = new HashSet<RemoteRFQCommunication>();
            this.RemoteRFQDocuments = new HashSet<RemoteRFQDocument>();
            this.RemoteRFQItemsInfoes = new HashSet<RemoteRFQItemsInfo>();
        }
    
        public int RFQItemsId { get; set; }
        public int RFQRevisionId { get; set; }
        public int MPRItemDetailsid { get; set; }
        public double QuotationQty { get; set; }
        public string VendorModelNo { get; set; }
        public string HSNCode { get; set; }
        public string RequestRemarks { get; set; }
        public bool DeleteFlag { get; set; }
        public Nullable<System.DateTime> SyncDate { get; set; }
        public Nullable<bool> SyncStatus { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RemoteRFQCommunication> RemoteRFQCommunications { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RemoteRFQDocument> RemoteRFQDocuments { get; set; }
        public virtual RemoteRFQRevision RemoteRFQRevision { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RemoteRFQItemsInfo> RemoteRFQItemsInfoes { get; set; }
    }
}
