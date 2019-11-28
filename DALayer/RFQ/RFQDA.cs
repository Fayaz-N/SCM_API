using SCMModels.SCMModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALayer.RFQ
{
	public class RFQDA:IRFQDA
	{
		public List<RFQItemsView> getRFQItems(int RevisionId)
		{
			using (YSCMEntities Context = new YSCMEntities())
			{
				return Context.RFQItemsViews.Where(li => li.MPRRevisionId == RevisionId).ToList();
			}
		}
	}
}

