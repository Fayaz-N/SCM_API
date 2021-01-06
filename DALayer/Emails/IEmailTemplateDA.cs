using SCMModels;
using SCMModels.SCMModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALayer.Emails
{
	public interface IEmailTemplateDA
	{
		bool prepareMPREmailTemplate(string typeOfuser,int revisionId, string FrmEmailId, string ToEmailId, string Remarks);
        bool prepareRFQGeneratedEmail(string FrmEmailId, int VendorId,string rfqno,bool Reminder);
        bool prepareMPRStatusEmail(string FrmEmailId, string ToEmailId, string type, int revisionid);
        bool sendMailtoVendor(sendMailObj mailObj);
		bool mailtoRequestor(int revisionId, string FrmEmailId);
		bool prepareAribaTemplate(int PaId, string FrmEmailId, string ToMailId, string typeOfUser, int revisionId);
		bool sendEmail(EmailSend emlSndngList);
		bool prepareVendRegTemplate(string typeOfUser,  int VendorId, bool isexistvendor);
		bool sendTechNotificationMail(int RFQRevisionId, string status,string StatusBy);
		bool sendASNCommunicationMail(int ASNId, string Remarks, string RemarksFrom);
		bool sendASNInitiationEmail(ASNInitiation asnIniLocal);

	}
}
