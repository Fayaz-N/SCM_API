using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMModels.MPRMasterModels
{
   public class MPRProcurementSourceModel
    {
        public MPRProcurementSourceModel()
        {
            //this.MPRRevisions = new HashSet<MPRRevision>();
        }

        public byte ProcurementSourceId { get; set; }
        public string ProcurementSource { get; set; }
        public bool BoolInUse { get; set; }
        //public virtual ICollection<MPRRevision> MPRRevisions { get; set; }
    }
}
