using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMModels.RFQModels
{
   public class YILTermsandConditionModel
    {
        public int TermId { get; set; }
        public byte TermGroupId { get; set; }
        public int BuyerGroupId { get; set; }
        public string Terms { get; set; }
        public Nullable<int> DisplayOrder { get; set; }
        public bool DefaultSelect { get; set; }
        public string DeleteFlag { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
    }
}
