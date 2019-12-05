using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMModels.MPRMasterModels
{
   public class MPRDepartmentModel
    {
        public byte DepartmentId { get; set; }
        public string Department { get; set; }
        public string SecondApprover { get; set; }
        public string ThirdApprover { get; set; }
        public bool BoolInUse { get; set; }
        //public virtual ICollection<MPRRevision> MPRRevisions { get; set; }
    }
}
