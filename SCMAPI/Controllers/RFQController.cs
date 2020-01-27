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
        public async Task<IHttpActionResult> InsertDocument(RfqDocumentsModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            status = await _rfqBusenessAcess.InsertDocument(model);
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
        [Route("UpdateVendorCommunication")]
        [ResponseType(typeof(string))]
        public IHttpActionResult UpdateVendorCommunication(RfqCommunicationModel model)
        {
            return Ok(_rfqBusenessAcess.UpdateVendorCommunication(model));
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
        [HttpPost]
        [Route("getRFQList")]
        public IHttpActionResult getRFQList(rfqFilterParams rfqfilterparams)
        {
            return Ok(this._rfqBusenessAcess.getRFQList(rfqfilterparams));
        }
        //pa authorization
        [Route("InsertPAAuthorizationLimits")]
        [ResponseType(typeof(statuscheckmodel))]
        public async Task<IHttpActionResult> InsertPAAuthorizationLimits(PAAuthorizationLimitModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            status = await _rfqBusenessAcess.InsertPAAuthorizationLimits(model);
            return Ok(status);
        }
        [Route("GetPAAuthorizationLimitById")]
        [ResponseType(typeof(PAAuthorizationLimitModel))]
        public async Task<IHttpActionResult> GetPAAuthorizationLimitById(int deptid)
        {
            PAAuthorizationLimitModel status = new PAAuthorizationLimitModel();
            status = await _rfqBusenessAcess.GetPAAuthorizationLimitById(deptid);
            return Ok(status);
        }
        [Route("CreatePAAuthirizationEmployeeMapping")]
        [ResponseType(typeof(statuscheckmodel))]
        public async Task<IHttpActionResult> CreatePAAuthirizationEmployeeMapping(PAAuthorizationEmployeeMappingModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            status = await _rfqBusenessAcess.CreatePAAuthirizationEmployeeMapping(model);
            return Ok(status);
        }
        [Route("GetMappingEmployee")]
        [ResponseType(typeof(PAAuthorizationEmployeeMappingModel))]
        public async Task<IHttpActionResult> GetMappingEmployee(PAAuthorizationLimitModel model)
        {
            PAAuthorizationEmployeeMappingModel status = new PAAuthorizationEmployeeMappingModel();
            status = await _rfqBusenessAcess.GetMappingEmployee(model);
            return Ok(status);
        }
        [Route("CreatePACreditDaysmaster")]
        [ResponseType(typeof(statuscheckmodel))]
        public async Task<IHttpActionResult> CreatePACreditDaysmaster(PACreditDaysMasterModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            status = await _rfqBusenessAcess.CreatePACreditDaysmaster(model);
            return Ok(status);
        }
        [Route("GetCreditdaysMasterByID")]
        [ResponseType(typeof(PACreditDaysMasterModel))]
        public async Task<IHttpActionResult> GetCreditdaysMasterByID(int creditdaysid)
        {
            PACreditDaysMasterModel status = new PACreditDaysMasterModel();
            status = await _rfqBusenessAcess.GetCreditdaysMasterByID(creditdaysid);
            return Ok(status);
        }
        [Route("AssignCreditdaysToEmployee")]
        [ResponseType(typeof(statuscheckmodel))]
        public async Task<IHttpActionResult> AssignCreditdaysToEmployee(PACreditDaysApproverModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            status = await _rfqBusenessAcess.AssignCreditdaysToEmployee(model);
            return Ok(status);
        }
        [Route("RemovePAAuthorizationLimitsByID")]
        [ResponseType(typeof(statuscheckmodel))]
        public async Task<IHttpActionResult> RemovePAAuthorizationLimitsByID(int authid)
        {
            statuscheckmodel status = new statuscheckmodel();
            status = await _rfqBusenessAcess.RemovePAAuthorizationLimitsByID(authid);
            return Ok(status);
        }
        [Route("RemovePACreditDaysMaster")]
        [ResponseType(typeof(statuscheckmodel))]
        public async Task<IHttpActionResult> RemovePACreditDaysMaster(int creditid)
        {
            statuscheckmodel status = new statuscheckmodel();
            status = await _rfqBusenessAcess.RemovePACreditDaysMaster(creditid);
            return Ok(status);
        }
        [Route("GetPAAuthorizationLimitsByDeptId")]
        [ResponseType(typeof(List<PAAuthorizationLimitModel>))]
        public async Task<IHttpActionResult> GetPAAuthorizationLimitsByDeptId(int departmentid)
        {
            List<PAAuthorizationLimitModel> model = new List<PAAuthorizationLimitModel>();
            model = await _rfqBusenessAcess.GetPAAuthorizationLimitsByDeptId(departmentid);
            return Ok(model);
        }
        [HttpPost]
        [Route("RemovePACreditDaysApprover")]
        [ResponseType(typeof(statuscheckmodel))]
        public async Task<IHttpActionResult> RemovePACreditDaysApprover(EmployeemappingtocreditModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            status = await _rfqBusenessAcess.RemovePACreditDaysApprover(model);
            return Ok(model);
        }
        [Route("GetPACreditDaysApproverById")]
        [ResponseType(typeof(PACreditDaysApproverModel))]
        public async Task<IHttpActionResult> GetPACreditDaysApproverById(int ApprovalId)
        {
            PACreditDaysApproverModel status = new PACreditDaysApproverModel();
            status = await _rfqBusenessAcess.GetPACreditDaysApproverById(ApprovalId);
            return Ok(status);
        }

        [HttpPost]
        [Route("GetEmployeeMappings")]
        [ResponseType(typeof(EmployeModel))]
        public async Task<IHttpActionResult> GetEmployeeMappings(PAConfigurationModel model)
        {
            EmployeModel employee = new EmployeModel();
            employee = await _rfqBusenessAcess.GetEmployeeMappings(model);
            return Ok(employee);
        }
        [Route("GetRfqItemsByRevisionId")]
        [ResponseType(typeof(List<RfqItemModel>))]
        public async Task<IHttpActionResult> GetRfqItemsByRevisionId(int revisionid)
        {
            List<RfqItemModel> model = new List<RfqItemModel>();
            model = await _rfqBusenessAcess.GetRfqItemsByRevisionId(revisionid);
            return Ok(model);
        }
        //[HttpPost]
        //[Route("GetItemsByMasterIDs")]
        //[ResponseType(typeof(List<LoadItemsByID>))]
        //public async Task<IHttpActionResult> GetItemsByMasterIDs(PADetailsModel masters)
        //{

        //    List<LoadItemsByID> model = new List<LoadItemsByID>();
        //    model = await _rfqBusenessAcess.GetItemsByMasterIDs(masters);
        //    return Ok(model);
        //}
        [HttpPost]
        [Route("GetItemsByMasterIDs")]
        [ResponseType(typeof(List<LoadItemsByID>))]
        public IHttpActionResult GetItemsByMasterIDs(PADetailsModel masters)
        {
            return Ok(this._rfqBusenessAcess.GetItemsByMasterIDs(masters));
        }
        [HttpGet]
        [Route("GetAllDepartments")]
        [ResponseType(typeof(List<DepartmentModel>))]
        public async Task<IHttpActionResult> GetAllDepartments()
        {
            List<DepartmentModel> model = new List<DepartmentModel>();
            model = await _rfqBusenessAcess.GetAllDepartments();
            return Ok(model);
        }
        [HttpGet]
        [Route("GetSlabsByDepartmentID/{DeptID}")]
        [ResponseType(typeof(List<PAAuthorizationLimitModel>))]
        public async Task<IHttpActionResult> GetSlabsByDepartmentID(int DeptID)
        {
            List<PAAuthorizationLimitModel> model = new List<PAAuthorizationLimitModel>();
            model = await _rfqBusenessAcess.GetSlabsByDepartmentID(DeptID);
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
        [HttpGet]
        [Route("GetAllCredits")]
        [ResponseType(typeof(List<PAAuthorizationLimitModel>))]
        public async Task<IHttpActionResult> GetAllCredits()
        {
            List<PAAuthorizationLimitModel> model = new List<PAAuthorizationLimitModel>();
            model = await _rfqBusenessAcess.GetAllCredits();
            return Ok(model);
        }
        [HttpGet]
        [Route("GetAllCreditDays")]
        [ResponseType(typeof(List<PACreditDaysMasterModel>))]
        public async Task<IHttpActionResult> GetAllCreditDays()
        {
            List<PACreditDaysMasterModel> model = new List<PACreditDaysMasterModel>();
            model = await _rfqBusenessAcess.GetAllCreditDays();
            return Ok(model);
        }
        [HttpGet]
        [Route("GetAllMprPAPurchaseModes")]
        [ResponseType(typeof(List<MPRPAPurchaseModesModel>))]
        public async Task<IHttpActionResult> GetAllMprPAPurchaseModes()
        {
            List<MPRPAPurchaseModesModel> model = new List<MPRPAPurchaseModesModel>();
            model = await _rfqBusenessAcess.GetAllMprPAPurchaseModes();
            return Ok(model);
        }
        [HttpGet]
        [Route("GetAllMprPAPurchaseTypes")]
        [ResponseType(typeof(List<MPRPAPurchaseTypesModel>))]
        public async Task<IHttpActionResult> GetAllMprPAPurchaseTypes()
        {
            List<MPRPAPurchaseTypesModel> model = new List<MPRPAPurchaseTypesModel>();
            model = await _rfqBusenessAcess.GetAllMprPAPurchaseTypes();
            return Ok(model);
        }
        [HttpPost]
        [Route("InsertPurchaseAuthorization")]
        [ResponseType(typeof(statuscheckmodel))]
        public async Task<IHttpActionResult> InsertPurchaseAuthorization(MPRPADetailsModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            status = await _rfqBusenessAcess.InsertPurchaseAuthorization(model);
            return Ok(status);
        }
        [HttpGet]
        [Route("GetMPRPADeatilsByPAID/{PID}")]
        [ResponseType(typeof(MPRPADetailsModel))]
        public async Task<IHttpActionResult> GetMPRPADeatilsByPAID(int PID)
        {
            MPRPADetailsModel model = new MPRPADetailsModel();
            model = await _rfqBusenessAcess.GetMPRPADeatilsByPAID(PID);
            return Ok(model);
        }
        [HttpGet]
        [Route("GetAllMPRPAList")]
        [ResponseType(typeof(List<MPRPADetailsModel>))]
        public async Task<IHttpActionResult> GetAllMPRPAList()
        {
            List<MPRPADetailsModel> model = new List<MPRPADetailsModel>();
            model = await _rfqBusenessAcess.GetAllMPRPAList();
            return Ok(model);
        }
        [HttpGet]
        [Route("GetAllPAFunctionalRoles")]
        [ResponseType(typeof(List<PAFunctionalRolesModel>))]
        public async Task<IHttpActionResult> GetAllPAFunctionalRoles()
        {
            List<PAFunctionalRolesModel> model = new List<PAFunctionalRolesModel>();
            model = await _rfqBusenessAcess.GetAllPAFunctionalRoles();
            return Ok(model);
        }
        [HttpGet]
        [Route("GetCreditSlabsandemployees")]
        [ResponseType(typeof(List<EmployeemappingtocreditModel>))]
        public async Task<IHttpActionResult> GetCreditSlabsandemployees()
        {
            List<EmployeemappingtocreditModel> model = new List<EmployeemappingtocreditModel>();
            model = await _rfqBusenessAcess.GetCreditSlabsandemployees();
            return Ok(model);
        }
        [HttpGet]
        [Route("GetPurchaseSlabsandMappedemployees")]
        [ResponseType(typeof(List<EmployeemappingtopurchaseModel>))]
        public async Task<IHttpActionResult> GetPurchaseSlabsandMappedemployees()
        {
            List<EmployeemappingtopurchaseModel> model = new List<EmployeemappingtopurchaseModel>();
            model = await _rfqBusenessAcess.GetPurchaseSlabsandMappedemployees();
            return Ok(model);
        }
        [HttpPost]
        [Route("RemovePurchaseApprover")]
        [ResponseType(typeof(statuscheckmodel))]
        public async Task<IHttpActionResult> RemovePurchaseApprover(EmployeemappingtopurchaseModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            status = await _rfqBusenessAcess.RemovePurchaseApprover(model);
            return Ok(model);
        }
        [HttpGet]
        [Route("GetAllProjectManagers")]
        [ResponseType(typeof(List<ProjectManagerModel>))]
        public async Task<IHttpActionResult> GetAllProjectManagers()
        {
            List<ProjectManagerModel> model = new List<ProjectManagerModel>();
            model = await _rfqBusenessAcess.LoadAllProjectManagers();
            return Ok(model);
        }
        [HttpPost]
        [Route("LoadVendorByMprDetailsId")]
        [ResponseType(typeof(List<VendormasterModel>))]
        public async Task<IHttpActionResult> LoadVendorByMprDetailsId(List<int> MPRItemDetailsid)
        {
            List<VendormasterModel> model = new List<VendormasterModel>();
            model = await _rfqBusenessAcess.LoadVendorByMprDetailsId(MPRItemDetailsid);
            return Ok(model);
        }
        [HttpPost]
        [Route("GetAllMasterCurrency")]
        [ResponseType(typeof(List<CurrencyMasterModel>))]
        public async Task<IHttpActionResult> GetAllMasterCurrency()
        {
            List<CurrencyMasterModel> model = new List<CurrencyMasterModel>();
            model = await _rfqBusenessAcess.GetAllMasterCurrency();
            return Ok(model);
        }
        [HttpGet]
        [Route("GetAllApproversList")]
        [ResponseType(typeof(List<MPRPAApproversModel>))]
        public async Task<IHttpActionResult> GetAllApproversList()
        {
            List<MPRPAApproversModel> model = new List<MPRPAApproversModel>();
            model = await _rfqBusenessAcess.GetAllApproversList();
            return Ok(model);
        }
        [HttpPost]
        [Route("GetMprApproverDetailsBySearch")]
        [ResponseType(typeof(List<GetmprApproverdeatil>))]
        public async Task<IHttpActionResult> GetMprApproverDetailsBySearch(PAApproverDetailsInputModel model)
        {
            List<GetmprApproverdeatil> details = new List<GetmprApproverdeatil>();
            details = await _rfqBusenessAcess.GetMprApproverDetailsBySearch(model);
            return Ok(details);
        }
        [HttpPost]
        [Route("UpdateMprpaApproverStatus")]
        [ResponseType(typeof(statuscheckmodel))]
        public async Task<IHttpActionResult> UpdateMprpaApproverStatus(MPRPAApproversModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            status = await _rfqBusenessAcess.UpdateMprpaApproverStatus(model);
            return Ok(model);
        }
    }
}
