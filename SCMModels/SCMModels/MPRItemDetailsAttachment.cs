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
    
    public partial class MPRItemDetailsAttachment
    {
        public int ItemDetailsAttachmentId { get; set; }
        public int ItemDetailsId { get; set; }
        public string FileName { get; set; }
    
        public virtual MPRItemDetails_X MPRItemDetails_X { get; set; }
    }
}
