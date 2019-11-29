using SCMModels.SCMModels;
using System;
using System.Collections.Generic;

namespace BALayer.RFQ
{
	public interface IRFQBA
	{

		List<RFQItemsView> getRFQItems(int RevisionId);
	}
}
