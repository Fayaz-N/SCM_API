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
    
    public partial class MPRBuyerGroup
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MPRBuyerGroup()
        {
            this.MPRBuyerGroupMembers = new HashSet<MPRBuyerGroupMember>();
            this.MPRRevisions = new HashSet<MPRRevision>();
            this.Test_CategoryMaster = new HashSet<Test_CategoryMaster>();
        }
    
        public byte BuyerGroupId { get; set; }
        public string BuyerGroup { get; set; }
        public bool BoolInUse { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MPRBuyerGroupMember> MPRBuyerGroupMembers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MPRRevision> MPRRevisions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Test_CategoryMaster> Test_CategoryMaster { get; set; }
    }
}
