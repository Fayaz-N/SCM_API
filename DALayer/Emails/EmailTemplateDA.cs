/*
    Name of File : <<EmailTemplateDA>>  Author :<<Prasanna>>  
    Date of Creation <<01-12-2019>>
    Purpose : <<email template preparation to send emails>>
    Review Date :<<>>   Reviewed By :<<>>
    Version : 0.1 <change version only if there is major change - new release etc>
    Sourcecode Copyright : Yokogawa India Limited
*/
using SCMModels.SCMModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using SCMModels;
using System.Text.RegularExpressions;
using SCMModels.RemoteModel;
using DALayer.Common;

namespace DALayer.Emails
{


	/*Name of Class : <<EmailTemplateDA>>  Author :<<Prasanna>>  
    Date of Creation <<01-12-2019>>
    Purpose : <<email template preparation to send emails>
    Review Date :<<>>   Reviewed By :<<>>*/
	public class EmailTemplateDA : IEmailTemplateDA
	{
		private ErrorLog log = new ErrorLog();
		/*Name of Function : <<prepareMPREmailTemplate>>  Author :<<Prasanna>>  
		  Date of Creation <<01-12-2019>>
		  Purpose : <<preparing Email template to send status to requestiot,checker,approver,.. based onstatus>>
		  Review Date :<<>>   Reviewed By :<<>>*/
		public bool prepareMPREmailTemplate(string typeOfUser, int revisionId, string FrmEmailId, string ToEmailId, string Remarks)
		{

			try
			{
				using (var db = new YSCMEntities()) //ok
				{

					var ipaddress = ConfigurationManager.AppSettings["UI_IpAddress"];
					MPRRevisionDetail mprrevisionDetails = db.MPRRevisionDetails.Where(li => li.RevisionId == revisionId).FirstOrDefault<MPRRevisionDetail>();
					ipaddress = ipaddress + "SCM/MPRForm/" + mprrevisionDetails.RevisionId + "";
					var issueOfPurpose = mprrevisionDetails.IssuePurposeId == 1 ? "For Enquiry" : "For Issuing PO";
					EmailSend emlSndngList = new EmailSend();
					if (typeOfUser == "")
					{
						emlSndngList.Body = "<html><meta charset=\"ISO-8859-1\"><head><link rel = 'stylesheet' href = 'https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css' ></head><body><div class='container'><table border='1' class='table table-bordered table-sm'><tr><td><b>MPR Number</b></td><td>" + mprrevisionDetails.DocumentNo + "</td><td><b>Document Description</b></td><td>" + mprrevisionDetails.DocumentDescription + "</td><td><b>Purpose of Iussing MPR</b></td><td>" + issueOfPurpose + "</td></tr><tr><td><b>Department</b></td><td>" + mprrevisionDetails.DepartmentName + "</td><td><b>Project Manager</td></td><td>" + mprrevisionDetails.ProjectManagerName + "</td><td><b>Job Code</b></td><td>" + mprrevisionDetails.JobCode + "</td></tr><tr><td><b>Client Name</b></td><td>" + mprrevisionDetails.ClientName + "</td><td><b>Job Name</b></td><td>" + mprrevisionDetails.JobName + "</td><td><b>Buyer Group</b></td><td>" + mprrevisionDetails.BuyerGroupName + "</td></tr><tr><td><b>Checker Name</b></td><td>" + mprrevisionDetails.CheckedName + "</td><td><b>Checker Status</b></td><td>" + mprrevisionDetails.CheckStatus + "</td><td><b>Checker Remarks</b></td><td>" + mprrevisionDetails.CheckerRemarks + "</td></tr><tr><td><b>Approver Name</b></td><td>" + mprrevisionDetails.ApproverName + "</td><td><b>Approver Status</b></td><td>" + mprrevisionDetails.ApprovalStatus + "</td><td><b>Approver Remarks</b></td><td>" + mprrevisionDetails.ApproverRemarks + "</td></tr></table><br/><br/><span><b>Remarks : <b/>" + Remarks + "</span><br/><br/><b>Click here to redirect : </b>&nbsp<a href='" + ipaddress + "'>" + ipaddress + " </a></div></body></html>";
						Employee frmEmail = db.Employees.Where(li => li.EmployeeNo == FrmEmailId).FirstOrDefault<Employee>();
						emlSndngList.FrmEmailId = frmEmail.EMail;
						emlSndngList.Subject = "Comments From " + frmEmail.Name;
						//emlSndngList.ToEmailId = "Developer@in.yokogawa.com";
						emlSndngList.ToEmailId = (db.Employees.Where(li => li.EmployeeNo == ToEmailId).FirstOrDefault<Employee>()).EMail;
						if (emlSndngList.FrmEmailId != "NULL" && emlSndngList.ToEmailId != "NULL")
							this.sendEmail(emlSndngList);
					}
					else
					{
						emlSndngList.Body = "<html><meta charset=\"ISO-8859-1\"><head><link rel = 'stylesheet' href = 'https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css' ></head><body><div class='container'><table border='1' class='table table-bordered table-sm'><tr><td><b>MPR Number</b></td><td>" + mprrevisionDetails.DocumentNo + "</td><td><b>Document Description</b></td><td>" + mprrevisionDetails.DocumentDescription + "</td><td><b>Purpose of Iussing MPR</b></td><td>" + issueOfPurpose + "</td></tr><tr><td><b>Department</b></td><td>" + mprrevisionDetails.DepartmentName + "</td><td><b>Project Manager</td></td><td>" + mprrevisionDetails.ProjectManagerName + "</td><td><b>Job Code</b></td><td>" + mprrevisionDetails.JobCode + "</td></tr><tr><td><b>Client Name</b></td><td>" + mprrevisionDetails.ClientName + "</td><td><b>Job Name</b></td><td>" + mprrevisionDetails.JobName + "</td><td><b>Buyer Group</b></td><td>" + mprrevisionDetails.BuyerGroupName + "</td></tr><tr><td><b>Checker Name</b></td><td>" + mprrevisionDetails.CheckedName + "</td><td><b>Checker Status</b></td><td>" + mprrevisionDetails.CheckStatus + "</td><td><b>Checker Remarks</b></td><td>" + mprrevisionDetails.CheckerRemarks + "</td></tr><tr><td><b>Approver Name</b></td><td>" + mprrevisionDetails.ApproverName + "</td><td><b>Approver Status</b></td><td>" + mprrevisionDetails.ApprovalStatus + "</td><td><b>Approver Remarks</b></td><td>" + mprrevisionDetails.ApproverRemarks + "</td></tr></table><br/><br/><b>Click here to redirect : </b>&nbsp<a href='" + ipaddress + "'>" + ipaddress + "</a></div></body></html>";
						if (typeOfUser == "Requestor")
						{
							var res = db.Employees.Where(li => li.EmployeeNo == mprrevisionDetails.PreparedBy).FirstOrDefault<Employee>();
							emlSndngList.FrmEmailId = (db.Employees.Where(li => li.EmployeeNo == mprrevisionDetails.PreparedBy).FirstOrDefault<Employee>()).EMail;
							emlSndngList.Subject = "MPR Information: " + mprrevisionDetails.DocumentNo + " ; " + "Approver Status: " + mprrevisionDetails.ApprovalStatus;
							if (mprrevisionDetails.CheckedBy != "-" && mprrevisionDetails.CheckedBy != "")
							{
								emlSndngList.ToEmailId = (db.Employees.Where(li => li.EmployeeNo == mprrevisionDetails.CheckedBy).FirstOrDefault<Employee>()).EMail;
								if ((!string.IsNullOrEmpty(emlSndngList.FrmEmailId) && !string.IsNullOrEmpty(emlSndngList.FrmEmailId)) && (emlSndngList.FrmEmailId != "NULL" && emlSndngList.ToEmailId != "NULL"))
									this.sendEmail(emlSndngList);
							}
							//emlSndngList.Subject = "MPR Information: " + mprrevisionDetails.DocumentNo + " ; " + "Checker Status: " + mprrevisionDetails.CheckStatus;
							//if (mprrevisionDetails.ApprovedBy != "-" && mprrevisionDetails.ApprovedBy != "")
							//{
							//    emlSndngList.ToEmailId = (db.Employees.Where(li => li.EmployeeNo == mprrevisionDetails.ApprovedBy).FirstOrDefault<Employee>()).EMail;
							//    if (emlSndngList.FrmEmailId != "NULL" && emlSndngList.ToEmailId != "NULL")
							//        this.sendEmail(emlSndngList);
							//}
						}
						if (typeOfUser == "Checker")
						{
							//emlSndngList.FrmEmailId = "Developer@in.yokogawa.com";
							//emlSndngList.ToEmailId = "Developer@in.yokogawa.com";
							emlSndngList.FrmEmailId = (db.Employees.Where(li => li.EmployeeNo == mprrevisionDetails.CheckedBy).FirstOrDefault<Employee>()).EMail;
							emlSndngList.Subject = "MPR Information: " + mprrevisionDetails.DocumentNo + " ; " + "Checker Status: " + mprrevisionDetails.CheckStatus + " ; " + "Approver Status: " + mprrevisionDetails.ApprovalStatus;
							emlSndngList.ToEmailId = (db.Employees.Where(li => li.EmployeeNo == mprrevisionDetails.PreparedBy).FirstOrDefault<Employee>()).EMail;
							if ((!string.IsNullOrEmpty(emlSndngList.FrmEmailId) && !string.IsNullOrEmpty(emlSndngList.FrmEmailId)) && (emlSndngList.FrmEmailId != "NULL" && emlSndngList.ToEmailId != "NULL"))
								this.sendEmail(emlSndngList);
							if (mprrevisionDetails.CheckStatus == "Approved")
							{
								emlSndngList.Subject = "MPR Information: " + mprrevisionDetails.DocumentNo + " ; " + "Checker Status: " + mprrevisionDetails.CheckStatus;
								if (mprrevisionDetails.ApprovedBy != "-" && mprrevisionDetails.ApprovedBy != "")
								{
									emlSndngList.ToEmailId = (db.Employees.Where(li => li.EmployeeNo == mprrevisionDetails.ApprovedBy).FirstOrDefault<Employee>()).EMail;
									if ((!string.IsNullOrEmpty(emlSndngList.FrmEmailId) && !string.IsNullOrEmpty(emlSndngList.FrmEmailId)) && (emlSndngList.FrmEmailId != "NULL" && emlSndngList.ToEmailId != "NULL"))
										this.sendEmail(emlSndngList);
								}
							}
						}
						if (typeOfUser == "Approver")
						{
							emlSndngList.FrmEmailId = (db.Employees.Where(li => li.EmployeeNo == mprrevisionDetails.ApprovedBy).FirstOrDefault<Employee>()).EMail;
							emlSndngList.Subject = "MPR Information: " + mprrevisionDetails.DocumentNo + " ; " + "Checker Status: " + mprrevisionDetails.CheckStatus + " ; " + "Approver Status: " + mprrevisionDetails.ApprovalStatus;
							emlSndngList.ToEmailId = (db.Employees.Where(li => li.EmployeeNo == mprrevisionDetails.PreparedBy).FirstOrDefault<Employee>()).EMail;
							if ((!string.IsNullOrEmpty(emlSndngList.FrmEmailId) && !string.IsNullOrEmpty(emlSndngList.FrmEmailId)) && (emlSndngList.FrmEmailId != "NULL" && emlSndngList.ToEmailId != "NULL"))
								this.sendEmail(emlSndngList);
							emlSndngList.Subject = "MPR Information: " + mprrevisionDetails.DocumentNo + " ; " + "Approver Status: " + mprrevisionDetails.ApprovalStatus;
							emlSndngList.ToEmailId = (db.Employees.Where(li => li.EmployeeNo == mprrevisionDetails.CheckedBy).FirstOrDefault<Employee>()).EMail;
							if ((!string.IsNullOrEmpty(emlSndngList.FrmEmailId) && !string.IsNullOrEmpty(emlSndngList.FrmEmailId)) && (emlSndngList.FrmEmailId != "NULL" && emlSndngList.ToEmailId != "NULL"))
								this.sendEmail(emlSndngList);
							if (mprrevisionDetails.ApprovalStatus == "Approved")
							{
								emlSndngList.Subject = "MPR Information: " + mprrevisionDetails.DocumentNo + " ; " + "Checker Status: " + mprrevisionDetails.CheckStatus + " ; " + "Approver Status: " + mprrevisionDetails.ApprovalStatus;
								if (mprrevisionDetails.SecondApprover != "-" && mprrevisionDetails.SecondApprover != "")
								{
									emlSndngList.ToEmailId = (db.Employees.Where(li => li.EmployeeNo == mprrevisionDetails.SecondApprover).FirstOrDefault<Employee>()).EMail;
									if ((!string.IsNullOrEmpty(emlSndngList.FrmEmailId) && !string.IsNullOrEmpty(emlSndngList.FrmEmailId)) && (emlSndngList.FrmEmailId != "NULL" && emlSndngList.ToEmailId != "NULL"))
										this.sendEmail(emlSndngList);
								}
								else
								{
									emlSndngList.Subject = "New MPR is submitted for acknowledgement - " + mprrevisionDetails.DocumentNo + "";
									var TooEmailId = db.MPRBuyerGroups.Where(li => li.BuyerGroupId == mprrevisionDetails.BuyerGroupId).FirstOrDefault().BuyerManager;
									emlSndngList.ToEmailId = (db.Employees.Where(li => li.EmployeeNo == TooEmailId).FirstOrDefault<Employee>()).EMail;
									if ((!string.IsNullOrEmpty(emlSndngList.FrmEmailId) && !string.IsNullOrEmpty(emlSndngList.FrmEmailId)) && (emlSndngList.FrmEmailId != "NULL" && emlSndngList.ToEmailId != "NULL"))
									{
										List<MPRBGMailConfiguration> configList = db.MPRBGMailConfigurations.Where(li => li.BuyerGroupID == mprrevisionDetails.BuyerGroupId).ToList();
										foreach (var item in configList)
										{
											var mailId = (db.Employees.Where(li => li.EmployeeNo == item.EmployeeNo).FirstOrDefault<Employee>()).EMail;
											if (item.MailType == "BCC" && mailId != null)
											{
												emlSndngList.BCC += mailId + ",";
											}
											if (item.MailType == "CC" && mailId != null)
											{
												emlSndngList.CC += mailId + ",";
											}
											if (item.MailType == "TO" && mailId != null)
											{
												emlSndngList.ToEmailId += mailId + ",";
											}
										}
										this.sendEmail(emlSndngList);
									}
								}
							}
						}
						if (typeOfUser == "SecondApprover")
						{
							emlSndngList.FrmEmailId = (db.Employees.Where(li => li.EmployeeNo == mprrevisionDetails.SecondApprover).FirstOrDefault<Employee>()).EMail;
							emlSndngList.Subject = "MPR Information: " + mprrevisionDetails.DocumentNo + " ; " + "Checker Status: " + mprrevisionDetails.CheckStatus + " ; " + "Approver Status: " + mprrevisionDetails.ApprovalStatus;

							emlSndngList.ToEmailId = (db.Employees.Where(li => li.EmployeeNo == mprrevisionDetails.PreparedBy).FirstOrDefault<Employee>()).EMail;
							if ((!string.IsNullOrEmpty(emlSndngList.FrmEmailId) && !string.IsNullOrEmpty(emlSndngList.FrmEmailId)) && (emlSndngList.FrmEmailId != "NULL" && emlSndngList.ToEmailId != "NULL"))
								this.sendEmail(emlSndngList);

							emlSndngList.Subject = "MPR Information: " + mprrevisionDetails.DocumentNo + " ; " + "Approver Status: " + mprrevisionDetails.ApprovalStatus;
							emlSndngList.ToEmailId = (db.Employees.Where(li => li.EmployeeNo == mprrevisionDetails.CheckedBy).FirstOrDefault<Employee>()).EMail;
							if ((!string.IsNullOrEmpty(emlSndngList.FrmEmailId) && !string.IsNullOrEmpty(emlSndngList.FrmEmailId)) && (emlSndngList.FrmEmailId != "NULL" && emlSndngList.ToEmailId != "NULL"))
								this.sendEmail(emlSndngList);

							emlSndngList.Subject = "MPR Information: " + mprrevisionDetails.DocumentNo + " ; " + "Checker Status: " + mprrevisionDetails.CheckStatus + " ; " + "Approver Status: " + mprrevisionDetails.ApprovalStatus;
							emlSndngList.ToEmailId = (db.Employees.Where(li => li.EmployeeNo == mprrevisionDetails.ApprovedBy).FirstOrDefault<Employee>()).EMail;
							if ((!string.IsNullOrEmpty(emlSndngList.FrmEmailId) && !string.IsNullOrEmpty(emlSndngList.FrmEmailId)) && (emlSndngList.FrmEmailId != "NULL" && emlSndngList.ToEmailId != "NULL"))
								this.sendEmail(emlSndngList);

							if (mprrevisionDetails.SecondApproversStatus == "Approved")
							{
								emlSndngList.Subject = "MPR Information: " + mprrevisionDetails.DocumentNo + " ; " + "Checker Status: " + mprrevisionDetails.CheckStatus + " ; " + "Approver Status: " + mprrevisionDetails.ApprovalStatus + " ; " + "Second Approval Status: " + mprrevisionDetails.SecondApproversStatus;
								if (mprrevisionDetails.ThirdApprover != "-" && mprrevisionDetails.ThirdApprover != "")
								{
									emlSndngList.ToEmailId = (db.Employees.Where(li => li.EmployeeNo == mprrevisionDetails.ThirdApprover).FirstOrDefault<Employee>()).EMail;
									if ((!string.IsNullOrEmpty(emlSndngList.FrmEmailId) && !string.IsNullOrEmpty(emlSndngList.FrmEmailId)) && (emlSndngList.FrmEmailId != "NULL" && emlSndngList.ToEmailId != "NULL"))
										this.sendEmail(emlSndngList);
								}
								else
								{
									emlSndngList.Subject = "New MPR is submitted for acknowledgement - " + mprrevisionDetails.DocumentNo + "";
									var TooEmailId = db.MPRBuyerGroups.Where(li => li.BuyerGroupId == mprrevisionDetails.BuyerGroupId).FirstOrDefault().BuyerManager;
									emlSndngList.ToEmailId = (db.Employees.Where(li => li.EmployeeNo == TooEmailId).FirstOrDefault<Employee>()).EMail;
									if ((!string.IsNullOrEmpty(emlSndngList.FrmEmailId) && !string.IsNullOrEmpty(emlSndngList.FrmEmailId)) && (emlSndngList.FrmEmailId != "NULL" && emlSndngList.ToEmailId != "NULL"))
									{
										List<MPRBGMailConfiguration> configList = db.MPRBGMailConfigurations.Where(li => li.BuyerGroupID == mprrevisionDetails.BuyerGroupId).ToList();
										foreach (var item in configList)
										{
											var mailId = (db.Employees.Where(li => li.EmployeeNo == item.EmployeeNo).FirstOrDefault<Employee>()).EMail;
											if (item.MailType == "BCC" && mailId != null)
											{
												emlSndngList.BCC += mailId + ",";
											}
											if (item.MailType == "CC" && mailId != null)
											{
												emlSndngList.CC += mailId + ",";
											}
											if (item.MailType == "TO" && mailId != null)
											{
												emlSndngList.ToEmailId += "," + mailId;
											}
										}
										this.sendEmail(emlSndngList);
									}
								}
							}
						}
						if (typeOfUser == "ThirdApprover")
						{
							emlSndngList.FrmEmailId = (db.Employees.Where(li => li.EmployeeNo == mprrevisionDetails.ThirdApprover).FirstOrDefault<Employee>()).EMail;
							emlSndngList.Subject = "MPR Information: " + mprrevisionDetails.DocumentNo + " ; " + "Checker Status: " + mprrevisionDetails.CheckStatus + " ; " + "Approver Status: " + mprrevisionDetails.ApprovalStatus;
							var TooEmailId = db.MPRBuyerGroups.Where(li => li.BuyerGroupId == mprrevisionDetails.BuyerGroupId).FirstOrDefault().BuyerManager;
							emlSndngList.ToEmailId = (db.Employees.Where(li => li.EmployeeNo == TooEmailId).FirstOrDefault<Employee>()).EMail;
							if ((!string.IsNullOrEmpty(emlSndngList.FrmEmailId) && !string.IsNullOrEmpty(emlSndngList.FrmEmailId)) && (emlSndngList.FrmEmailId != "NULL" && emlSndngList.ToEmailId != "NULL"))
								this.sendEmail(emlSndngList);

							emlSndngList.Subject = "MPR Information: " + mprrevisionDetails.DocumentNo + " ; " + "Approver Status: " + mprrevisionDetails.ApprovalStatus;
							emlSndngList.ToEmailId = (db.Employees.Where(li => li.EmployeeNo == mprrevisionDetails.CheckedBy).FirstOrDefault<Employee>()).EMail;
							if ((!string.IsNullOrEmpty(emlSndngList.FrmEmailId) && !string.IsNullOrEmpty(emlSndngList.FrmEmailId)) && (emlSndngList.FrmEmailId != "NULL" && emlSndngList.ToEmailId != "NULL"))
								this.sendEmail(emlSndngList);

							emlSndngList.Subject = "MPR Information: " + mprrevisionDetails.DocumentNo + " ; " + "Checker Status: " + mprrevisionDetails.CheckStatus + " ; " + "Approver Status: " + mprrevisionDetails.ApprovalStatus;
							emlSndngList.ToEmailId = (db.Employees.Where(li => li.EmployeeNo == mprrevisionDetails.ApprovedBy).FirstOrDefault<Employee>()).EMail;
							if ((!string.IsNullOrEmpty(emlSndngList.FrmEmailId) && !string.IsNullOrEmpty(emlSndngList.FrmEmailId)) && (emlSndngList.FrmEmailId != "NULL" && emlSndngList.ToEmailId != "NULL"))
								this.sendEmail(emlSndngList);


							emlSndngList.Subject = "MPR Information: " + mprrevisionDetails.DocumentNo + " ; " + "Checker Status: " + mprrevisionDetails.CheckStatus + " ; " + "Approver Status: " + mprrevisionDetails.ApprovalStatus + " ; " + "Second Approval Status: " + mprrevisionDetails.SecondApproversStatus;
							emlSndngList.ToEmailId = (db.Employees.Where(li => li.EmployeeNo == mprrevisionDetails.SecondApprover).FirstOrDefault<Employee>()).EMail;
							if ((!string.IsNullOrEmpty(emlSndngList.FrmEmailId) && !string.IsNullOrEmpty(emlSndngList.FrmEmailId)) && (emlSndngList.FrmEmailId != "NULL" && emlSndngList.ToEmailId != "NULL"))
								this.sendEmail(emlSndngList);
							if (mprrevisionDetails.ThirdApproverStatus == "Approved")
							{
								emlSndngList.Subject = "New MPR is submitted for acknowledgement - " + mprrevisionDetails.DocumentNo + "";
								emlSndngList.ToEmailId = db.MPRBuyerGroups.Where(li => li.BuyerGroupId == mprrevisionDetails.BuyerGroupId).FirstOrDefault().BuyerManager;
								if ((!string.IsNullOrEmpty(emlSndngList.FrmEmailId) && !string.IsNullOrEmpty(emlSndngList.FrmEmailId)) && (emlSndngList.FrmEmailId != "NULL" && emlSndngList.ToEmailId != "NULL"))
								{
									List<MPRBGMailConfiguration> configList = db.MPRBGMailConfigurations.Where(li => li.BuyerGroupID == mprrevisionDetails.BuyerGroupId).ToList();
									foreach (var item in configList)
									{
										var mailId = (db.Employees.Where(li => li.EmployeeNo == item.EmployeeNo).FirstOrDefault<Employee>()).EMail;
										if (item.MailType == "BCC" && mailId != null)
										{
											emlSndngList.BCC += mailId + ",";
										}
										if (item.MailType == "CC" && mailId != null)
										{
											emlSndngList.CC += mailId + ",";
										}
										if (item.MailType == "TO" && mailId != null)
										{
											emlSndngList.ToEmailId += mailId + ",";
										}
									}
									this.sendEmail(emlSndngList);
								}
							}
						}
					}

				}
			}
			catch (Exception ex)
			{
				log.ErrorMessage("EmailTemplate", "prepareMPREmailTemplate", ex.Message + "; " + ex.StackTrace.ToString());
				//throw ex;
			}
			return true;

		}

