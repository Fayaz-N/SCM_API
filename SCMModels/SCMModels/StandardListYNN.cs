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
    
    public partial class StandardListYNN
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public StandardListYNN()
        {
            this.MPRRevisions = new HashSet<MPRRevision>();
            this.MPRRevisions1 = new HashSet<MPRRevision>();
            this.MPRRevisions2 = new HashSet<MPRRevision>();
            this.MPRRevisions3 = new HashSet<MPRRevision>();
            this.MPRRevisions4 = new HashSet<MPRRevision>();
            this.MPRRevisions5 = new HashSet<MPRRevision>();
        }
    
        public byte ListItemId { get; set; }
        public string ListItem { get; set; }
        public byte XOrder { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MPRRevision> MPRRevisions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MPRRevision> MPRRevisions1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MPRRevision> MPRRevisions2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MPRRevision> MPRRevisions3 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MPRRevision> MPRRevisions4 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MPRRevision> MPRRevisions5 { get; set; }
    }
}
