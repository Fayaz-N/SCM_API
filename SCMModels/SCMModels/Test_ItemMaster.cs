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
    
    public partial class Test_ItemMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Test_ItemMaster()
        {
            this.ItemAttributesMasters = new HashSet<ItemAttributesMaster>();
            this.Test_ItemAttributesMaster = new HashSet<Test_ItemAttributesMaster>();
        }
    
        public int itemId { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public string ItemName { get; set; }
        public string HSNCode { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ItemAttributesMaster> ItemAttributesMasters { get; set; }
        public virtual Test_CategoryMaster Test_CategoryMaster { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Test_ItemAttributesMaster> Test_ItemAttributesMaster { get; set; }
    }
}