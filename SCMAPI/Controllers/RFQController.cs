using BALayer.RFQ;
using SCMModels;
using SCMModels.MPRMasterModels;
using SCMModels.RFQModels;
using SCMModels.SCMModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace SCMAPI.Controllers
{
	[RoutePrefix("Api/RFQ")]
	public class RFQController : ApiController
	{
		private readonly IRFQBA _rfqBusenessAcess;
		public RFQController(IRFQBA rfqBA)
		{
			this._rfqBusenessAcess = rfqBA;
		}
		[HttpGet]
		[Route("getRFQItems/{MPRRevisionId}")]
		public IHttpActionResult getRFQItems(int MPRRevisionId)
		{
			return Ok(this._rfqBusenessAcess.getRFQItems(MPRRevisionId));
		}
		[HttpPost]
		[Route("updateVendorQuotes")]
		public IHttpActionResult updateVendorQuotes([FromBody] DataModel Result)
		{
			return Ok(this._rfqBusenessAcess.updateVendorQuotes(Result.RFQQuoteViewList, Result.TermsList, Result.mprfqDocs));
		}
		[HttpGet]
		[Route("getRFQCompareItems/{MPRRevisionId}")]
		public IHttpActionResult getRFQCompareItems(int MPRRevisionId)
		{
			return Ok(this._rfqBusenessAcess.getRFQCompareItems(MPRRevisionId));
		}
		[HttpGet]
		[Route("GetAllMasterCurrency")]
		public IHttpActionResult GetAllMasterCurrency()
		{
			return Ok(this._rfqBusenessAcess.GetAllMasterCurrency());
		}
		[HttpPost]
		[Route("rfqStatusUpdate")]
		public IHttpActionResult rfqStatusUpdate([FromBody] DataModel Result)
		{
			return Ok(this._rfqBusenessAcess.rfqStatusUpdate(Result.RFQQuoteViewList));
		}

		[Route("GetRFQById")]
		[ResponseType(typeof(RFQMasterModel))]
		public async Task<IHttpActionResult> GetRFQById(int id)
		{
			RFQMasterModel model = new RFQMasterModel();
			model = await _rfqBusenessAcess.GetRFQById(id);
			return Ok(model);
		}


		[Route("CreateRfq")]
		[HttpPost]
		public async Task<IHttpActionResult> CreateRfq(RfqRevisionModel model)
		{
			return Ok(await _rfqBusenessAcess.CreateRfQ(model, false));
		}
		[Route("getallrfqlist")]
		[ResponseType(typeof(List<RFQMasterModel>))]
		public async Task<IHttpActionResult> getallrfqlist()
		{
			List<RFQMasterModel> status = new List<RFQMasterModel>();
			status = await _rfqBusenessAcess.getallrfqlist();
			return Ok(status);
		}
		[Route("GetItemsByRevisionId/{id}")]
		[ResponseType(typeof(List<RfqItemModel>))]
		public async Task<IHttpActionResult> GetItemsByRevisionId(int id)
		{
			List<RfqItemModel> status = new List<RfqItemModel>();
			status = await _rfqBusenessAcess.GetItemsByRevisionId(id);
			return Ok(status);
		}

		[Route("DeleteRfqItemById")]
		[ResponseType(typeof(statuscheckmodel))]
		public IHttpActionResult DeleteRfqItemById(int id)
		{
			statuscheckmodel status = new statuscheckmodel();
			status = _rfqBusenessAcess.DeleteRfqItemById(id);
			return Ok();
		}

		[Route("InsertDocument")]
		[ResponseType(typeof(statuscheckmodel))]
		public async Task<IHttpActionResult> InsertDocument(RfqDocumentsModel model)
		{
			statuscheckmodel status = new statuscheckmodel();
			status = await _rfqBusenessAcess.InsertDocument(model);
			return Ok(status);
		}

		[Route("UpdateVendorCommunication")]
		[ResponseType(typeof(string))]
		public IHttpActionResult UpdateVendorCommunication(RfqCommunicationModel model)
		{
			return Ok(_rfqBusenessAcess.UpdateVendorCommunication(model));
		}
		[HttpPost]
		[Route("addNewRfqRevision")]
		public IHttpActionResult addNewRfqRevision([FromBody] int revisionId)
		{
			return Ok(_rfqBusenessAcess.addNewRfqRevision(revisionId));
		}

		[Route("GetItemsByItemId")]
		[ResponseType(typeof(RfqItemModel))]
		public async Task<IHttpActionResult> GetItemsByItemId(int id)
		{
			RfqItemModel status = new RfqItemModel();
			status = await _rfqBusenessAcess.GetItemsByItemId(id);
			return Ok(status);
		}
		[Route("GetAllvendorList")]
		[ResponseType(typeof(List<VendormasterModel>))]
		public IHttpActionResult GetAllvendorList()
		{
			List<VendormasterModel> status = new List<VendormasterModel>();
			status = _rfqBusenessAcess.GetAllvendorList();
			return Ok(status);
		}

		[Route("GetRfqDetailsById/{RevisionId}")]
		[ResponseType(typeof(RfqRevisionModel))]
		public async Task<IHttpActionResult> GetRfqDetailsById(int RevisionId)
		{
			RfqRevisionModel revision = new RfqRevisionModel();
			revision = await _rfqBusenessAcess.GetRfqDetailsById(RevisionId);
			return Ok(revision);
		}
		[HttpPost]
		[Route("updateRfqDocStatus")]
		public IHttpActionResult updateRfqDocStatus([FromBody] List<RFQDocument> rfqdocs)
		{
			return Ok(_rfqBusenessAcess.updateRfqDocStatus(rfqdocs));
		}



		[Route("CreateNewRfq")]
		[ResponseType(typeof(statuscheckmodel))]
		public async Task<IHttpActionResult> CreateNewRfq(RFQMasterModel model)
		{
			statuscheckmodel status = new statuscheckmodel();
			status = await _rfqBusenessAcess.CreateNewRfq(model);
			return Ok(status);
		}

		[Route("GetvendorById/{id}")]
		[ResponseType(typeof(VendormasterModel))]
		public async Task<IHttpActionResult> GetvendorById(int id)
		{
			VendormasterModel status = new VendormasterModel();
			status = await _rfqBusenessAcess.GetvendorById(id);
			return Ok(status);
		}
		[Route("InsertVendorterms")]
		[ResponseType(typeof(statuscheckmodel))]
		public async Task<IHttpActionResult> InsertVendorterms(VendorRfqtermModel vendor)
		{
			statuscheckmodel status = new statuscheckmodel();
			status = await _rfqBusenessAcess.InsertVendorterms(vendor);
			return Ok(status);
		}
		[Route("InsertRfqItemInfo")]
		[ResponseType(typeof(RfqRevisionModel))]
		public async Task<IHttpActionResult> InsertRfqItemInfo(RFQItemsInfo_N model)
		{

			return Ok(await _rfqBusenessAcess.InsertRfqItemInfo(model));
		}

		[HttpGet]
		[Route("DeleteRfqIteminfoByid/{RFQSplitId}")]
		public async Task<IHttpActionResult> DeleteRfqIteminfoByid(int RFQSplitId)
		{
			return Ok(await _rfqBusenessAcess.DeleteRfqIteminfoByid(RFQSplitId));
		}

		[HttpGet]
		[Route("DeleteRfqItemByid/{RFQItemId}")]
		public async Task<IHttpActionResult> DeleteRfqItemByid(int RFQItemId)
		{
			return Ok(await _rfqBusenessAcess.DeleteRfqItemByid(RFQItemId));
		}
		[Route("DeleteRfqitemandinfosById/{id}")]
		[ResponseType(typeof(statuscheckmodel))]
		public async Task<IHttpActionResult> DeleteRfqitemandinfosById(int id)
		{
			statuscheckmodel status = new statuscheckmodel();
			status = await _rfqBusenessAcess.DeleteRfqitemandinfosById(id);
			return Ok(status);
		}
		[Route("UpdateRfqItemInfoById")]
		[ResponseType(typeof(statuscheckmodel))]
		public async Task<IHttpActionResult> UpdateRfqItemInfoById(RfqItemInfoModel model)
		{
			statuscheckmodel status = new statuscheckmodel();
			status = await _rfqBusenessAcess.UpdateRfqItemInfoById(model);
			return Ok(status);
		}

		[Route("GetRfqItemByMPrId")]
		[ResponseType(typeof(RfqItemModel))]
		public async Task<IHttpActionResult> GetRfqItemByMPrId(int id)
		{
			RfqItemModel status = new RfqItemModel();
			status = await _rfqBusenessAcess.GetRfqItemByMPrId(id);
			return Ok(status);
		}

		[Route("InsertSingleIteminfos")]
		[ResponseType(typeof(statuscheckmodel))]
		public async Task<IHttpActionResult> InsertSingleIteminfos(RfqItemInfoModel model)
		{
			statuscheckmodel status = new statuscheckmodel();
			status = await _rfqBusenessAcess.InsertSingleIteminfos(model);
			return Ok(status);
		}

		[Route("InsertBulkItemInfos")]
		[ResponseType(typeof(statuscheckmodel))]
		public async Task<IHttpActionResult> InsertBulkItemInfos(List<RfqItemInfoModel> model)
		{
			statuscheckmodel status = new statuscheckmodel();
			status = await _rfqBusenessAcess.InsertBulkItemInfos(model);
			return Ok(status);
		}

		[Route("GetUnitMasterList")]
		[ResponseType(typeof(List<UnitMasterModel>))]
		public async Task<IHttpActionResult> GetUnitMasterList()
		{
			List<UnitMasterModel> model = new List<UnitMasterModel>();
			model = await _rfqBusenessAcess.GetUnitMasterList();
			return Ok(model);
		}
		[Route("GetAllMPRBuyerGroups")]
		[ResponseType(typeof(List<MPRBuyerGroupModel>))]
		public async Task<IHttpActionResult> GetAllMPRBuyerGroups()
		{
			List<MPRBuyerGroupModel> model = new List<MPRBuyerGroupModel>();
			model = await _rfqBusenessAcess.GetAllMPRBuyerGroups();
			return Ok(model);
		}
		[Route("InsertBuyerGroup")]
		[ResponseType(typeof(statuscheckmodel))]
		public async Task<IHttpActionResult> InsertBuyerGroup(MPRBuyerGroupModel model)
		{
			statuscheckmodel status = new statuscheckmodel();
			status = await _rfqBusenessAcess.InsertMprBuyerGroups(model);
			return Ok(status);
		}
		[Route("UpdateMprBuyerGroups")]
		[ResponseType(typeof(statuscheckmodel))]
		public async Task<IHttpActionResult> UpdateMprBuyerGroups(MPRBuyerGroupModel model)
		{
			statuscheckmodel status = new statuscheckmodel();
			status = await _rfqBusenessAcess.UpdateMprBuyerGroups(model);
			return Ok(status);
		}
		[Route("GetMPRApprovalsById/{id}")]
		[ResponseType(typeof(MPRApproverModel))]
		public async Task<IHttpActionResult> GetMPRApprovalsById(int id)
		{
			MPRApproverModel model = new MPRApproverModel();
			model = await _rfqBusenessAcess.GetMPRApprovalsById(id);
			return Ok(model);
		}

		[Route("GetAllMPRApprovers")]
		[ResponseType(typeof(List<MPRApproversViewModel>))]
		public async Task<IHttpActionResult> GetAllMPRApprovers()
		{
			List<MPRApproversViewModel> model = new List<MPRApproversViewModel>();
			model = await _rfqBusenessAcess.GetAllMPRApprovers();
			return Ok(model);
		}

		[Route("InsertMPRApprover")]
		[ResponseType(typeof(statuscheckmodel))]
		public async Task<IHttpActionResult> InsertMPRApprover(MPRApproverModel model)
		{
			statuscheckmodel status = new statuscheckmodel();
			status = await _rfqBusenessAcess.InsertMPRApprover(model);
			return Ok(status);
		}

		[Route("GetMPRBuyerGroupsById")]
		[ResponseType(typeof(MPRBuyerGroupModel))]
		public async Task<IHttpActionResult> GetMPRBuyerGroupsById(int id)
		{
			MPRBuyerGroupModel status = new MPRBuyerGroupModel();
			status = await _rfqBusenessAcess.GetMPRBuyerGroupsById(id);
			return Ok(status);
		}

		[Route("GetAllMPRApprovals")]
		[ResponseType(typeof(List<MPRApproverModel>))]
		public async Task<IHttpActionResult> GetAllMPRApprovals()
		{
			List<MPRApproverModel> model = new List<MPRApproverModel>();
			model = await _rfqBusenessAcess.GetAllMPRApprovals();
			return Ok(model);
		}
		[Route("GetAllMPRDepartments")]
		[ResponseType(typeof(List<MPRDepartmentModel>))]
		public async Task<IHttpActionResult> GetAllMPRDepartments()
		{
			List<MPRDepartmentModel> model = new List<MPRDepartmentModel>();
			model = await _rfqBusenessAcess.GetAllMPRDepartments();
			return Ok(model);
		}
		[Route("GetMPRDepartmentById/{id}")]
		[ResponseType(typeof(MPRDepartmentModel))]
		public async Task<IHttpActionResult> GetMPRDepartmentById(int id)
		{
			MPRDepartmentModel model = new MPRDepartmentModel();
			model = await _rfqBusenessAcess.GetMPRDepartmentById(id);
			return Ok(model);
		}
		[Route("GetAllMPRDispatchLocations")]
		[ResponseType(typeof(List<MPRDispatchLocationModel>))]
		public async Task<IHttpActionResult> GetAllMPRDispatchLocations()
		{
			List<MPRDispatchLocationModel> model = new List<MPRDispatchLocationModel>();
			model = await _rfqBusenessAcess.GetAllMPRDispatchLocations();
			return Ok(model);
		}
		[Route("GetMPRDispatchLocationById/{id}")]
		[ResponseType(typeof(MPRDispatchLocationModel))]
		public async Task<IHttpActionResult> GetMPRDispatchLocationById(int id)
		{
			MPRDispatchLocationModel model = new MPRDispatchLocationModel();
			model = await _rfqBusenessAcess.GetMPRDispatchLocationById(id);
			return Ok(model);
		}

		[Route("InsertYILTerms")]
		[ResponseType(typeof(statuscheckmodel))]
		public async Task<IHttpActionResult> InsertYILTerms(YILTermsandConditionModel model)
		{
			statuscheckmodel status = new statuscheckmodel();
			status = await _rfqBusenessAcess.InsertYILTerms(model);
			return Ok(model);
		}
		[Route("InsertYILTermsGroup")]
		[ResponseType(typeof(statuscheckmodel))]
		public async Task<IHttpActionResult> InsertYILTermsGroup(YILTermsGroupModel model)
		{
			statuscheckmodel status = new statuscheckmodel();
			status = await _rfqBusenessAcess.InsertYILTermsGroup(model);
			return Ok(model);
		}
		[Route("InsertRFQTerms")]
		[ResponseType(typeof(statuscheckmodel))]
		public async Task<IHttpActionResult> InsertRFQTerms(RFQTermsModel model)
		{
			statuscheckmodel status = new statuscheckmodel();
			status = await _rfqBusenessAcess.InsertRFQTerms(model);
			return Ok(model);
		}
		[HttpGet]
		[Route("GetAllEmployee")]
		[ResponseType(typeof(List<EmployeeModel>))]
		public async Task<IHttpActionResult> GetAllEmployee()
		{
			List<EmployeModel> model = new List<EmployeModel>();
			model = await _rfqBusenessAcess.GetAllEmployee();
			return Ok(model);
		}
		[HttpPost]
		[Route("getRFQList")]
		public IHttpActionResult getRFQList(rfqFilterParams rfqfilterparams)
		{
			return Ok(this._rfqBusenessAcess.getRFQList(rfqfilterparams));
		}


		[HttpPost]
		[Route("PreviouPriceUpdate")]
		public IHttpActionResult PreviouPriceUpdate(MPRItemInfo previousprice)
		{
			return Ok(this._rfqBusenessAcess.PreviouPriceUpdate(previousprice));
		}

		[HttpGet]
		[Route("GetRfqByVendorId/{VendorId}")]
		public async Task<IHttpActionResult> GetRfqByVendorId(int VendorId)
		{
			return Ok(await _rfqBusenessAcess.GetRfqByVendorId(VendorId));
		}
		[HttpPost]
		[Route("updateHandlingCharges")]
		public IHttpActionResult updateHandlingCharges(List<RFQItems_N> rfqItems)
		{
			return Ok(this._rfqBusenessAcess.updateHandlingCharges(rfqItems));
		}
		[HttpPost]
		[Route("UpdateNewCurrencyMaster")]
		public IHttpActionResult UpdateNewCurrencyMaster(CurrencyMaster currencyMaster)
		{
			return Ok(this._rfqBusenessAcess.UpdateNewCurrencyMaster(currencyMaster));
		}
		[HttpGet]
		[Route("RemoveMasterCurrencyById/{currencyId}/{DeletedBy}")]
		public IHttpActionResult RemoveMasterCurrencyById(int currencyId,string DeletedBy)
		{
			return Ok(this._rfqBusenessAcess.RemoveMasterCurrencyById(currencyId, DeletedBy));
		}
	}
}
