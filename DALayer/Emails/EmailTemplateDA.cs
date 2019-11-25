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
		public bool prepareEmailTemplate(string typeOfUser, int revisionId)
		{

			try
			{
				using (var db = new YSCMEntities()) //ok
				{
					MPRRevision mprrevisionDetails = db.MPRRevisions.Where(li => li.RevisionId == revisionId).FirstOrDefault<MPRRevision>();
					var issueOfPurpose = mprrevisionDetails.IssuePurposeId == 1 ? "For Enquiry" : "For Issuing PO";
					EmailSend emlSndngList = new EmailSend();
					emlSndngList.Subject = "MPR Information:" + mprrevisionDetails.MPRDetail.DocumentNo + "";
					emlSndngList.Body = "<html><head><link rel = 'stylesheet' href = 'https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css' ></head><body><div class='container'><table class='table table-bordered table-sm'><tr><td><b>MPR Number</b></td><td>" + mprrevisionDetails.MPRDetail.DocumentNo + "</td><td><b>Document Description</b></td><td>" + mprrevisionDetails.MPRDetail.DocumentDescription + "</td><td><b>Purpose of Iussing MPR</b></td><td>" + issueOfPurpose + "</td></tr><tr><td><b>Department</b></td><td>" + mprrevisionDetails.DepartmentId + "</td><td><b>Project Manager</td></td><td>" + mprrevisionDetails.ProjectManager + "</td><td><b>Job Code</b></td><td>" + mprrevisionDetails.JobCode + "</td></tr></table></div></body></html>";
					if (typeOfUser == "Requestor")
					{
						//emlSndngList.FrmEmailId = (db.Employees.Where(li => li.EmployeeNo == mprrevisionDetails.PreparedBy).FirstOrDefault<Employee>()).EMail;
						emlSndngList.FrmEmailId = "Developer@in.yokogawa.com";
						emlSndngList.ToEmailId = "Developer@in.yokogawa.com";
						//emlSndngList.ToEmailId= (db.Employees.Where(li => li.EmployeeNo == mprrevisionDetails.CheckedBy).FirstOrDefault<Employee>()).EMail;
						this.sensEmail(emlSndngList);
					}
					if (typeOfUser == "Checker")
					{
						emlSndngList.FrmEmailId = "Developer@in.yokogawa.com";
						emlSndngList.ToEmailId = "Developer@in.yokogawa.com";
						this.sensEmail(emlSndngList);
						//emlSndngList.FrmEmailId = (db.Employees.Where(li => li.EmployeeNo == mprrevisionDetails.CheckedBy).FirstOrDefault<Employee>()).EMail;
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