		/*Name of Function : <<prepareRFQGeneratedEmail>>  Author :<<Prasanna>>  
		  Date of Creation <<01-12-2019>>
		  Purpose : <<Emial template when rfq generated>>
		  Review Date :<<>>   Reviewed By :<<>>*/
		public bool prepareRFQGeneratedEmail(string FrmEmailId, int VendorId)
		{
			try
			{
				using (var db = new YSCMEntities()) //ok
				{
					var vscm = new VSCMEntities();
					var vendor = vscm.RemoteVendorUserMasters.Where(li => li.VendorId == VendorId).FirstOrDefault();
					if (vendor != null)
					{
						var ipaddress = ConfigurationManager.AppSettings["UI_vendor_IpAddress"];
						EmailSend emlSndngList = new EmailSend();
						emlSndngList.Subject = "New RFQ Generated From YOKOGAWA";
						emlSndngList.Body = "<html><meta charset=\"ISO-8859-1\"><head><link rel = 'stylesheet' href = 'https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css' ></head><body><div class='container'><div>Dear Vendor, </div><br/><div>You have received new RFQ from Yokogawa</div><br/>The required portal details and the password is given below : <br /><br /> <b  style='color:#40bfbf;'>Click Here to Redirect : <a href='" + ipaddress + "'>" + ipaddress + "</a></b><br /> <br /> <b style='color:#40bfbf;'>Instruction: </b> Open the link with GOOGLE CHROME <br /> <b style='color:#40bfbf;'>User Name:</b> " + vendor.Vuserid + " <br /><b style='color:#40bfbf;'>Pass word:</b> " + vendor.pwd + "<br /><br/><div>Regards,<br/><div>CMM Department</div></body></html>";
						var mailData = (db.Employees.Where(li => li.EmployeeNo == FrmEmailId).FirstOrDefault<Employee>());
						if (mailData != null)
							emlSndngList.FrmEmailId = mailData.EMail;
						//emlSndngList.ToEmailId = "Developer@in.yokogawa.com";
						string emails = (db.VendorMasters.Where(li => li.Vendorid == VendorId).FirstOrDefault<VendorMaster>()).Emailid;
						List<string> emailList = emails.Split(',').ToList();
						foreach (var item in emailList)
						{
							emlSndngList.ToEmailId = item;
							if ((!string.IsNullOrEmpty(emlSndngList.FrmEmailId) && !string.IsNullOrEmpty(emlSndngList.FrmEmailId)) && (emlSndngList.FrmEmailId != "NULL" && emlSndngList.ToEmailId != "NULL"))
								this.sendEmail(emlSndngList);
						}
					}

				}
			}
			catch (Exception ex)
			{
				log.ErrorMessage("EmailTemplate", "prepareRFQGeneratedEmail", ex.Message + "; " + ex.StackTrace.ToString());
				//throw ex;
			}
			return true;
		}

