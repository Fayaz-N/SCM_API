using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using SCMModels.SCMModels;
using System.Configuration;

namespace DALayer.PAEmailDA
{
   public class PAEmailDA:IPAEmailDA
    {
        YSCMEntities obj = new YSCMEntities();
        public bool PAEmailRequest( int paid, string loginemployee)
        {
            var ipaddress = ConfigurationManager.AppSettings["UI_IpAddress"];
            var data = obj.MPRPAApprovers.Where(x => x.PAId == paid).ToList();
            ipaddress = ipaddress + "SCM/mprpa/" + paid + "";
            EmailSend mails = new EmailSend();
            mails.FrmEmailId = obj.Employees.Where(x => x.EmployeeNo == loginemployee).FirstOrDefault().EMail;
            //mails.CC = "n.senthilkumar@in.yokogawa.com";
            foreach (var item in data)
            {
                var maildata = obj.Mailsendingviews.Where(x => x.Approver==item.Approver && x.PAId==paid).FirstOrDefault();
                mails.Body = "<html><meta charset=\"ISO-8859-1\"><head><link rel = 'stylesheet' href = 'https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css' ></head>" +
                    "<body>" +
                    "<div class='container'>" +
                    "<table border='1' class='table table-bordered table-sm'>" +
                    "<tr>" +
                    "<td><b>MPR Number</b></td><td>" + maildata.DocumentNo + "</td>" +
                    "<td><b>Document Description</b></td><td>" + maildata.DocumentDescription + "</td>" +
                    "<td><b>Vendor</b></td><td>" + maildata.VendorName + "</td>" +
                    "</tr>" +
                    "<tr>" +
                    "<td><b>Department</b></td><td>" + maildata.Department + "</td>" +
                    "<td><b>Project Manager</td></td><td>" + maildata.projectmanagername + "</td>" +
                    "<td><b>Buyer Group</b></td><td>" + maildata.BuyerGroup + "</td></tr>" +
                    "</tr>" +
                    "<tr>" +
                    "<td><b>Paid</b></td><td>" + maildata.PAId + "</td>" +
                    "<td><b>PARequested</b></td><td>" + maildata.parequestedname + "</td>" +
                    "<td><b>RequestedOn</b></td><td>" + maildata.RequestedOn + "</td>" +
                    "</tr>" +
                    "<tr>"+
                    "<td><b>Approver Name</b></td><td>" + maildata.approvername + "</td>" +
                    "<td><b>Approver Status</b></td><td>" + maildata.ApprovalStatus + "</td>" +
                    "<td><b>Approver Remarks</b></td><td>" + maildata.ApproversRemarks + "</td>" +
                    "</tr>" +
                    "</table><br/><b>Please Click The Below Link To Approve:</b><br/>&nbsp<a href='" + ipaddress + "'>" + ipaddress + " </a></div></body></html>";
                mails.ToEmailId = obj.Employees.Where(x => x.EmployeeNo == item.Approver).FirstOrDefault().EMail;
                mails.Subject = "Purchase Authorization Waiting For Your Approval";
                if (mails.FrmEmailId != "NULL" && mails.ToEmailId != "NULL")
                    this.sendEmail(mails);
                
            }
            return true;
        }

