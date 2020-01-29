using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMModels.RFQModels
{
   public class RfqCommunicationModel
    {
        public RfqCommunicationModel()
        {
            Rfqtracking = new List<RfqRemainderTrackingModel>();
        }
        public int Rfqccid { get; set; }
        public Nullable<int> RfqItemId { get; set; }
        public int RfqRevisionId { get; set; }
        public string RemarksFrom { get; set; }
        public string RemarksTo { get; set; }
        public Nullable<bool> SendEmail { get; set; }
        public Nullable<bool> SetReminder { get; set; }
        public Nullable<System.DateTime> ReminderDate { get; set; }
        public DateTime RemarksDate { get; set; }
        public string Remarks { get; set; }
        public bool DeleteFlag { get; set; }
        public RfqItemModel RfqItem { get; set; }
        public RfqRevisionModel RfqRevision { get; set; }
        public List<RfqRemainderTrackingModel> Rfqtracking { get; set; }

    }
}