		/*Name of Function : <<prepareMPRStatusEmail>>  Author :<<Prasanna>>  
		  Date of Creation <<01-12-2019>>
		  Purpose : <<Emial template when buyer group changed,mpr assigned>>
		  Review Date :<<>>   Reviewed By :<<>>*/
		public bool prepareMPRStatusEmail(string FrmEmailId, string ToEmailId, string type, int revisionid)
		{
			try
			{
				using (var db = new YSCMEntities()) //ok
				{
					var ipaddress = ConfigurationManager.AppSettings["UI_IpAddress"];
					ipaddress = ipaddress + "SCM/MPRForm/" + revisionid + "";
					MPRRevisionDetail mprrevisionDetails = db.MPRRevisionDetails.Where(li => li.RevisionId == revisionid).FirstOrDefault<MPRRevisionDetail>();
					EmailSend emlSndngList = new EmailSend();
					string bodyTxt = "";
					if (type == "mprAssign")
					{
						emlSndngList.Subject = "MPR Assigned";
						bodyTxt = "MPR No:" + mprrevisionDetails.DocumentNo + " is assigned to you";
					}
					else
					{
						emlSndngList.Subject = "Buyer Group Changed";
						bodyTxt = "<b>MPR No: </b>" + mprrevisionDetails.DocumentNo + " buyer group is changed to " + mprrevisionDetails.BuyerGroupName + "";
					}

					emlSndngList.Body = "<html><meta charset=\"ISO-8859-1\"><head><link rel = 'stylesheet' href = 'https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css' ></head><body><div class='container'><span>" + bodyTxt + "</span><br/><br/><b>Click here to redirect : </b>&nbsp<a href='" + ipaddress + "'>" + ipaddress + "</a></div></body></html>";
					emlSndngList.FrmEmailId = (db.Employees.Where(li => li.EmployeeNo == FrmEmailId).FirstOrDefault<Employee>()).EMail;
					//emlSndngList.ToEmailId = "Developer@in.yokogawa.com";
					emlSndngList.ToEmailId = (db.Employees.Where(li => li.EmployeeNo == ToEmailId).FirstOrDefault<Employee>()).EMail;
					if ((!string.IsNullOrEmpty(emlSndngList.FrmEmailId) && !string.IsNullOrEmpty(emlSndngList.FrmEmailId)) && (emlSndngList.FrmEmailId != "NULL" && emlSndngList.ToEmailId != "NULL"))
						this.sendEmail(emlSndngList);

				}
			}
			catch (Exception ex)
			{
				log.ErrorMessage("EmailTemplate", "prepareMPRStatusEmail", ex.Message + "; " + ex.StackTrace.ToString());
				//throw ex;
			}
			return true;
		}

