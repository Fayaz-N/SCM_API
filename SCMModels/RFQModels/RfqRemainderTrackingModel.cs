using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMModels.RFQModels
{
   public class RfqRemainderTrackingModel
    {
        public int Reminderid { get; set; }
        public int rfqccid { get; set; }
        public string ReminderTo { get; set; }
        public Nullable<System.DateTime> MailsSentOn { get; set; }
        public Nullable<System.DateTime> Acknowledgementon { get; set; }
        public string AcknowledgementRemarks { get; set; }
        public bool DeleteFlag { get; set; }
        public RfqCommunicationModel RFQCommunication { get; set; }
    }
}
