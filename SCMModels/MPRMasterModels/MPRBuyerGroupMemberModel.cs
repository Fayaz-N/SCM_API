using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMModels.MPRMasterModels
{
   public class MPRBuyerGroupMemberModel
    {
        public short BuyerGroupMemberId { get; set; }
        public byte BuyerGroupId { get; set; }
        public string GroupMember { get; set; }

        public  MPRBuyerGroupModel MPRBuyerGroup { get; set; }
    }
}