		/*Name of Function : <<sendMailtoVendor>>  Author :<<Prasanna>>  
		  Date of Creation <<01-12-2019>>
		  Purpose : <<Emial template for vendor when RFQ Generated>>
		  Review Date :<<>>   Reviewed By :<<>>*/
		public bool sendMailtoVendor(sendMailObj mailObj)
		{
			try
			{
				using (var db = new YSCMEntities()) //ok
				{
					var vscm = new VSCMEntities();
					var ipaddress = ConfigurationManager.AppSettings["UI_vendor_IpAddress"];
					var fromEmail = ConfigurationManager.AppSettings["fromEmailTovendor"];
					EmailSend emlSndngList = new EmailSend();
					emlSndngList.Subject = "Message From Yokogawa";
					emlSndngList.FrmEmailId = fromEmail;
					//emlSndngList.ToEmailId = "Developer@in.yokogawa.com;";
					List<RemoteVendorUserMaster> userlist = vscm.RemoteVendorUserMasters.ToList();
					foreach (var item in userlist)
					{

						string mailbody = mailObj.Message;
						emlSndngList.ToEmailId = item.Vuserid;
						if (mailObj.IncludeUrl)
						{
							string url = "The required portal details and the password is given below : <br /><br /> <b  style='color:#40bfbf;'>Click Here to Redirect: <a href='" + ipaddress + "'>" + ipaddress + "</a></b><br />";
							mailbody = mailbody.Replace("The required portal details and the password is given below : <br />", url);
						}
						if (mailObj.IncludeCredentials)
						{
							string credentials = "The required portal details and the password is given below : <br /> <br /> <b style='color:#40bfbf;'>User Name:</b> " + item.Vuserid + " <br /><b style='color:#40bfbf;'>Pass word:</b> " + item.pwd + " <br />";
							mailbody = mailbody.Replace("The required portal details and the password is given below : <br />", credentials);
						}

						emlSndngList.Body = "<html><head></head><body><div>" + mailbody + "</div></body></html>";
						if ((!string.IsNullOrEmpty(emlSndngList.FrmEmailId) && !string.IsNullOrEmpty(emlSndngList.FrmEmailId)) && (emlSndngList.FrmEmailId != "NULL" && emlSndngList.ToEmailId != "NULL"))
							this.sendEmail(emlSndngList);

					}

				}
			}
			catch (Exception ex)
			{
				log.ErrorMessage("EmailTemplate", "sendMailtoVendor", ex.Message + "; " + ex.StackTrace.ToString());
				//throw ex;
			}
			return true;

		}


