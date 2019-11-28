using SCMModels.SCMModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALayer.RFQ
{
	public interface IRFQDA
	{
		List<RFQItemsView> getRFQItems(int RevisionId);
	}
}
