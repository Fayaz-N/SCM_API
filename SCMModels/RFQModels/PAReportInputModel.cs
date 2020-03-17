using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMModels.RFQModels
{
   public class PAReportInputModel
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int MPRRevisionId { get; set; }

    }
}
