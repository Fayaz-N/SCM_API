using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMModels.RFQModels
{
   public class PAFunctionalRolesModel
    {
        public PAFunctionalRolesModel()
        {
            Mapping = new List<PAAuthorizationEmployeeMappingModel>();
        }
        public string FunctionalRoleId { get; set; }
        public Nullable<byte> DepartmentID { get; set; }
        public string FunctionalRole { get; set; }
        public Nullable<byte> XOrder { get; set; }
        public Nullable<bool> DeleteFlag { get; set; }
        public List<PAAuthorizationEmployeeMappingModel> Mapping { get; set; }
    }
}