		public bool mailtoRequestor(int revisionId, string FrmEmailId)
		{
			try
			{
				using (var db = new YSCMEntities()) //ok
				{

					var ipaddress = ConfigurationManager.AppSettings["UI_IpAddress"];
					MPRRevisionDetail mprrevisionDetails = db.MPRRevisionDetails.Where(li => li.RevisionId == revisionId).FirstOrDefault<MPRRevisionDetail>();
					ipaddress = ipaddress + "SCM/MPRForm/" + mprrevisionDetails.RevisionId + "";
					var issueOfPurpose = mprrevisionDetails.IssuePurposeId == 1 ? "For Enquiry" : "For Issuing PO";
					EmailSend emlSndngList = new EmailSend();
					Employee frmEmail = db.Employees.Where(li => li.EmployeeNo == FrmEmailId).FirstOrDefault<Employee>();
					emlSndngList.FrmEmailId = frmEmail.EMail;
					emlSndngList.Subject = "MPR Information: " + mprrevisionDetails.DocumentNo + " ; " + "Client Name: " + mprrevisionDetails.ClientName + " ; " + "Current Status: " + mprrevisionDetails.MPRStatus;
					emlSndngList.Body = "<html><meta charset=\"ISO-8859-1\"><head><link rel = 'stylesheet' href = 'https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css' ></head><body><div class='container'><table border='1' class='table table-bordered table-sm'><tr><td><b>MPR Number</b></td><td>" + mprrevisionDetails.DocumentNo + "</td><td><b>Document Description</b></td><td>" + mprrevisionDetails.DocumentDescription + "</td><td><b>Purpose of Iussing MPR</b></td><td>" + issueOfPurpose + "</td></tr><tr><td><b>Department</b></td><td>" + mprrevisionDetails.DepartmentName + "</td><td><b>Project Manager</td></td><td>" + mprrevisionDetails.ProjectManagerName + "</td><td><b>Job Code</b></td><td>" + mprrevisionDetails.JobCode + "</td></tr><tr><td><b>Client Name</b></td><td>" + mprrevisionDetails.ClientName + "</td><td><b>Job Name</b></td><td>" + mprrevisionDetails.JobName + "</td><td><b>Buyer Group</b></td><td>" + mprrevisionDetails.BuyerGroupName + "</td></tr><tr><td><b>Checker Name</b></td><td>" + mprrevisionDetails.CheckedName + "</td><td><b>Checker Status</b></td><td>" + mprrevisionDetails.CheckStatus + "</td><td><b>Checker Remarks</b></td><td>" + mprrevisionDetails.CheckerRemarks + "</td></tr><tr><td><b>Approver Name</b></td><td>" + mprrevisionDetails.ApproverName + "</td><td><b>Approver Status</b></td><td>" + mprrevisionDetails.ApprovalStatus + "</td><td><b>Approver Remarks</b></td><td>" + mprrevisionDetails.ApproverRemarks + "</td></tr></table><br/><br/><b>Click here to redirect : </b>&nbsp<a href='" + ipaddress + "'>" + ipaddress + "</a></div></body></html>";
					emlSndngList.ToEmailId = (db.Employees.Where(li => li.EmployeeNo == mprrevisionDetails.PreparedBy).FirstOrDefault<Employee>()).EMail;
					if (mprrevisionDetails.CheckedBy != "-" && mprrevisionDetails.CheckedBy != "")
					{
						emlSndngList.CC = (db.Employees.Where(li => li.EmployeeNo == mprrevisionDetails.CheckedBy).FirstOrDefault<Employee>()).EMail;
					}
					if ((!string.IsNullOrEmpty(emlSndngList.FrmEmailId) && !string.IsNullOrEmpty(emlSndngList.FrmEmailId)) && (emlSndngList.FrmEmailId != "NULL" && emlSndngList.ToEmailId != "NULL"))
						this.sendEmail(emlSndngList);
				}
			}
			catch (Exception ex)
			{
				log.ErrorMessage("EmailTemplate", "mailtoRequestor", ex.Message + "; " + ex.StackTrace.ToString());
				//throw ex;
			}
			return true;
		}

