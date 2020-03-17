using DALayer.RFQ;
using DALayer.PurchaseAuthorization;
using SCMModels.MPRMasterModels;
using SCMModels.RFQModels;
using SCMModels.SCMModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using SCMModels;

namespace BALayer.PurchaseAuthorization
{
   public class PurchaseAuthorizationBA:IPurchaseAuthorizationBA
    {
        public readonly IPurchaseAuthorizationDA _purchaseDataAcess;
        public PurchaseAuthorizationBA(IPurchaseAuthorizationDA purchase)
        {
            this._purchaseDataAcess = purchase;
        }
        public async Task<statuscheckmodel> InsertPAAuthorizationLimits(PAAuthorizationLimitModel model)
        {
            return await _purchaseDataAcess.InsertPAAuthorizationLimits(model);
        }

        public async Task<PAAuthorizationLimitModel> GetPAAuthorizationLimitById(int deptid)
        {
            return await _purchaseDataAcess.GetPAAuthorizationLimitById(deptid);
        }

        public async Task<statuscheckmodel> CreatePAAuthirizationEmployeeMapping(PAAuthorizationEmployeeMappingModel model)
        {
            return await _purchaseDataAcess.CreatePAAuthirizationEmployeeMapping(model);
        }

        public async Task<PAAuthorizationEmployeeMappingModel> GetMappingEmployee(PAAuthorizationLimitModel limit)
        {
            return await _purchaseDataAcess.GetMappingEmployee(limit);
        }

        public async Task<statuscheckmodel> CreatePACreditDaysmaster(PACreditDaysMasterModel model)
        {
            return await _purchaseDataAcess.CreatePACreditDaysmaster(model);
        }

        public async Task<PACreditDaysMasterModel> GetCreditdaysMasterByID(int creditdaysid)
        {
            return await _purchaseDataAcess.GetCreditdaysMasterByID(creditdaysid);
        }

        public async Task<statuscheckmodel> AssignCreditdaysToEmployee(PACreditDaysApproverModel model)
        {
            return await _purchaseDataAcess.AssignCreditdaysToEmployee(model);
        }

        public async Task<statuscheckmodel> RemovePAAuthorizationLimitsByID(int authid)
        {
            return await _purchaseDataAcess.RemovePAAuthorizationLimitsByID(authid);
        }

        public async Task<statuscheckmodel> RemovePACreditDaysMaster(int creditid)
        {
            return await _purchaseDataAcess.RemovePACreditDaysMaster(creditid);
        }

        public async Task<List<PAAuthorizationLimitModel>> GetPAAuthorizationLimitsByDeptId(int departmentid)
        {
            return await _purchaseDataAcess.GetPAAuthorizationLimitsByDeptId(departmentid);
        }

        public async Task<statuscheckmodel> RemovePACreditDaysApprover(EmployeemappingtocreditModel model)
        {
            return await _purchaseDataAcess.RemovePACreditDaysApprover(model);
        }

        public async Task<PACreditDaysApproverModel> GetPACreditDaysApproverById(int ApprovalId)
        {
            return await _purchaseDataAcess.GetPACreditDaysApproverById(ApprovalId);
        }

        public async Task<EmployeModel> GetEmployeeMappings(PAConfigurationModel model)
        {
            return await _purchaseDataAcess.GetEmployeeMappings(model);
        }

        public async Task<statuscheckmodel> RemovePurchaseApprover(EmployeemappingtopurchaseModel model)
        {
            return await _purchaseDataAcess.RemovePurchaseApprover(model);
        }

        public List<LoadItemsByID> GetItemsByMasterIDs(PADetailsModel masters)
        {
            return _purchaseDataAcess.GetItemsByMasterIDs(masters);
        }

        public async Task<List<DepartmentModel>> GetAllDepartments()
        {
            return await _purchaseDataAcess.GetAllDepartments();
        }

        public async Task<List<PAAuthorizationLimitModel>> GetSlabsByDepartmentID(int DeptID)
        {
            return await _purchaseDataAcess.GetSlabsByDepartmentID(DeptID);
        }

        public async Task<List<EmployeModel>> GetAllEmployee()
        {
            return await _purchaseDataAcess.GetAllEmployee();
        }

