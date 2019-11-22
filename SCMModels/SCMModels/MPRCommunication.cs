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
    
    public partial class MPRCommunication
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MPRCommunication()
        {
            this.MPRReminderTrackings = new HashSet<MPRReminderTracking>();
        }
    
        public int MPRCCId { get; set; }
        public int RevisionId { get; set; }
        public string Remarks { get; set; }
        public string RemarksFrom { get; set; }
        public bool SendEmail { get; set; }
        public bool SetReminder { get; set; }
        public Nullable<System.DateTime> ReminderDate { get; set; }
        public System.DateTime RemarksDate { get; set; }
        public bool DeleteFlag { get; set; }
    
        public virtual MPRCommunication MPRCommunications1 { get; set; }
        public virtual MPRCommunication MPRCommunication1 { get; set; }
        public virtual MPRRevision MPRRevision { get; set; }
        public virtual UserDetail UserDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MPRReminderTracking> MPRReminderTrackings { get; set; }
    }
}
