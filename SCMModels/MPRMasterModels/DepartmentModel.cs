using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMModels.MPRMasterModels
{
   public class DepartmentModel
    {
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentHead { get; set; }
        public int  BUCode { get; set; }
        public bool BoolinUse { get; set; }
    }
}
