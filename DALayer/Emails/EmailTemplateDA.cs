using SCMModels.SCMModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace DALayer.Emails
{
    public class EmailTemplateDA : IEmailTemplateDA
    {
        public bool prepareEmailTemplate(string typeOfUser, int revisionId, string FrmEmailId, string ToEmailId, string Remarks)
        {

            try
            {
                using (var db = new YSCMEntities()) //ok
                {
                    MPRRevisionDetail mprrevisionDetails = db.MPRRevisionDetails.Where(li => li.RevisionId == revisionId).FirstOrDefault<MPRRevisionDetail>();
                    var issueOfPurpose = mprrevisionDetails.IssuePurposeId == 1 ? "For Enquiry" : "For Issuing PO";
                    EmailSend emlSndngList = new EmailSend();
                    if (typeOfUser == "")
                    {
                        emlSndngList.Body = "<html><meta charset=\"ISO-8859-1\"><head><link rel = 'stylesheet' href = 'https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css' ></head><body><div class='container'><table border='1' class='table table-bordered table-sm'><tr><td><b>MPR Number</b></td><td>" + mprrevisionDetails.DocumentNo + "</td><td><b>Document Description</b></td><td>" + mprrevisionDetails.DocumentDescription + "</td><td><b>Purpose of Iussing MPR</b></td><td>" + issueOfPurpose + "</td></tr><tr><td><b>Department</b></td><td>" + mprrevisionDetails.DepartmentName + "</td><td><b>Project Manager</td></td><td>" + mprrevisionDetails.ProjectManagerName + "</td><td><b>Job Code</b></td><td>" + mprrevisionDetails.JobCode + "</td></tr><tr><td><b>Client Name</b></td><td>" + mprrevisionDetails.ClientName + "</td><td><b>Job Name</b></td><td>" + mprrevisionDetails.JobName + "</td><td><b>Buyer Group</b></td><td>" + mprrevisionDetails.BuyerGroupName + "</td></tr><tr><td><b>Checker Name</b></td><td>" + mprrevisionDetails.CheckedName + "</td><td><b>Checker Status</b></td><td>" + mprrevisionDetails.CheckStatus + "</td><td><b>Checker Remarks</b></td><td>" + mprrevisionDetails.CheckerRemarks + "</td></tr><tr><td><b>Approver Name</b></td><td>" + mprrevisionDetails.ApproverName + "</td><td><b>Approver Status</b></td><td>" + mprrevisionDetails.ApprovalStatus + "</td><td><b>Approver Remarks</b></td><td>" + mprrevisionDetails.ApproverRemarks + "</td></tr></table><br/><br/><span><b>Remarks : <b/>" + Remarks + "</span><br/><br/><b>Click here to redirect : </b>&nbsp<a href='http://10.29.15.68:99/SCM/MPRForm/" + mprrevisionDetails.RevisionId + "'>http://10.29.15.68:99/SCM/MPRForm/" + mprrevisionDetails.RevisionId + " </a></div></body></html>";
                        Employee frmEmail = db.Employees.Where(li => li.EmployeeNo == FrmEmailId).FirstOrDefault<Employee>();
                        emlSndngList.FrmEmailId = frmEmail.EMail;
                        emlSndngList.Subject = "Comments From " + frmEmail.Name;
                        //emlSndngList.ToEmailId = "Developer@in.yokogawa.com";
                        emlSndngList.ToEmailId = (db.Employees.Where(li => li.EmployeeNo == ToEmailId).FirstOrDefault<Employee>()).EMail;
                        if (!string.IsNullOrEmpty(emlSndngList.ToEmailId))
                            this.sensEmail(emlSndngList);
                    }
                    else
                    {
                        emlSndngList.Body = "<html><meta charset=\"ISO-8859-1\"><head><link rel = 'stylesheet' href = 'https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css' ></head><body><div class='container'><table border='1' class='table table-bordered table-sm'><tr><td><b>MPR Number</b></td><td>" + mprrevisionDetails.DocumentNo + "</td><td><b>Document Description</b></td><td>" + mprrevisionDetails.DocumentDescription + "</td><td><b>Purpose of Iussing MPR</b></td><td>" + issueOfPurpose + "</td></tr><tr><td><b>Department</b></td><td>" + mprrevisionDetails.DepartmentName + "</td><td><b>Project Manager</td></td><td>" + mprrevisionDetails.ProjectManagerName + "</td><td><b>Job Code</b></td><td>" + mprrevisionDetails.JobCode + "</td></tr><tr><td><b>Client Name</b></td><td>" + mprrevisionDetails.ClientName + "</td><td><b>Job Name</b></td><td>" + mprrevisionDetails.JobName + "</td><td><b>Buyer Group</b></td><td>" + mprrevisionDetails.BuyerGroupName + "</td></tr><tr><td><b>Checker Name</b></td><td>" + mprrevisionDetails.CheckedName + "</td><td><b>Checker Status</b></td><td>" + mprrevisionDetails.CheckStatus + "</td><td><b>Checker Remarks</b></td><td>" + mprrevisionDetails.CheckerRemarks + "</td></tr><tr><td><b>Approver Name</b></td><td>" + mprrevisionDetails.ApproverName + "</td><td><b>Approver Status</b></td><td>" + mprrevisionDetails.ApprovalStatus + "</td><td><b>Approver Remarks</b></td><td>" + mprrevisionDetails.ApproverRemarks + "</td></tr></table><br/><br/><b>Click here to redirect : </b>&nbsp<a href='http://10.29.15.68:99/SCM/MPRForm/" + mprrevisionDetails.RevisionId + "'>http://10.29.15.68:99/SCM/MPRForm/" + mprrevisionDetails.RevisionId + "</a></div></body></html>";
                        if (typeOfUser == "Requestor")
                        {
                            emlSndngList.FrmEmailId = (db.Employees.Where(li => li.EmployeeNo == mprrevisionDetails.PreparedBy).FirstOrDefault<Employee>()).EMail;
                            emlSndngList.Subject = "MPR Information: " + mprrevisionDetails.DocumentNo + " ; " + "Approver Status: " + mprrevisionDetails.ApprovalStatus;
                            emlSndngList.ToEmailId = (db.Employees.Where(li => li.EmployeeNo == mprrevisionDetails.CheckedBy).FirstOrDefault<Employee>()).EMail;
                            if (!string.IsNullOrEmpty(emlSndngList.ToEmailId))
                                this.sensEmail(emlSndngList);
                            emlSndngList.Subject = "MPR Information: " + mprrevisionDetails.DocumentNo + " ; " + "Checker Status: " + mprrevisionDetails.CheckStatus;
                            emlSndngList.ToEmailId = (db.Employees.Where(li => li.EmployeeNo == mprrevisionDetails.ApprovedBy).FirstOrDefault<Employee>()).EMail;
                            if (!string.IsNullOrEmpty(emlSndngList.ToEmailId))
                                this.sensEmail(emlSndngList);
                        }
                        if (typeOfUser == "Checker")
                        {
                            //emlSndngList.FrmEmailId = "Developer@in.yokogawa.com";
                            //emlSndngList.ToEmailId = "Developer@in.yokogawa.com";
                            emlSndngList.FrmEmailId = (db.Employees.Where(li => li.EmployeeNo == mprrevisionDetails.CheckedBy).FirstOrDefault<Employee>()).EMail;
                            emlSndngList.Subject = "MPR Information: " + mprrevisionDetails.DocumentNo + " ; " + "Checker Status: " + mprrevisionDetails.CheckStatus + " ; " + "Approver Status: " + mprrevisionDetails.ApprovalStatus;
                            emlSndngList.ToEmailId = (db.Employees.Where(li => li.EmployeeNo == mprrevisionDetails.PreparedBy).FirstOrDefault<Employee>()).EMail;
                            if (!string.IsNullOrEmpty(emlSndngList.ToEmailId))
                                this.sensEmail(emlSndngList);
                            if (mprrevisionDetails.CheckStatus == "Approved")
                            {
                                emlSndngList.Subject = "MPR Information: " + mprrevisionDetails.DocumentNo + " ; " + "Checker Status: " + mprrevisionDetails.CheckStatus;
                                emlSndngList.ToEmailId = (db.Employees.Where(li => li.EmployeeNo == mprrevisionDetails.ApprovedBy).FirstOrDefault<Employee>()).EMail;
                                if (!string.IsNullOrEmpty(emlSndngList.ToEmailId))
                                    this.sensEmail(emlSndngList);
                            }
                        }
                        if (typeOfUser == "Approver")
                        {
                            emlSndngList.FrmEmailId = (db.Employees.Where(li => li.EmployeeNo == mprrevisionDetails.ApprovedBy).FirstOrDefault<Employee>()).EMail;
                            emlSndngList.Subject = "MPR Information: " + mprrevisionDetails.DocumentNo + " ; " + "Checker Status: " + mprrevisionDetails.CheckStatus + " ; " + "Approver Status: " + mprrevisionDetails.ApprovalStatus;
                            emlSndngList.ToEmailId = (db.Employees.Where(li => li.EmployeeNo == mprrevisionDetails.PreparedBy).FirstOrDefault<Employee>()).EMail;
                            if (!string.IsNullOrEmpty(emlSndngList.ToEmailId))
                                this.sensEmail(emlSndngList);
                            emlSndngList.Subject = "MPR Information: " + mprrevisionDetails.DocumentNo + " ; " + "Approver Status: " + mprrevisionDetails.ApprovalStatus;
                            emlSndngList.ToEmailId = (db.Employees.Where(li => li.EmployeeNo == mprrevisionDetails.CheckedBy).FirstOrDefault<Employee>()).EMail;
                            if (!string.IsNullOrEmpty(emlSndngList.ToEmailId))
                                this.sensEmail(emlSndngList);
                            if (mprrevisionDetails.ApprovalStatus == "Approved")
                            {
                                emlSndngList.Subject = "MPR Information: " + mprrevisionDetails.DocumentNo + " ; " + "Checker Status: " + mprrevisionDetails.CheckStatus + " ; " + "Approver Status: " + mprrevisionDetails.ApprovalStatus;
                                emlSndngList.ToEmailId = (db.Employees.Where(li => li.EmployeeNo == mprrevisionDetails.SecondApprover).FirstOrDefault<Employee>()).EMail;
                                if (!string.IsNullOrEmpty(emlSndngList.ToEmailId))
                                    this.sensEmail(emlSndngList);
                            }
                        }
                        if (typeOfUser == "SecondApprover")
                        {
                            emlSndngList.FrmEmailId = (db.Employees.Where(li => li.EmployeeNo == mprrevisionDetails.ApprovedBy).FirstOrDefault<Employee>()).EMail;
                            emlSndngList.Subject = "MPR Information: " + mprrevisionDetails.DocumentNo + " ; " + "Checker Status: " + mprrevisionDetails.CheckStatus + " ; " + "Approver Status: " + mprrevisionDetails.ApprovalStatus;
                            emlSndngList.ToEmailId = (db.Employees.Where(li => li.EmployeeNo == mprrevisionDetails.PreparedBy).FirstOrDefault<Employee>()).EMail;
                            if (!string.IsNullOrEmpty(emlSndngList.ToEmailId))
                                this.sensEmail(emlSndngList);

                            emlSndngList.Subject = "MPR Information: " + mprrevisionDetails.DocumentNo + " ; " + "Approver Status: " + mprrevisionDetails.ApprovalStatus;
                            emlSndngList.ToEmailId = (db.Employees.Where(li => li.EmployeeNo == mprrevisionDetails.CheckedBy).FirstOrDefault<Employee>()).EMail;
                            if (!string.IsNullOrEmpty(emlSndngList.ToEmailId))
                                this.sensEmail(emlSndngList);

                            emlSndngList.Subject = "MPR Information: " + mprrevisionDetails.DocumentNo + " ; " + "Checker Status: " + mprrevisionDetails.CheckStatus + " ; " + "Approver Status: " + mprrevisionDetails.ApprovalStatus;
                            emlSndngList.ToEmailId = (db.Employees.Where(li => li.EmployeeNo == mprrevisionDetails.ApprovedBy).FirstOrDefault<Employee>()).EMail;
                            if (!string.IsNullOrEmpty(emlSndngList.ToEmailId))
                                this.sensEmail(emlSndngList);

                            if (mprrevisionDetails.ApprovalStatus == "Approved")
                            {
                                emlSndngList.Subject = "MPR Information: " + mprrevisionDetails.DocumentNo + " ; " + "Checker Status: " + mprrevisionDetails.CheckStatus + " ; " + "Approver Status: " + mprrevisionDetails.ApprovalStatus + " ; " + "Second Approval Status: " + mprrevisionDetails.SecondApproversStatus;
                                emlSndngList.ToEmailId = (db.Employees.Where(li => li.EmployeeNo == mprrevisionDetails.ThirdApprover).FirstOrDefault<Employee>()).EMail;
                                if (!string.IsNullOrEmpty(emlSndngList.ToEmailId))
                                    this.sensEmail(emlSndngList);
                            }
                        }
                        if (typeOfUser == "ThirdApprover")
                        {
                            emlSndngList.FrmEmailId = (db.Employees.Where(li => li.EmployeeNo == mprrevisionDetails.ApprovedBy).FirstOrDefault<Employee>()).EMail;
                            emlSndngList.Subject = "MPR Information: " + mprrevisionDetails.DocumentNo + " ; " + "Checker Status: " + mprrevisionDetails.CheckStatus + " ; " + "Approver Status: " + mprrevisionDetails.ApprovalStatus;
                            emlSndngList.ToEmailId = (db.Employees.Where(li => li.EmployeeNo == mprrevisionDetails.PreparedBy).FirstOrDefault<Employee>()).EMail;
                            if (!string.IsNullOrEmpty(emlSndngList.ToEmailId))
                                this.sensEmail(emlSndngList);

                            emlSndngList.Subject = "MPR Information: " + mprrevisionDetails.DocumentNo + " ; " + "Approver Status: " + mprrevisionDetails.ApprovalStatus;
                            emlSndngList.ToEmailId = (db.Employees.Where(li => li.EmployeeNo == mprrevisionDetails.CheckedBy).FirstOrDefault<Employee>()).EMail;
                            if (!string.IsNullOrEmpty(emlSndngList.ToEmailId))
                                this.sensEmail(emlSndngList);

                            emlSndngList.Subject = "MPR Information: " + mprrevisionDetails.DocumentNo + " ; " + "Checker Status: " + mprrevisionDetails.CheckStatus + " ; " + "Approver Status: " + mprrevisionDetails.ApprovalStatus;
                            emlSndngList.ToEmailId = (db.Employees.Where(li => li.EmployeeNo == mprrevisionDetails.ApprovedBy).FirstOrDefault<Employee>()).EMail;
                            if (!string.IsNullOrEmpty(emlSndngList.ToEmailId))
                                this.sensEmail(emlSndngList);


                            emlSndngList.Subject = "MPR Information: " + mprrevisionDetails.DocumentNo + " ; " + "Checker Status: " + mprrevisionDetails.CheckStatus + " ; " + "Approver Status: " + mprrevisionDetails.ApprovalStatus + " ; " + "Second Approval Status: " + mprrevisionDetails.SecondApproversStatus;
                            emlSndngList.ToEmailId = (db.Employees.Where(li => li.EmployeeNo == mprrevisionDetails.SecondApprover).FirstOrDefault<Employee>()).EMail;
                            if (!string.IsNullOrEmpty(emlSndngList.ToEmailId))
                                this.sensEmail(emlSndngList);

                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;

        }

        public bool sensEmail(EmailSend emlSndngList)
        {
            MailMessage mailMessage = new MailMessage(emlSndngList.FrmEmailId, emlSndngList.ToEmailId);
            SmtpClient client = new SmtpClient();
            if (!string.IsNullOrEmpty(emlSndngList.Subject))
                mailMessage.Subject = emlSndngList.Subject;
            if (!string.IsNullOrEmpty(emlSndngList.CC))
                mailMessage.CC.Add(emlSndngList.CC);
            mailMessage.Body = emlSndngList.Body;
            mailMessage.IsBodyHtml = true;
            mailMessage.BodyEncoding = Encoding.UTF8;
            SmtpClient mailClient = new SmtpClient("10.29.15.9", 25);
            //mailClient.EnableSsl = true;
            mailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            mailClient.Send(mailMessage);
            return true;
        }
    }

    public class EmailSend
    {
        public string FrmEmailId { get; set; }
        public string ToEmailId { get; set; }
        public string CC { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