        public async Task<List<PAAuthorizationLimitModel>> GetAllCredits()
        {
            return await _purchaseDataAcess.GetAllCredits();
        }

        public async Task<List<PACreditDaysMasterModel>> GetAllCreditDays()
        {
            return await _purchaseDataAcess.GetAllCreditDays();
        }

        public async Task<List<MPRPAPurchaseModesModel>> GetAllMprPAPurchaseModes()
        {
            return await _purchaseDataAcess.GetAllMprPAPurchaseModes();
        }

        public async Task<List<MPRPAPurchaseTypesModel>> GetAllMprPAPurchaseTypes()
        {
            return await _purchaseDataAcess.GetAllMprPAPurchaseTypes();
        }

        public async Task<statuscheckmodel> InsertPurchaseAuthorization(MPRPADetailsModel model)
        {
            return await _purchaseDataAcess.InsertPurchaseAuthorization(model);
        }

        public async Task<MPRPADetailsModel> GetMPRPADeatilsByPAID(int PID)
        {
            return await _purchaseDataAcess.GetMPRPADeatilsByPAID(PID);
        }

        public async Task<List<MPRPADetailsModel>> GetAllMPRPAList()
        {
            return await _purchaseDataAcess.GetAllMPRPAList();
        }

        public async Task<List<PAFunctionalRolesModel>> GetAllPAFunctionalRoles()
        {
            return await _purchaseDataAcess.GetAllPAFunctionalRoles();
        }

        public async Task<List<EmployeemappingtocreditModel>> GetCreditSlabsandemployees()
        {
            return await _purchaseDataAcess.GetCreditSlabsandemployees();
        }

        public async Task<List<EmployeemappingtopurchaseModel>> GetPurchaseSlabsandMappedemployees()
        {
            return await _purchaseDataAcess.GetPurchaseSlabsandMappedemployees();
        }

        public async Task<List<ProjectManagerModel>> LoadAllProjectManagers()
        {
            return await _purchaseDataAcess.LoadAllProjectManagers();
        }

        public async Task<List<VendormasterModel>> LoadVendorByMprDetailsId(List<int?> MPRItemDetailsid)
        {
            return await _purchaseDataAcess.LoadVendorByMprDetailsId(MPRItemDetailsid);
        }

        public async Task<List<MPRPAApproversModel>> GetAllApproversList()
        {
            return await _purchaseDataAcess.GetAllApproversList();
        }

        public async Task<List<GetmprApproverdeatil>> GetMprApproverDetailsBySearch(PAApproverDetailsInputModel model)
        {
            return await _purchaseDataAcess.GetMprApproverDetailsBySearch(model);
        }

        public async Task<statuscheckmodel> UpdateMprpaApproverStatus(MPRPAApproversModel model)
        {
            return await _purchaseDataAcess.UpdateMprpaApproverStatus(model);
        }

        public async Task<List<DisplayRfqTermsByRevisionId>> getrfqtermsbyrevisionid(List<int> RevisionId)
        {
            return await _purchaseDataAcess.getrfqtermsbyrevisionid(RevisionId);
        }

        public async Task<List<Employeemappingtopurchase>> GetPurchaseSlabsandMappedemployeesByDeptId(EmployeeFilterModel model)
        {
            return await _purchaseDataAcess.GetPurchaseSlabsandMappedemployeesByDeptId(model);
        }

        public async Task<statuscheckmodel> InsertPaitems(List<ItemsViewModel> paitem)
        {
            return await _purchaseDataAcess.InsertPaitems(paitem);
        }
        public async Task<List<GetMappedSlab>> GetAllMappedSlabs()
        {
            return await _purchaseDataAcess.GetAllMappedSlabs();
        }

        public async Task<statuscheckmodel> RemoveMappedSlab(PAAuthorizationLimitModel model)
        {
            return await _purchaseDataAcess.RemoveMappedSlab(model);
        }

        public async Task<List<GetMprPaDetailsByFilter>> getMprPaDetailsBySearch(PADetailsModel model)
        {
            return await _purchaseDataAcess.getMprPaDetailsBySearch(model);
        }

        public async Task<List<MPRDate>> GetPaStatusReports(PAReportInputModel model)
        {
            return await _purchaseDataAcess.GetPaStatusReports(model);
        }
    }
}
