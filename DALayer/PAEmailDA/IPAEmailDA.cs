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
        bool paemailstatus(int paid, int statusid,int mprrevisionid,string ApprovalStatus,string employeeno);
        bool PAEmailRequestForApproval(int paid, string ToEmailId);
    }
}
