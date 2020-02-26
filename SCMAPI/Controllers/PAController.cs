﻿using System;
using SCMModels;
using SCMModels.MPRMasterModels;
using SCMModels.RFQModels;
using SCMModels.SCMModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using BALayer.PurchaseAuthorization;

namespace SCMAPI.Controllers
{
    [RoutePrefix("Api/PA")]
    public class PAController : ApiController
    {
        private readonly IPurchaseAuthorizationBA _paBusenessAcess;
        public PAController(IPurchaseAuthorizationBA purchase)
        {
            this._paBusenessAcess = purchase;
        }
        [Route("InsertPAAuthorizationLimits")]
        [ResponseType(typeof(statuscheckmodel))]
        public async Task<IHttpActionResult> InsertPAAuthorizationLimits(PAAuthorizationLimitModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            status = await _paBusenessAcess.InsertPAAuthorizationLimits(model);
            return Ok(status);
        }
        [Route("GetPAAuthorizationLimitById")]
        [ResponseType(typeof(PAAuthorizationLimitModel))]
        public async Task<IHttpActionResult> GetPAAuthorizationLimitById(int deptid)
        {
            PAAuthorizationLimitModel status = new PAAuthorizationLimitModel();
            status = await _paBusenessAcess.GetPAAuthorizationLimitById(deptid);
            return Ok(status);
        }
        [Route("CreatePAAuthirizationEmployeeMapping")]
        [ResponseType(typeof(statuscheckmodel))]
        public async Task<IHttpActionResult> CreatePAAuthirizationEmployeeMapping(PAAuthorizationEmployeeMappingModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            status = await _paBusenessAcess.CreatePAAuthirizationEmployeeMapping(model);
            return Ok(status);
        }
        [Route("GetMappingEmployee")]
        [ResponseType(typeof(PAAuthorizationEmployeeMappingModel))]
        public async Task<IHttpActionResult> GetMappingEmployee(PAAuthorizationLimitModel model)
        {
            PAAuthorizationEmployeeMappingModel status = new PAAuthorizationEmployeeMappingModel();
            status = await _paBusenessAcess.GetMappingEmployee(model);
            return Ok(status);
        }
        [Route("CreatePACreditDaysmaster")]
        [ResponseType(typeof(statuscheckmodel))]
        public async Task<IHttpActionResult> CreatePACreditDaysmaster(PACreditDaysMasterModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            status = await _paBusenessAcess.CreatePACreditDaysmaster(model);
            return Ok(status);
        }
        [Route("GetCreditdaysMasterByID")]
        [ResponseType(typeof(PACreditDaysMasterModel))]
        public async Task<IHttpActionResult> GetCreditdaysMasterByID(int creditdaysid)
        {
            PACreditDaysMasterModel status = new PACreditDaysMasterModel();
            status = await _paBusenessAcess.GetCreditdaysMasterByID(creditdaysid);
            return Ok(status);
        }
        [Route("AssignCreditdaysToEmployee")]
        [ResponseType(typeof(statuscheckmodel))]
        public async Task<IHttpActionResult> AssignCreditdaysToEmployee(PACreditDaysApproverModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            status = await _paBusenessAcess.AssignCreditdaysToEmployee(model);
            return Ok(status);
        }
        [Route("RemovePAAuthorizationLimitsByID")]
        [ResponseType(typeof(statuscheckmodel))]
        public async Task<IHttpActionResult> RemovePAAuthorizationLimitsByID(int authid)
        {
            statuscheckmodel status = new statuscheckmodel();
            status = await _paBusenessAcess.RemovePAAuthorizationLimitsByID(authid);
            return Ok(status);
        }
        [Route("RemovePACreditDaysMaster")]
        [ResponseType(typeof(statuscheckmodel))]
        public async Task<IHttpActionResult> RemovePACreditDaysMaster(int creditid)
        {
            statuscheckmodel status = new statuscheckmodel();
            status = await _paBusenessAcess.RemovePACreditDaysMaster(creditid);
            return Ok(status);
        }
        [Route("GetPAAuthorizationLimitsByDeptId")]
        [ResponseType(typeof(List<PAAuthorizationLimitModel>))]
        public async Task<IHttpActionResult> GetPAAuthorizationLimitsByDeptId(int departmentid)
        {
            List<PAAuthorizationLimitModel> model = new List<PAAuthorizationLimitModel>();
            model = await _paBusenessAcess.GetPAAuthorizationLimitsByDeptId(departmentid);
            return Ok(model);
        }
        [HttpPost]
        [Route("RemovePACreditDaysApprover")]
        [ResponseType(typeof(statuscheckmodel))]
        public async Task<IHttpActionResult> RemovePACreditDaysApprover(EmployeemappingtocreditModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            status = await _paBusenessAcess.RemovePACreditDaysApprover(model);
            return Ok(model);
        }
        [Route("GetPACreditDaysApproverById")]
        [ResponseType(typeof(PACreditDaysApproverModel))]
        public async Task<IHttpActionResult> GetPACreditDaysApproverById(int ApprovalId)
        {
            PACreditDaysApproverModel status = new PACreditDaysApproverModel();
            status = await _paBusenessAcess.GetPACreditDaysApproverById(ApprovalId);
            return Ok(status);
        }

