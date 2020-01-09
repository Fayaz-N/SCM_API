using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMModels.RFQModels
{
   public class EmployeModel
    {
        public string EmployeeNo { get; set; }
        public string Name { get; set; }
        public string EMail { get; set; }
        public Nullable<byte> DeptID { get; set; }
    }
}
