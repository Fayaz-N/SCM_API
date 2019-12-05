using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMModels.MPRMasterModels
{
   public class MPRCustomsDutyModel
    {
        public MPRCustomsDutyModel()
        {
            //this.MPRRevisions = new HashSet<MPRRevision>();
        }
        public byte CustomsDutyId { get; set; }
        public string CustomsDuty { get; set; }
        public bool BoolInUse { get; set; }
        //public virtual ICollection<MPRRevision> MPRRevisions { get; set; }
    }
}