        public bool sendEmail(EmailSend emlSndngList)
        {
            bool validEmail = IsValidEmail(emlSndngList.ToEmailId);
            if (!string.IsNullOrEmpty(emlSndngList.ToEmailId) && !string.IsNullOrEmpty(emlSndngList.FrmEmailId) && validEmail)
            {
                var BCC = ConfigurationManager.AppSettings["PABCC"];
                var SMTPServer = ConfigurationManager.AppSettings["SMTPServer"];
                MailMessage mailMessage = new MailMessage(emlSndngList.FrmEmailId, emlSndngList.ToEmailId);
                SmtpClient client = new SmtpClient();
                if (!string.IsNullOrEmpty(emlSndngList.Subject))
                    mailMessage.Subject = emlSndngList.Subject;
                if (!string.IsNullOrEmpty(emlSndngList.CC))
                    mailMessage.CC.Add(emlSndngList.CC);
                if (!string.IsNullOrEmpty(BCC))
                    mailMessage.Bcc.Add(BCC);
                mailMessage.Body = emlSndngList.Body;
                mailMessage.IsBodyHtml = true;
                mailMessage.BodyEncoding = Encoding.UTF8;
                SmtpClient mailClient = new SmtpClient(SMTPServer, 25);
                //SmtpClient mailClient = new SmtpClient("10.29.15.9", 25);
                //mailClient.EnableSsl = true;
                mailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                mailClient.Send(mailMessage);
            }
            return true;

            bool IsValidEmail(string email)
            {
                try
                {
                    var addr = new System.Net.Mail.MailAddress(email);
                    return addr.Address == email;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool paemailstatus(int statusid, int paid,int mprrevisionid,string ApprovalStatus,string employeeno)
        {
            var ipaddress = ConfigurationManager.AppSettings["UI_IpAddress"];
            var ipaddress1 = ConfigurationManager.AppSettings["UI_IpAddress"];
            string parequestby = obj.MPRPADetails.Where(x => x.PAId == paid).FirstOrDefault().RequestedBy;
            ipaddress = ipaddress + "SCM/mprpa/" + paid + "";
            ipaddress1= ipaddress1 + "SCM/MPRForm/" + mprrevisionid + "";
            EmailSend mails = new EmailSend();
            mails.FrmEmailId = obj.Employees.Where(x => x.EmployeeNo == employeeno).FirstOrDefault().EMail;
            string mprpreparedby = obj.MPRRevisions.Where(x => x.RevisionId == mprrevisionid).FirstOrDefault().PreparedBy;
            if (ApprovalStatus=="Approved")
            {
                var maildata = obj.Mailsendingviews.Where(x => x.Approver == employeeno).FirstOrDefault();
                mails.Body = "<html><meta charset=\"ISO-8859-1\"><head><link rel = 'stylesheet' href = 'https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css' ></head>" +
                    "<body>" +
                    "<div class='container'>" +
                    "<table border='1' class='table table-bordered table-sm'>" +
                    "<tr>" +
                    "<td><b>Paid</b></td><td>" + maildata.PAId + "</td>" +
                    "<td><b>PARequested</b></td><td>" + maildata.parequestedname + "</td>" +
                    "<td><b>RequestedOn</b></td><td>" + maildata.RequestedOn + "</td>" +
                    "</tr>" +
                    "<tr>" +
                    "<td><b>Approver Name</b></td><td>" + maildata.Approver + "</td>" +
                    "<td><b>Approver Status</b></td><td>" + maildata.ApproversRemarks + "</td>" +
                    "<td><b>Approver Remarks</b></td><td>" + maildata.ApproversRemarks + "</td>" +
                    "</tr>" +
                    "</table><br/><b>Please click the below link to View:</b><br/>&nbsp<a href='" + ipaddress + "'>" + ipaddress + " </a></div></body></html>";
                mails.ToEmailId = obj.Employees.Where(x => x.EmployeeNo == parequestby).FirstOrDefault().EMail;
                mails.Subject = "Purchase Authorization Is Approved";
                Nullable<byte> buyergroup = obj.MPRRevisions.Where(x => x.RevisionId == mprrevisionid).FirstOrDefault().BuyerGroupId;
                string BuyerManager = obj.MPRBuyerGroups.Where(x => x.BuyerGroupId == buyergroup).FirstOrDefault().BuyerManager;
                mails.CC = obj.Employees.Where(x => x.EmployeeNo == BuyerManager).FirstOrDefault().EMail;
                if (mails.FrmEmailId!="NULL" && mails.ToEmailId!= "NULL")
                    sendEmail(mails);
                mails.ToEmailId = obj.Employees.Where(x => x.EmployeeNo == mprpreparedby).FirstOrDefault().EMail;
                mails.Body = "<html><meta charset=\"ISO-8859-1\"><head><link rel = 'stylesheet' href = 'https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css' ></head><body><div class='container'><br/><b>Please click the below link to View:</b><br/>&nbsp<a href='" + ipaddress1 + "'>" + ipaddress1 + " </a></div></body></html>";
                mails.CC = "";
                if (mails.FrmEmailId != "NULL" && mails.ToEmailId != "NULL")
                    sendEmail(mails);
            }
            else
            {
                mails.Body = "<html><meta charset=\"ISO-8859-1\"><head><link rel = 'stylesheet' href = 'https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css' ></head><body><div class='container'><br/><b>Please click the below link to View:</b><br/>&nbsp<a href='" + ipaddress + "'>" + ipaddress + " </a></div></body></html>";
                mails.ToEmailId = obj.Employees.Where(x => x.EmployeeNo == parequestby).FirstOrDefault().EMail;
                mails.Subject = "Purchase Authorization Is Rejected";
                Nullable<byte> buyergroup = obj.MPRRevisions.Where(x => x.RevisionId == mprrevisionid).FirstOrDefault().BuyerGroupId;
                string BuyerManager = obj.MPRBuyerGroups.Where(x => x.BuyerGroupId == buyergroup).FirstOrDefault().BuyerManager;
                mails.CC = obj.Employees.Where(x => x.EmployeeNo == BuyerManager).FirstOrDefault().EMail;
                if (mails.FrmEmailId != "NULL" && mails.ToEmailId != "NULL")
                    sendEmail(mails);
                mails.ToEmailId = obj.Employees.Where(x => x.EmployeeNo == mprpreparedby).FirstOrDefault().EMail;
                mails.Body = "<html><meta charset=\"ISO-8859-1\"><head><link rel = 'stylesheet' href = 'https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css' ></head><body><div class='container'><br/><b>Please click the below link to View:</b><br/>&nbsp<a href='" + ipaddress1 + "'>" + ipaddress1 + " </a></div></body></html>";
                mails.CC = "";
                if (mails.FrmEmailId != "NULL" && mails.ToEmailId != "NULL")
                    sendEmail(mails);
                //mails.Body = "<html><meta charset=\"ISO-8859-1\"><head><link rel = 'stylesheet' href = 'https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css' ></head><body><div class='container'><br/><b>Please Click The Below Link To Approve:</b><br/>&nbsp<a href='" + ipaddress + "'>" + ipaddress + " </a></div></body></html>";
                //mails.ToEmailId = obj.Employees.Where(x => x.EmployeeNo == parequestby).FirstOrDefault().EMail;
                //mails.mprpreparedby = obj.Employees.Where(x => x.EmployeeNo == mprpreparedby).FirstOrDefault().EMail;
                //mails.Subject = "Purchase Authorization Is Rejected";
                //sendEmail(mails);
            }
            return true;
        }
        public bool PAEmailRequestForApproval(int paid, string ToEmailId,string EmployeeNo)
        {
            EmailSend emails = new EmailSend();
            var ipaddress = ConfigurationManager.AppSettings["UI_IpAddress"];
            ipaddress = ipaddress + "SCM/mprpa/" + paid + "";
            string employeeno = obj.MPRPADetails.Where(x => x.PAId == paid).FirstOrDefault().RequestedBy;
            var maildata = obj.Mailsendingviews.Where(x => x.Approver == employeeno).FirstOrDefault();
            try
            {
                emails.ToEmailId = ToEmailId;
                emails.FrmEmailId = obj.Employees.Where(x=>x.EmployeeNo==employeeno).FirstOrDefault().EMail;
                emails.Subject = "Reminder for Purchase Authorization to approve";
                emails.Body = "<html><meta charset=\"ISO-8859-1\"><head><link rel = 'stylesheet' href = 'https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css' ></head>" +
                    "<body>" +
                    "<div class='container'>" +
                    "<table border='1' class='table table-bordered table-sm'>" +
                    "<tr>" +
                    "<td><b>MPR Number</b></td><td>" + maildata.DocumentNo + "</td>" +
                    "<td><b>Document Description</b></td><td>" + maildata.DocumentDescription + "</td>" +
                    "<td><b>Vendor</b></td><td>" + maildata.DocumentDescription + "</td>" +
                    "</tr>" +
                    "<tr>" +
                    "<td><b>Department</b></td><td>" + maildata.Department + "</td>" +
                    "<td><b>Project Manager</td></td><td>" + maildata.projectmanagername + "</td>" +
                    "<td><b>Buyer Group</b></td><td>" + maildata.BuyerGroup + "</td></tr>" +
                    "</tr>" +
                    "<tr>" +
                    "<td><b>Paid</b></td><td>" + maildata.PAId + "</td>" +
                    "<td><b>PARequested</b></td><td>" + maildata.parequestedname + "</td>" +
                    "<td><b>RequestedOn</b></td><td>" + maildata.RequestedOn + "</td>" +
                    "</tr>" +
                    "<tr>" +
                    "<td><b>Approver Name</b></td><td>" + maildata.Approver + "</td>" +
                    "<td><b>Approver Status</b></td><td>" + maildata.ApproversRemarks + "</td>" +
                    "<td><b>Approver Remarks</b></td><td>" + maildata.ApproversRemarks + "</td>" +
                    "</tr>" +
                    "</table><br/><b>Request For Purchase Authorization To Approve:</b><br/>&nbsp<a href='" + ipaddress + "'>" + ipaddress + " </a></div></body></html>";

                if (emails.FrmEmailId != "NULL" && emails.ToEmailId != "NULL")
                    sendEmail(emails);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }   
    }
   
    public class EmailSend
    {
        public string FrmEmailId { get; set; }
        public string ToEmailId { get; set; }
        public string CC { get; set; }
        public string mprpreparedby { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