		public bool prepareAribaTemplate(int tokuchureqId, string FrmEmailId, string ToMailId, string typeOfUser, int revisionId)
		{
			try
			{
				using (var db = new YSCMEntities()) //ok
				{

					var UI_Ipaddress = ConfigurationManager.AppSettings["UI_IpAddress"];
					MPRRevisionDetail mprrevisionDetails = db.MPRRevisionDetails.Where(li => li.RevisionId == revisionId).FirstOrDefault<MPRRevisionDetail>();
					string ipaddress = UI_Ipaddress + "SCM/MPRForm/" + mprrevisionDetails.RevisionId + "";
					string tkaddress = UI_Ipaddress + "SCM/TokochuRequest/" + tokuchureqId + "";
					var issueOfPurpose = mprrevisionDetails.IssuePurposeId == 1 ? "For Enquiry" : "For Issuing PO";
					EmailSend emlSndngList = new EmailSend();
					Employee frmEmail = db.Employees.Where(li => li.EmployeeNo == FrmEmailId).FirstOrDefault<Employee>();
					emlSndngList.FrmEmailId = frmEmail.EMail;
					emlSndngList.ToEmailId = (db.Employees.Where(li => li.EmployeeNo == ToMailId).FirstOrDefault<Employee>()).EMail;
					if (typeOfUser == "Verifier" || typeOfUser == "PreVerifier")
					{
						//send mail to requestor
						emlSndngList.Subject = "Tokuchu Request Verfied - " + mprrevisionDetails.DocumentNo + "";
						emlSndngList.Body = "<html><meta charset=\"ISO-8859-1\"><head><link rel = 'stylesheet' href = 'https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css' ></head><body><div class='container'><div>Tokuchu request data has been verified.</div><br/><div><b>Tokuchu URL: </b>&nbsp<a href='" + tkaddress + "'>" + tkaddress + "</a></div></div><br/><table border='1' class='table table-bordered table-sm'><tr><td><b>MPR Number</b></td><td>" + mprrevisionDetails.DocumentNo + "</td><td><b>Document Description</b></td><td>" + mprrevisionDetails.DocumentDescription + "</td><td><b>Purpose of Iussing MPR</b></td><td>" + issueOfPurpose + "</td></tr><tr><td><b>Department</b></td><td>" + mprrevisionDetails.DepartmentName + "</td><td><b>Project Manager</td></td><td>" + mprrevisionDetails.ProjectManagerName + "</td><td><b>Job Code</b></td><td>" + mprrevisionDetails.JobCode + "</td></tr><tr><td><b>Client Name</b></td><td>" + mprrevisionDetails.ClientName + "</td><td><b>Job Name</b></td><td>" + mprrevisionDetails.JobName + "</td><td><b>Buyer Group</b></td><td>" + mprrevisionDetails.BuyerGroupName + "</td></tr><tr><td><b>Checker Name</b></td><td>" + mprrevisionDetails.CheckedName + "</td><td><b>Checker Status</b></td><td>" + mprrevisionDetails.CheckStatus + "</td><td><b>Checker Remarks</b></td><td>" + mprrevisionDetails.CheckerRemarks + "</td></tr><tr><td><b>Approver Name</b></td><td>" + mprrevisionDetails.ApproverName + "</td><td><b>Approver Status</b></td><td>" + mprrevisionDetails.ApprovalStatus + "</td><td><b>Approver Remarks</b></td><td>" + mprrevisionDetails.ApproverRemarks + "</td></tr></table><br/><br/><b>Click here to redirect : </b>&nbsp<a href='" + ipaddress + "'>" + ipaddress + "</a></div></body></html>";

						if ((!string.IsNullOrEmpty(emlSndngList.FrmEmailId) && !string.IsNullOrEmpty(emlSndngList.FrmEmailId)) && (emlSndngList.FrmEmailId != "NULL" && emlSndngList.ToEmailId != "NULL"))
							this.sendEmail(emlSndngList);

						if (typeOfUser == "PreVerifier")
						{
							//send mail to ariba
							emlSndngList.Subject = "Tokuchu request for ariba approval";
							emlSndngList.Body = "<html><head></head><body><div>Tokuchu request for ariba approval</div><br/><div>Tokuchu request data has been verified.</div><br/><div><b>Tokuchu URL: </b>&nbsp<a href='" + tkaddress + "'>" + tkaddress + "</a></div></body></html>";
							emlSndngList.ToEmailId = ConfigurationManager.AppSettings["AribaMailId"];
							if ((!string.IsNullOrEmpty(emlSndngList.FrmEmailId) && !string.IsNullOrEmpty(emlSndngList.FrmEmailId)) && (emlSndngList.FrmEmailId != "NULL" && emlSndngList.ToEmailId != "NULL"))
								this.sendEmail(emlSndngList);
						}
					}
					if (typeOfUser == "Requestor")
					{
						//send mail to verifier
						emlSndngList.Subject = "Tokuchu request for  verification- " + mprrevisionDetails.DocumentNo + "";
						emlSndngList.Body = "<html><meta charset=\"ISO-8859-1\"><head><link rel = 'stylesheet' href = 'https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css' ></head><body><div class='container'><div>Please verify the data for Ariba.</div><br/><div><b>Tokuchu URL: </b>&nbsp<a href='" + tkaddress + "'>" + tkaddress + "</a></div></div><br/><table border='1' class='table table-bordered table-sm'><tr><td><b>MPR Number</b></td><td>" + mprrevisionDetails.DocumentNo + "</td><td><b>Document Description</b></td><td>" + mprrevisionDetails.DocumentDescription + "</td><td><b>Purpose of Iussing MPR</b></td><td>" + issueOfPurpose + "</td></tr><tr><td><b>Department</b></td><td>" + mprrevisionDetails.DepartmentName + "</td><td><b>Project Manager</td></td><td>" + mprrevisionDetails.ProjectManagerName + "</td><td><b>Job Code</b></td><td>" + mprrevisionDetails.JobCode + "</td></tr><tr><td><b>Client Name</b></td><td>" + mprrevisionDetails.ClientName + "</td><td><b>Job Name</b></td><td>" + mprrevisionDetails.JobName + "</td><td><b>Buyer Group</b></td><td>" + mprrevisionDetails.BuyerGroupName + "</td></tr><tr><td><b>Checker Name</b></td><td>" + mprrevisionDetails.CheckedName + "</td><td><b>Checker Status</b></td><td>" + mprrevisionDetails.CheckStatus + "</td><td><b>Checker Remarks</b></td><td>" + mprrevisionDetails.CheckerRemarks + "</td></tr><tr><td><b>Approver Name</b></td><td>" + mprrevisionDetails.ApproverName + "</td><td><b>Approver Status</b></td><td>" + mprrevisionDetails.ApprovalStatus + "</td><td><b>Approver Remarks</b></td><td>" + mprrevisionDetails.ApproverRemarks + "</td></tr></table><br/><br/><b>Click here to redirect : </b>&nbsp<a href='" + ipaddress + "'>" + ipaddress + "</a></div></body></html>";
						if ((!string.IsNullOrEmpty(emlSndngList.FrmEmailId) && !string.IsNullOrEmpty(emlSndngList.FrmEmailId)) && (emlSndngList.FrmEmailId != "NULL" && emlSndngList.ToEmailId != "NULL"))
							this.sendEmail(emlSndngList);
					}
				}
			}
			catch (Exception ex)
			{
				log.ErrorMessage("EmailTemplate", "prepareAribaTemplate", ex.Message + "; " + ex.StackTrace.ToString());
				//throw ex;
			}
			return true;
		}


