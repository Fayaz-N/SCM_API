using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMModels.MPRMasterModels
{
	public class MPRApproversViewModel
	{
		public string Name { get; set; }
		public string EmployeeNo { get; set; }
		public bool BoolActive { get; set; }
		public string DeactivatedByEmpNo { get; set; }
		public string DeactivatedBy { get; set; }
		public Nullable<System.DateTime> DeactivatedOn { get; set; }
	}
}
