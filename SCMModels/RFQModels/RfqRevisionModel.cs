using SCMModels.SCMModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMModels.RFQModels
{
	public class RfqRevisionModel
	{
		public RfqRevisionModel()
		{
			rfqitem = new List<RfqItemModel>();
			RFQStatus = new List<RFQStatu>();
			rfqvendor = new List<RfqVendorTermModel>();
			RFQTerms = new List<RFQTermsModel>();
			mprIncharges = new List<MPRIncharge>();
			rfqCommunications = new List<RFQCommunication>();
			RfqDocuments = new List<MPRRFQDocument>();
			RFQStatusTrackDetails = new List<RFQStatusTrackDetail>();

		}
		public int RfqRevisionId { get; set; }
		public int RfqMasterId { get; set; }
		public int RfqRevisionNo { get; set; }
		public string RFQType { get; set; }
		public Nullable<System.DateTime> QuoteValidFrom { get; set; }
		public Nullable<System.DateTime> QuoteValidTo { get; set; }
		public string CreatedBy { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime RfqValidDate { get; set; }
		public string PackingForwading { get; set; }
		public string ExciseDuty { get; set; }
		public string salesTax { get; set; }
		public string freight { get; set; }
		public string Insurance { get; set; }
		public string CustomsDuty { get; set; }
		public string VendorName { get; set; }
		public Nullable<int> ShipmentModeId { get; set; }
		public Nullable<int> PaymentTermDays { get; set; }
		public string PaymentTermRemarks { get; set; }
		public string Remarks { get; set; }
		public string BankGuarantee { get; set; }
		public Nullable<int> DeliveryMinWeeks { get; set; }
		public Nullable<int> DeliveryMaxWeeks { get; set; }
		public Nullable<bool> IsDeleted { get; set; }
		public bool ActiveRevision { get; set; }
		public RFQMasterModel rfqmaster { get; set; }
		public List<RfqItemModel> rfqitem { get; set; }
		public List<RFQStatu> RFQStatus { get; set; }
		public List<RfqVendorTermModel> rfqvendor { get; set; }
		public List<RFQTermsModel> RFQTerms { get; set; }
		public List<MPRIncharge> mprIncharges { get; set; }
		public List<RFQCommunication> rfqCommunications { get; set; }
		public List<MPRRFQDocument> RfqDocuments { get; set; }
		public List<RFQDocument> RFQDocs { get; set; }
		public Nullable<byte> StatusId { get; set; }
		public List<RFQStatusTrackDetail> RFQStatusTrackDetails { get; set; }

	}
}
