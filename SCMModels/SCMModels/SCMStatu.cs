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
    
    public partial class SCMStatu
    {
        public byte Statusid { get; set; }
        public string StatusName { get; set; }
        public string StatusType { get; set; }
        public Nullable<bool> RemarksRequired { get; set; }
        public Nullable<byte> Sequence { get; set; }
        public Nullable<bool> deleteFlag { get; set; }
        public string DeletedBy { get; set; }
        public Nullable<System.DateTime> Deletedon { get; set; }
    }
}
