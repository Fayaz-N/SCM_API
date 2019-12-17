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
            return Ok(this._rfqBusenessAcess.updateVendorQuotes(Result.RFQQuoteViewList, Result.TermsList));
        }
        [HttpGet]
        [Route("getRFQCompareItems/{MPRRevisionId}")]
        public IHttpActionResult getRFQCompareItems(int MPRRevisionId)
        {
            return Ok(this._rfqBusenessAcess.getRFQCompareItems(MPRRevisionId));
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
        //[Route("GetAllRFQs")]
        //[ResponseType(typeof(List<RFQRevision>))]
        //public async Task<IHttpActionResult> GetAllRFQs()
        //{
        //    List<RFQRevision> model = new List<RFQRevision>();
        //    model =await _rfqbusiness.GetAllRFQs();
        //    return Ok(model);
        //}

        [Route("CreateRfq")]
        [ResponseType(typeof(statuscheckmodel))]
        public async Task<IHttpActionResult> CreateRfq(RfqRevisionModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            status = await _rfqBusenessAcess.CreateRfQ(model);
            return Ok(status);
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
        [Route("GetAllrevisionRFQs")]
        [ResponseType(typeof(List<RfqRevisionModel>))]
        public async Task<IHttpActionResult> GetAllrevisionRFQs()
        {
            List<RfqRevisionModel> status = new List<RfqRevisionModel>();
            status = await _rfqBusenessAcess.GetAllrevisionRFQs();
            return Ok(status);
        }

        [Route("DeleteRfqById")]
        [ResponseType(typeof(statuscheckmodel))]
        public IHttpActionResult DeleteRfqById(int id)
        {
            statuscheckmodel status = new statuscheckmodel();
            status = _rfqBusenessAcess.DeleteRfqById(id);
            return Ok();
        }
        [Route("UpdateRfqRevision")]
        [ResponseType(typeof(statuscheckmodel))]
        public async Task<IHttpActionResult> UpdateRfqRevision(RfqRevisionModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            status = await _rfqBusenessAcess.UpdateRfqRevision(model);
            return Ok(status);
        }
        [Route("UpdateRfqItemByBulk")]
        [ResponseType(typeof(statuscheckmodel))]
        public async Task<IHttpActionResult> UpdateRfqItemByBulk(RfqItemModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            status = await _rfqBusenessAcess.UpdateRfqItemByBulk(model);
            return Ok(status);
        }

        [Route("UpdateSingleRfqItem")]
        [ResponseType(typeof(statuscheckmodel))]
        public async Task<IHttpActionResult> UpdateSingleRfqItem(RfqItemModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            status = await _rfqBusenessAcess.UpdateSingleRfqItem(model);
            return Ok(status);
        }

        [Route("UpdateBulkRfqRevision")]
        [ResponseType(typeof(statuscheckmodel))]
        public async Task<IHttpActionResult> UpdateBulkRfqRevision(RfqRevisionModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            status = await _rfqBusenessAcess.UpdateBulkRfqRevision(model);
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

        [Route("DeleteBulkItemsByItemId")]
        [ResponseType(typeof(statuscheckmodel))]
        public IHttpActionResult DeleteBulkItemsByItemId(List<int> id)
        {
            statuscheckmodel status = new statuscheckmodel();
            status = _rfqBusenessAcess.DeleteBulkItemsByItemId(id);
            return Ok();
        }

        [Route("InsertDocument")]
        [ResponseType(typeof(statuscheckmodel))]
        public IHttpActionResult InsertDocument(RfqDocumentsModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            status = _rfqBusenessAcess.InsertDocument(model);
            return Ok(status);
        }
        [Route("CommunicationAdd")]
        [ResponseType(typeof(statuscheckmodel))]
        public IHttpActionResult CommunicationAdd(RfqCommunicationModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            status = _rfqBusenessAcess.CommunicationAdd(model);
            return Ok(status);
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

        [Route("GetRfqDetailsById")]
        [ResponseType(typeof(RfqRevisionModel))]
        public async Task<IHttpActionResult> GetRfqDetailsById(int revisionId)
        {
            RfqRevisionModel revision = new RfqRevisionModel();
            revision = await _rfqBusenessAcess.GetRfqDetailsById(revisionId);
            return Ok(revision);
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
        [ResponseType(typeof(statuscheckmodel))]
        public async Task<IHttpActionResult> InsertRfqItemInfo(RfqItemModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            status = await _rfqBusenessAcess.InsertRfqItemInfo(model);
            return Ok(status);
        }

        [Route("DeleteRfqIteminfoByid")]
        [ResponseType(typeof(statuscheckmodel))]
        public async Task<IHttpActionResult> DeleteRfqIteminfoByid(List<int> id)
        {
            statuscheckmodel status = new statuscheckmodel();
            status = await _rfqBusenessAcess.DeleteRfqIteminfoByid(id);
            return Ok(status);
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
        [Route("GetMPRBuyerGroupsById")]
        [ResponseType(typeof(MPRBuyerGroupModel))]
        public async Task<IHttpActionResult> GetMPRBuyerGroupsById(int id)
        {
            MPRBuyerGroupModel status = new MPRBuyerGroupModel();
            status = await _rfqBusenessAcess.GetMPRBuyerGroupsById(id);
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
        [Route("GetAllMPRDepartments")]
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
        [Route("GetAllCustomDuty")]
        [ResponseType(typeof(List<MPRCustomsDutyModel>))]
        public async Task<IHttpActionResult> GetAllCustomDuty()
        {
            List<MPRCustomsDutyModel> model = new List<MPRCustomsDutyModel>();
            model = await _rfqBusenessAcess.GetAllCustomDuty();
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
        [Route("UpdateRFQTerms")]
        [ResponseType(typeof(statuscheckmodel))]
        public async Task<IHttpActionResult> UpdateRFQTerms(RFQTermsModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            status = await _rfqBusenessAcess.UpdateRFQTerms(model);
            return Ok(status);
        }
        [Route("GetRfqTermsById")]
        [ResponseType(typeof(RFQTermsModel))]
        public async Task<IHttpActionResult> GetRfqTermsById(int id)
        {
            RFQTermsModel status = new RFQTermsModel();
            status = await _rfqBusenessAcess.GetRfqTermsById(id);
            return Ok(status);
        }
        [Route("GetYILTermsByBuyerGroupID")]
        [ResponseType(typeof(YILTermsandConditionModel))]
        public async Task<IHttpActionResult> GetYILTermsByBuyerGroupID(int id)
        {
            YILTermsandConditionModel status = new YILTermsandConditionModel();
            status = await _rfqBusenessAcess.GetYILTermsByBuyerGroupID(id);
            return Ok(status);
        }
        [Route("GetYILTermsGroupById")]
        [ResponseType(typeof(YILTermsGroupModel))]
        public async Task<IHttpActionResult> GetYILTermsGroupById(int id)
        {
            YILTermsGroupModel status = new YILTermsGroupModel();
            status = await _rfqBusenessAcess.GetYILTermsGroupById(id);
            return Ok(status);
        }
    }
}
