using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMModels.RFQModels
{
    public class RfqItemInfoModel
    {
        public int RFQSplitItemId { get; set; }
        public int RFQItemsId { get; set; }
        public Nullable<double> StartQty { get; set; }
        public Nullable<double> EndQty { get; set; }
        public double Qty { get; set; }
        public int UOM { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal Discount { get; set; }
        public Nullable<decimal> DiscountAmount { get; set; }
        public int CurrencyId { get; set; }
        public decimal CurrencyValue { get; set; }
        public string Remarks { get; set; }
        public DateTime DeliveryDate { get; set; }
        public Nullable<int> DeliveryDays { get; set; }
        public Nullable<System.DateTime> ValidFrom { get; set; }
        public Nullable<System.DateTime> ValidTo { get; set; }
        public bool IsDeleted { get; set; }
        public RfqItemModel item { get; set; }
    }
}
