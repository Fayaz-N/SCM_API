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
    
    public partial class Test_AttributeMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Test_AttributeMaster()
        {
            this.ItemAttributesMasters = new HashSet<ItemAttributesMaster>();
            this.Test_CategoryAttributes = new HashSet<Test_CategoryAttributes>();
            this.Test_ItemAttributesMaster = new HashSet<Test_ItemAttributesMaster>();
        }
    
        public int AttributeId { get; set; }
        public string AttributeName { get; set; }
        public string Description { get; set; }
        public Nullable<bool> DeleteFlag { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ItemAttributesMaster> ItemAttributesMasters { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Test_CategoryAttributes> Test_CategoryAttributes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Test_ItemAttributesMaster> Test_ItemAttributesMaster { get; set; }
    }
}
