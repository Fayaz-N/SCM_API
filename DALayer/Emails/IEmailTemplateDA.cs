using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALayer.Emails
{
	public interface IEmailTemplateDA
	{
		bool prepareEmailTemplate(string typeOfuser,int revisionId, string FrmEmailId, string ToEmailId, string Remarks);
	}
}