        [HttpPost]
        [Route("GetEmployeeMappings")]
        [ResponseType(typeof(EmployeModel))]
        public async Task<IHttpActionResult> GetEmployeeMappings(PAConfigurationModel model)
        {
            EmployeModel employee = new EmployeModel();
            employee = await _paBusenessAcess.GetEmployeeMappings(model);
            return Ok(employee);
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
            return Ok(this._paBusenessAcess.GetItemsByMasterIDs(masters));
        }
        [HttpGet]
        [Route("GetAllDepartments")]
        [ResponseType(typeof(List<DepartmentModel>))]
        public async Task<IHttpActionResult> GetAllDepartments()
        {
            List<DepartmentModel> model = new List<DepartmentModel>();
            model = await _paBusenessAcess.GetAllDepartments();
            return Ok(model);
        }
        [HttpGet]
        [Route("GetSlabsByDepartmentID/{DeptID}")]
        [ResponseType(typeof(List<PAAuthorizationLimitModel>))]
        public async Task<IHttpActionResult> GetSlabsByDepartmentID(int DeptID)
        {
            List<PAAuthorizationLimitModel> model = new List<PAAuthorizationLimitModel>();
            model = await _paBusenessAcess.GetSlabsByDepartmentID(DeptID);
            return Ok(model);
        }
        [HttpGet]
        [Route("GetAllEmployee")]
        [ResponseType(typeof(List<EmployeeModel>))]
        public async Task<IHttpActionResult> GetAllEmployee()
        {
            List<EmployeModel> model = new List<EmployeModel>();
            model = await _paBusenessAcess.GetAllEmployee();
            return Ok(model);
        }
        [HttpGet]
        [Route("GetAllCredits")]
        [ResponseType(typeof(List<PAAuthorizationLimitModel>))]
        public async Task<IHttpActionResult> GetAllCredits()
        {
            List<PAAuthorizationLimitModel> model = new List<PAAuthorizationLimitModel>();
            model = await _paBusenessAcess.GetAllCredits();
            return Ok(model);
        }
        [HttpGet]
        [Route("GetAllCreditDays")]
        [ResponseType(typeof(List<PACreditDaysMasterModel>))]
        public async Task<IHttpActionResult> GetAllCreditDays()
        {
            List<PACreditDaysMasterModel> model = new List<PACreditDaysMasterModel>();
            model = await _paBusenessAcess.GetAllCreditDays();
            return Ok(model);
        }
        [HttpGet]
        [Route("GetAllMprPAPurchaseModes")]
        [ResponseType(typeof(List<MPRPAPurchaseModesModel>))]
        public async Task<IHttpActionResult> GetAllMprPAPurchaseModes()
        {
            List<MPRPAPurchaseModesModel> model = new List<MPRPAPurchaseModesModel>();
            model = await _paBusenessAcess.GetAllMprPAPurchaseModes();
            return Ok(model);
        }
        [HttpGet]
        [Route("GetAllMprPAPurchaseTypes")]
        [ResponseType(typeof(List<MPRPAPurchaseTypesModel>))]
        public async Task<IHttpActionResult> GetAllMprPAPurchaseTypes()
        {
            List<MPRPAPurchaseTypesModel> model = new List<MPRPAPurchaseTypesModel>();
            model = await _paBusenessAcess.GetAllMprPAPurchaseTypes();
            return Ok(model);
        }
        [HttpPost]
        [Route("InsertPurchaseAuthorization")]
        [ResponseType(typeof(statuscheckmodel))]
        public async Task<IHttpActionResult> InsertPurchaseAuthorization(MPRPADetailsModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            status = await _paBusenessAcess.InsertPurchaseAuthorization(model);
            return Ok(status);
        }
        [HttpGet]
        [Route("GetMPRPADeatilsByPAID/{PID}")]
        [ResponseType(typeof(MPRPADetailsModel))]
        public async Task<IHttpActionResult> GetMPRPADeatilsByPAID(int PID)
        {
            MPRPADetailsModel model = new MPRPADetailsModel();
            model = await _paBusenessAcess.GetMPRPADeatilsByPAID(PID);
            return Ok(model);
        }
        [HttpGet]
        [Route("GetAllMPRPAList")]
        [ResponseType(typeof(List<MPRPADetailsModel>))]
        public async Task<IHttpActionResult> GetAllMPRPAList()
        {
            List<MPRPADetailsModel> model = new List<MPRPADetailsModel>();
            model = await _paBusenessAcess.GetAllMPRPAList();
            return Ok(model);
        }
        [HttpGet]
        [Route("GetAllPAFunctionalRoles")]
        [ResponseType(typeof(List<PAFunctionalRolesModel>))]
        public async Task<IHttpActionResult> GetAllPAFunctionalRoles()
        {
            List<PAFunctionalRolesModel> model = new List<PAFunctionalRolesModel>();
            model = await _paBusenessAcess.GetAllPAFunctionalRoles();
            return Ok(model);
        }
        [HttpGet]
        [Route("GetCreditSlabsandemployees")]
        [ResponseType(typeof(List<EmployeemappingtocreditModel>))]
        public async Task<IHttpActionResult> GetCreditSlabsandemployees()
        {
            List<EmployeemappingtocreditModel> model = new List<EmployeemappingtocreditModel>();
            model = await _paBusenessAcess.GetCreditSlabsandemployees();
            return Ok(model);
        }
        [HttpGet]
        [Route("GetPurchaseSlabsandMappedemployees")]
        [ResponseType(typeof(List<EmployeemappingtopurchaseModel>))]
        public async Task<IHttpActionResult> GetPurchaseSlabsandMappedemployees()
        {
            List<EmployeemappingtopurchaseModel> model = new List<EmployeemappingtopurchaseModel>();
            model = await _paBusenessAcess.GetPurchaseSlabsandMappedemployees();
            return Ok(model);
        }
        [HttpPost]
        [Route("RemovePurchaseApprover")]
        [ResponseType(typeof(statuscheckmodel))]
        public async Task<IHttpActionResult> RemovePurchaseApprover(EmployeemappingtopurchaseModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            status = await _paBusenessAcess.RemovePurchaseApprover(model);
            return Ok(model);
        }
        [HttpGet]
        [Route("GetAllProjectManagers")]
        [ResponseType(typeof(List<ProjectManagerModel>))]
        public async Task<IHttpActionResult> GetAllProjectManagers()
        {
            List<ProjectManagerModel> model = new List<ProjectManagerModel>();
            model = await _paBusenessAcess.LoadAllProjectManagers();
            return Ok(model);
        }
        [HttpPost]
        [Route("LoadVendorByMprDetailsId")]
        [ResponseType(typeof(List<VendormasterModel>))]
        public async Task<IHttpActionResult> LoadVendorByMprDetailsId(List<int?> MPRItemDetailsid)
        {
            List<VendormasterModel> model = new List<VendormasterModel>();
            model = await _paBusenessAcess.LoadVendorByMprDetailsId(MPRItemDetailsid);
            return Ok(model);
        }
        [HttpGet]
        [Route("GetAllApproversList")]
        [ResponseType(typeof(List<MPRPAApproversModel>))]
        public async Task<IHttpActionResult> GetAllApproversList()
        {
            List<MPRPAApproversModel> model = new List<MPRPAApproversModel>();
            model = await _paBusenessAcess.GetAllApproversList();
            return Ok(model);
        }
        [HttpPost]
        [Route("GetMprApproverDetailsBySearch")]
        [ResponseType(typeof(List<GetmprApproverdeatil>))]
        public async Task<IHttpActionResult> GetMprApproverDetailsBySearch(PAApproverDetailsInputModel model)
        {
            List<GetmprApproverdeatil> details = new List<GetmprApproverdeatil>();
            details = await _paBusenessAcess.GetMprApproverDetailsBySearch(model);
            return Ok(details);
        }
        [HttpPost]
        [Route("UpdateMprpaApproverStatus")]
        [ResponseType(typeof(statuscheckmodel))]
        public async Task<IHttpActionResult> UpdateMprpaApproverStatus(MPRPAApproversModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            status = await _paBusenessAcess.UpdateMprpaApproverStatus(model);
            return Ok(model);
        }
        [HttpPost]
        [Route("getrfqtermsbyrevisionid")]
        [ResponseType(typeof(List<DisplayRfqTermsByRevisionId>))]
        public async Task<IHttpActionResult> getrfqtermsbyrevisionid(List<int> RevisionId)
        {
            List<DisplayRfqTermsByRevisionId> details = new List<DisplayRfqTermsByRevisionId>();
            details = await _paBusenessAcess.getrfqtermsbyrevisionid(RevisionId);
            return Ok(details);
        }
        [HttpPost]
        [Route("GetPurchaseSlabsandMappedemployeesByDeptId")]
        [ResponseType(typeof(List<Employeemappingtopurchase>))]
        public async Task<IHttpActionResult> GetPurchaseSlabsandMappedemployeesByDeptId(EmployeeFilterModel model)
        {
            List<Employeemappingtopurchase> purchase = new List<Employeemappingtopurchase>();
            purchase = await _paBusenessAcess.GetPurchaseSlabsandMappedemployeesByDeptId(model);
            return Ok(purchase);
        }
        [HttpPost]
        [Route("InsertPaitems")]
        [ResponseType(typeof(statuscheckmodel))]
        public async Task<IHttpActionResult> InsertPaitems(ItemsViewModel paitem)
        {
            statuscheckmodel model = new statuscheckmodel();
            model = await _paBusenessAcess.InsertPaitems(paitem);
            return Ok(model);
        }
        [HttpGet]
        [Route("GetAllMappedSlabs")]
        [ResponseType(typeof(List<GetMappedSlab>))]
        public async Task<IHttpActionResult> GetAllMappedSlabs()
        {
            List<GetMappedSlab> model = new List<GetMappedSlab>();
            model = await _paBusenessAcess.GetAllMappedSlabs();
            return Ok(model);
        }
        [HttpPost]
        [Route("RemoveMappedSlab")]
        [ResponseType(typeof(statuscheckmodel))]
        public async Task<IHttpActionResult> RemoveMappedSlab(PAAuthorizationLimitModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            status = await _paBusenessAcess.RemoveMappedSlab(model);
            return Ok(status);
        }
        [HttpPost]
        [Route("getMprPaDetailsBySearch")]
        [ResponseType(typeof(List<GetMprPaDetailsByFilter>))]
        public async Task<IHttpActionResult> getMprPaDetailsBySearch(PADetailsModel model)
        {
            List<GetMprPaDetailsByFilter> filter = new List<GetMprPaDetailsByFilter>();
            filter = await _paBusenessAcess.getMprPaDetailsBySearch(model);
            return Ok(filter);
        }
    }
}