		public bool prepareVendRegTemplate(string typeOfUser, int VendorId)
		{
			try
			{
				var vscm = new VSCMEntities();

				using (var db = new YSCMEntities()) //ok
				{
					VendorRegApprovalProcess vendorProcessDetails = db.VendorRegApprovalProcesses.Where(li => li.Vendorid == VendorId).FirstOrDefault();
					var Vendoripaddress = ConfigurationManager.AppSettings["UI_vendor_IpAddress"];
					var Scmipaddress = ConfigurationManager.AppSettings["UI_IpAddress"];
					Scmipaddress = Scmipaddress + "SCM/VendorRegInitiate/" + VendorId + "";
					EmailSend emlSndngList = new EmailSend();

					if (typeOfUser == "Buyer")
					{
						emlSndngList.FrmEmailId = (db.Employees.Where(li => li.EmployeeNo == vendorProcessDetails.IntiatedBy).FirstOrDefault<Employee>()).EMail;
						var vendor = vscm.RemoteVendorUserMasters.Where(li => li.VendorId == VendorId).FirstOrDefault();
						if (vendor != null)
						{
							emlSndngList.Subject = "Registration Initiated";
							emlSndngList.ToEmailId = vendorProcessDetails.VendorEmailId;
							emlSndngList.Body = "<html><meta charset=\"ISO-8859-1\"><head><link rel = 'stylesheet' href = 'https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css' ></head><body><div class='container'><div>Dear Vendor, </div><br/><div>You have initiated  registration from Yokogawa</div><br/>The required portal details and the password is given below : <br /><br /> <b  style='color:#40bfbf;'>Click Here to Redirect : <a href='" + Vendoripaddress + "'>" + Vendoripaddress + "</a></b><br /> <br /> <b style='color:#40bfbf;'>Instruction: </b> Open the link with GOOGLE CHROME <br /> <b style='color:#40bfbf;'>User Name:</b> " + vendor.Vuserid + " <br /><b style='color:#40bfbf;'>Pass word:</b> " + vendor.pwd + "<br /><br/><div>Regards,<br/><div>CMM Department</div></body></html>";
							if ((!string.IsNullOrEmpty(emlSndngList.FrmEmailId) && !string.IsNullOrEmpty(emlSndngList.FrmEmailId)) && (emlSndngList.FrmEmailId != "NULL" && emlSndngList.ToEmailId != "NULL"))
								this.sendEmail(emlSndngList);
						}

					}
					if (typeOfUser == "Checker")
					{
						//if (vendorProcessDetails.CheckerStatus == "Approved")
						//{
						emlSndngList.FrmEmailId = (db.Employees.Where(li => li.EmployeeNo == vendorProcessDetails.CheckedBy).FirstOrDefault<Employee>()).EMail;
						//mail to Approver
						if (!string.IsNullOrEmpty(vendorProcessDetails.ApprovedBy))
						{
							emlSndngList.Subject = "Vendor Registration: " + vendorProcessDetails.Vendorid + " ; " + "Checker Status: " + vendorProcessDetails.CheckerStatus;
							Employee toemail = db.Employees.Where(li => li.EmployeeNo == vendorProcessDetails.ApprovedBy).FirstOrDefault<Employee>();
							emlSndngList.ToEmailId = toemail.EMail;
							emlSndngList.Body = "<html><meta charset=\"ISO-8859-1\"><head><link rel = 'stylesheet' href = 'https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css' ></head><body><div class='container'> <b  style='color:#40bfbf;'>Click Here to Redirect : <a href='" + Scmipaddress + "'>" + Scmipaddress + "</a></b></div></body></html>";
							if ((!string.IsNullOrEmpty(emlSndngList.FrmEmailId) && !string.IsNullOrEmpty(emlSndngList.FrmEmailId)) && (emlSndngList.FrmEmailId != "NULL" && emlSndngList.ToEmailId != "NULL"))
								this.sendEmail(emlSndngList);
						}
						//}
					}
					if (typeOfUser == "Approver")
					{
						emlSndngList.FrmEmailId = (db.Employees.Where(li => li.EmployeeNo == vendorProcessDetails.ApprovedBy).FirstOrDefault<Employee>()).EMail;

						//mail to checker
						if (!string.IsNullOrEmpty(vendorProcessDetails.CheckedBy))
						{
							emlSndngList.Subject = "Vendor Registration: " + vendorProcessDetails.Vendorid + " ; " + "Checker Status: " + vendorProcessDetails.CheckerStatus + " ; " + "Approver Status: " + vendorProcessDetails.ApprovalStatus;
							Employee toemail = db.Employees.Where(li => li.EmployeeNo == vendorProcessDetails.CheckedBy).FirstOrDefault<Employee>();
							emlSndngList.ToEmailId = toemail.EMail;
							emlSndngList.Body = "<html><meta charset=\"ISO-8859-1\"><head><link rel = 'stylesheet' href = 'https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css' ></head><body><div class='container'> <b  style='color:#40bfbf;'>Click Here to Redirect : <a href='" + Scmipaddress + "'>" + Scmipaddress + "</a></b></div></body></html>";
							if ((!string.IsNullOrEmpty(emlSndngList.FrmEmailId) && !string.IsNullOrEmpty(emlSndngList.FrmEmailId)) && (emlSndngList.FrmEmailId != "NULL" && emlSndngList.ToEmailId != "NULL"))
								this.sendEmail(emlSndngList);
						}
						//mail to second approver
						if (vendorProcessDetails.ApprovalStatus == "Approved")
						{
							if (!string.IsNullOrEmpty(vendorProcessDetails.Verifier1))
							{
								emlSndngList.Subject = "Vendor Registration: " + vendorProcessDetails.Vendorid + " ; " + "Checker Status: " + vendorProcessDetails.CheckerStatus + " ; " + "Approver Status: " + vendorProcessDetails.ApprovalStatus;
								Employee toemail = db.Employees.Where(li => li.EmployeeNo == vendorProcessDetails.Verifier1).FirstOrDefault<Employee>();
								emlSndngList.ToEmailId = toemail.EMail;
								emlSndngList.Body = "<html><meta charset=\"ISO-8859-1\"><head><link rel = 'stylesheet' href = 'https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css' ></head><body><div class='container'> <b  style='color:#40bfbf;'>Click Here to Redirect : <a href='" + Scmipaddress + "'>" + Scmipaddress + "</a></b></div></body></html>";
								if ((!string.IsNullOrEmpty(emlSndngList.FrmEmailId) && !string.IsNullOrEmpty(emlSndngList.FrmEmailId)) && (emlSndngList.FrmEmailId != "NULL" && emlSndngList.ToEmailId != "NULL"))
									this.sendEmail(emlSndngList);
								Employee toemail2 = db.Employees.Where(li => li.EmployeeNo == vendorProcessDetails.Verifier2).FirstOrDefault<Employee>();
								emlSndngList.ToEmailId = toemail2.EMail;
								if ((!string.IsNullOrEmpty(emlSndngList.FrmEmailId) && !string.IsNullOrEmpty(emlSndngList.FrmEmailId)) && (emlSndngList.FrmEmailId != "NULL" && emlSndngList.ToEmailId != "NULL"))
									this.sendEmail(emlSndngList);
							}
						}
					}
					if (typeOfUser == "Verfier")
					{
						emlSndngList.FrmEmailId = (db.Employees.Where(li => li.EmployeeNo == vendorProcessDetails.VerifiedBy).FirstOrDefault<Employee>()).EMail;

						//mail to checker
						if (!string.IsNullOrEmpty(vendorProcessDetails.CheckedBy))
						{
							emlSndngList.Subject = "Vendor Registration: " + vendorProcessDetails.Vendorid + " ; " + "Checker Status: " + vendorProcessDetails.CheckerStatus + " ; " + "Approver Status: " + vendorProcessDetails.ApprovalStatus;
							Employee toemail = db.Employees.Where(li => li.EmployeeNo == vendorProcessDetails.CheckedBy).FirstOrDefault<Employee>();
							emlSndngList.ToEmailId = toemail.EMail;
							emlSndngList.Body = "<html><meta charset=\"ISO-8859-1\"><head><link rel = 'stylesheet' href = 'https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css' ></head><body><div class='container'> <b  style='color:#40bfbf;'>Click Here to Redirect : <a href='" + Scmipaddress + "'>" + Scmipaddress + "</a></b></div></body></html>";
							if ((!string.IsNullOrEmpty(emlSndngList.FrmEmailId) && !string.IsNullOrEmpty(emlSndngList.FrmEmailId)) && (emlSndngList.FrmEmailId != "NULL" && emlSndngList.ToEmailId != "NULL"))
								this.sendEmail(emlSndngList);
						}
						//mail to Approver
						if (!string.IsNullOrEmpty(vendorProcessDetails.ApprovedBy))
						{
							emlSndngList.Subject = "Vendor Registration: " + vendorProcessDetails.Vendorid + " ; " + "Checker Status: " + vendorProcessDetails.CheckerStatus + " ; " + "Approver Status: " + vendorProcessDetails.ApprovalStatus + " ; " + "Second Approval Status: " + vendorProcessDetails.VerifiedStatus;
							Employee toemail = db.Employees.Where(li => li.EmployeeNo == vendorProcessDetails.ApprovedBy).FirstOrDefault<Employee>();
							emlSndngList.ToEmailId = toemail.EMail;
							emlSndngList.Body = "<html><meta charset=\"ISO-8859-1\"><head><link rel = 'stylesheet' href = 'https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css' ></head><body><div class='container'> <b  style='color:#40bfbf;'>Click Here to Redirect : <a href='" + Scmipaddress + "'>" + Scmipaddress + "</a></b></div></body></html>";
							if ((!string.IsNullOrEmpty(emlSndngList.FrmEmailId) && !string.IsNullOrEmpty(emlSndngList.FrmEmailId)) && (emlSndngList.FrmEmailId != "NULL" && emlSndngList.ToEmailId != "NULL"))
								this.sendEmail(emlSndngList);
						}

					}

				}
			}
			catch (Exception ex)
			{
				log.ErrorMessage("EmailTemplate", "mailtoRequestor", ex.Message + "; " + ex.StackTrace.ToString());
				//throw ex;
			}
			return true;
		}

