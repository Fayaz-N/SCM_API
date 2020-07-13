using SCMModels;
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
        bool prepareRFQGeneratedEmail(string FrmEmailId, int VendorId);
        bool prepareMPRStatusEmail(string FrmEmailId, string ToEmailId, string type, int revisionid);
        bool sendMailtoVendor(sendMailObj mailObj);
		bool mailtoRequestor(int revisionId, string FrmEmailId);

		bool sendEmail(EmailSend emlSndngList);

    }
}
