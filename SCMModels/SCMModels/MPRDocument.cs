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
    
    public partial class MPRDocument
    {
        public int MprDocId { get; set; }
        public int RevisionId { get; set; }
        public Nullable<int> ItemDetailsId { get; set; }
        public string DocumentName { get; set; }
        public string UploadedBy { get; set; }
        public System.DateTime UplaodedDate { get; set; }
        public bool CanShareWithVendor { get; set; }
        public string Path { get; set; }
        public byte DocumentTypeid { get; set; }
        public bool Deleteflag { get; set; }
    
        public virtual MPRItemInfo MPRItemInfo { get; set; }
        public virtual MPRRevision MPRRevision { get; set; }
    }
}