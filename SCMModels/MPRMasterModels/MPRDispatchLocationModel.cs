using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMModels.MPRMasterModels
{
    public class MPRDispatchLocationModel
    {
        public byte DispatchLocationId { get; set; }
        public string DispatchLocation { get; set; }
        public byte XOrder { get; set; }
        public bool BoolInUse { get; set; }
    }
}
