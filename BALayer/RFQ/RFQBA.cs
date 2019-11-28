using DALayer.RFQ;
using SCMModels.SCMModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BALayer.RFQ
{
	public class RFQBA:IRFQBA
	{
		public readonly IRFQDA _rfqDataAcess;
		public RFQBA(IRFQDA RFQDA)
		{
			this._rfqDataAcess = RFQDA;
		}
		public List<RFQItemsView> getRFQItems(int RevisionId)
		{
			return this._rfqDataAcess.getRFQItems(RevisionId);
		}
	}
}
