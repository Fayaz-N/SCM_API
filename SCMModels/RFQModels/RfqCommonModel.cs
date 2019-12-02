using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMModels.RFQModels
{
   public class RfqCommonModel
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string ModelName { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
