using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALayer.PAEmailDA
{
   public interface IPAEmailDA
    {
        bool PAEmailRequest(int paid, string FrmEmailId);
    }
}
