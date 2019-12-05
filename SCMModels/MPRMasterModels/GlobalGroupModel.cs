using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMModels.MPRMasterModels
{
   public class GlobalGroupModel
    {
        public GlobalGroupModel()
        {
            GlobalGroupEmployees = new List<GlobalGroupEmployeeModel>();
        }

        public short GlobalGroupId { get; set; }
        public string GlobalGroupDescription { get; set; }
        public  List<GlobalGroupEmployeeModel> GlobalGroupEmployees { get; set; }
    }
}

