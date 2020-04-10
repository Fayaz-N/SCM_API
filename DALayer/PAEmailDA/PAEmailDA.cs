﻿using System;
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
                if (item.RoleName == "PM")
                {
                    mails.Body = "<html><meta charset=\"ISO-8859-1\"><head><link rel = 'stylesheet' href = 'https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css' ></head><body><div class='container'><br/><b>Please Click The Below Link To Approve:</b><br/>&nbsp<a href='" + ipaddress + "'>" + ipaddress + " </a></div></body></html>";
                    mails.ToEmailId = obj.Employees.Where(x => x.EmployeeNo == item.Approver).FirstOrDefault().EMail;
                    mails.Subject = "Purchase Authorization Waiting For Your Approval";
                    if (mails.FrmEmailId != "NULL" && mails.ToEmailId != "NULL")
                        this.sendEmail(mails);
                }
                else if (item.RoleName == "CPM")
                {
                    mails.Body = "<html><meta charset=\"ISO-8859-1\"><head><link rel = 'stylesheet' href = 'https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css' ></head><body><div class='container'><br/><b>Please Click The Below Link To Approve:</b><br/>&nbsp<a href='" + ipaddress + "'>" + ipaddress + " </a></div></body></html>";
                    mails.ToEmailId = obj.Employees.Where(x => x.EmployeeNo == item.Approver).FirstOrDefault().EMail;
                    mails.Subject = "Purchase Authorization Waiting For Your Approval";
                    if (mails.FrmEmailId != "NULL" && mails.ToEmailId != "NULL")
                        this.sendEmail(mails);
                }
                else if (item.RoleName == "DM")
                {
                    mails.Body = "<html><meta charset=\"ISO-8859-1\"><head><link rel = 'stylesheet' href = 'https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css' ></head><body><div class='container'><br/><b>Please Click The Below Link To Approve:</b><br/>&nbsp<a href='" + ipaddress + "'>" + ipaddress + " </a></div></body></html>";
                    mails.ToEmailId = obj.Employees.Where(x => x.EmployeeNo == item.Approver).FirstOrDefault().EMail;
                    mails.Subject = "Purchase Authorization Waiting For Your Approval";
                    if (mails.FrmEmailId != "NULL" && mails.ToEmailId != "NULL")
                        this.sendEmail(mails);
                }
                else if (item.RoleName == "CMMH")
                {
                    mails.Body = "<html><meta charset=\"ISO-8859-1\"><head><link rel = 'stylesheet' href = 'https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css' ></head><body><div class='container'><br/><b>Please Click The Below Link To Approve:</b><br/>&nbsp<a href='" + ipaddress + "'>" + ipaddress + " </a></div></body></html>";
                    mails.ToEmailId = obj.Employees.Where(x => x.EmployeeNo == item.Approver).FirstOrDefault().EMail;
                    mails.Subject = "Purchase Authorization Waiting For Your Approval";
                    if (mails.FrmEmailId != "NULL" && mails.ToEmailId != "NULL")
                        this.sendEmail(mails);
                }
                else if (item.RoleName == "UH")
                {
                    mails.Body = "<html><meta charset=\"ISO-8859-1\"><head><link rel = 'stylesheet' href = 'https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css' ></head><body><div class='container'><br/><b>Please Click The Below Link To Approve:</b><br/>&nbsp<a href='" + ipaddress + "'>" + ipaddress + " </a></div></body></html>";
                    mails.ToEmailId = obj.Employees.Where(x => x.EmployeeNo == item.Approver).FirstOrDefault().EMail;
                    mails.Subject = "Purchase Authorization Waiting For Your Approval";
                    if (mails.FrmEmailId != "NULL" && mails.ToEmailId != "NULL")
                        this.sendEmail(mails);
                }
                else if (item.RoleName == "DGM")
                {
                    mails.Body = "<html><meta charset=\"ISO-8859-1\"><head><link rel = 'stylesheet' href = 'https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css' ></head><body><div class='container'><br/><b>Please Click The Below Link To Approve:</b><br/>&nbsp<a href='" + ipaddress + "'>" + ipaddress + " </a></div></body></html>";
                    mails.ToEmailId = obj.Employees.Where(x => x.EmployeeNo == item.Approver).FirstOrDefault().EMail;
                    mails.Subject = "Purchase Authorization Waiting For Your Approval";
                    if (mails.FrmEmailId != "NULL" && mails.ToEmailId != "NULL")
                        this.sendEmail(mails);
                }
            }
            return true;
        }

        public bool sendEmail(EmailSend emlSndngList)
        {
            bool validEmail = IsValidEmail(emlSndngList.ToEmailId);
            if (!string.IsNullOrEmpty(emlSndngList.ToEmailId) && !string.IsNullOrEmpty(emlSndngList.FrmEmailId) && validEmail)
            {
                var BCC = ConfigurationManager.AppSettings["BCC"];
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