		/*Name of Function : <<sendEmail>>  Author :<<Prasanna>>  
		  Date of Creation <<01-12-2019>>
		  Purpose : <<Sending mail method>>
		  Review Date :<<>>   Reviewed By :<<>>*/
		public bool sendEmail(EmailSend emlSndngList)
		{
			bool validEmail = IsValidEmail(emlSndngList.ToEmailId);
			if (!string.IsNullOrEmpty(emlSndngList.ToEmailId) && !string.IsNullOrEmpty(emlSndngList.FrmEmailId) && validEmail)
			{
				var BCC = ConfigurationManager.AppSettings["BCC"];
				var SMTPServer = ConfigurationManager.AppSettings["SMTPServer"];
				MailMessage mailMessage = new MailMessage();
				mailMessage.From = new MailAddress(emlSndngList.FrmEmailId); //From Email Id
				string[] ToMuliId = emlSndngList.ToEmailId.Split(',');
				foreach (string ToEMailId in ToMuliId)
				{
					mailMessage.To.Add(new MailAddress(ToEMailId)); //adding multiple TO Email Id
				}
				SmtpClient client = new SmtpClient();
				if (!string.IsNullOrEmpty(emlSndngList.Subject))
					mailMessage.Subject = emlSndngList.Subject;

				if (!string.IsNullOrEmpty(emlSndngList.CC))
				{
					string[] CCId = emlSndngList.CC.Split(',');

					foreach (string CCEmail in CCId)
					{
						mailMessage.CC.Add(new MailAddress(CCEmail)); //Adding Multiple CC email Id
					}
				}

				if (!string.IsNullOrEmpty(emlSndngList.BCC))
				{
					string[] bccid = emlSndngList.BCC.Split(',');


					foreach (string bccEmailId in bccid)
					{
						mailMessage.Bcc.Add(new MailAddress(bccEmailId)); //Adding Multiple BCC email Id
					}
				}

				if (!string.IsNullOrEmpty(BCC))
					mailMessage.Bcc.Add(new MailAddress(BCC));
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
		}


		/*Name of Function : <<IsValidEmail>>  Author :<<Prasanna>>  
		  Date of Creation <<01-12-2019>>
		  Purpose : <<validate mail>>
		  Review Date :<<>>   Reviewed By :<<>>*/
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

	/*Name of Class : <<EmailSend>>  Author :<<Prasanna>>  
	  Date of Creation <<01-12-2019>>
	  Purpose : <<to send email>>
	  Review Date :<<>>   Reviewed By :<<>>*/
	public class EmailSend
	{
		public string FrmEmailId { get; set; }
		public string ToEmailId { get; set; }
		public string CC { get; set; }
		public string BCC { get; set; }
		public string Subject { get; set; }
		public string Body { get; set; }
	}
}
