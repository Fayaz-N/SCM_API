using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMModels.MPRMasterModels
{
    public class MPRBuyerGroupModel
    {
        public MPRBuyerGroupModel()
        {
            MPRBuyerGroup = new List<MPRBuyerGroupMemberModel>();
            Mprrevisions = new List<MPRRevisionModel>();
        }
        public byte BuyerGroupId { get; set; }
        public string BuyerGroup { get; set; }
        public bool BoolInUse { get; set; }
        public List<MPRBuyerGroupMemberModel> MPRBuyerGroup { get; set; }
        public List<MPRRevisionModel> Mprrevisions { get; set; }
    }
}
