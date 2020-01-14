using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMModels.RFQModels
{
   public class RfqDocumentsModel
    {
        public int RfqDocumentId { get; set; }
        public int RfqRevisionId { get; set; }
        public Nullable<int> RfqItemsId { get; set; }
        public string DocumentName { get; set; }
        public int DocumentType { get; set; }
        public string Path { get; set; }
        public string UploadedBy { get; set; }
        public DateTime UploadedDate { get; set; }
        public string StatusRemarks { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> Statusdate { get; set; }
        public string StatusBy { get; set; }
        public bool IsDeleted { get; set; }
        public RfqItemModel itemmodel { get; set; }
        public RfqRevisionModel revisionmodel { get; set; }
    }
}
