using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMModels.RFQModels
{
   public class MPRItemInfoModel
    {
        public MPRItemInfoModel()
        {
            items = new List<RfqItemModel>();
        }
        public int Itemdetailsid { get; set; }
        public Nullable<int> RevisionId { get; set; }
        public int Itemid { get; set; }
        public string ItemDescription { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public Nullable<byte> UnitId { get; set; }
        public string SaleOrderNo { get; set; }
        public string SOLineItemNo { get; set; }
        public Nullable<decimal> TargetSpend { get; set; }
        public string ReferenceDocNo { get; set; }
        public Nullable<bool> DeleteFlag { get; set; }
        public List<RfqItemModel> items { get; set; }
    }
}
