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

namespace DALayer.PurchaseAuthorization
{
   public interface IPurchaseAuthorizationDA
    {
        Task<statuscheckmodel> InsertPAAuthorizationLimits(PAAuthorizationLimitModel model);
        Task<PAAuthorizationLimitModel> GetPAAuthorizationLimitById(int deptid);
        Task<statuscheckmodel> CreatePAAuthirizationEmployeeMapping(PAAuthorizationEmployeeMappingModel model);
        Task<PAAuthorizationEmployeeMappingModel> GetMappingEmployee(PAAuthorizationLimitModel limit);
        Task<statuscheckmodel> CreatePACreditDaysmaster(PACreditDaysMasterModel model);
        Task<PACreditDaysMasterModel> GetCreditdaysMasterByID(int creditdaysid);
        Task<statuscheckmodel> AssignCreditdaysToEmployee(PACreditDaysApproverModel model);
        Task<statuscheckmodel> RemovePAAuthorizationLimitsByID(int authid);
        Task<statuscheckmodel> RemovePACreditDaysMaster(int creditid);
        Task<List<PAAuthorizationLimitModel>> GetPAAuthorizationLimitsByDeptId(int departmentid);
        Task<statuscheckmodel> RemovePACreditDaysApprover(EmployeemappingtocreditModel model);
        Task<statuscheckmodel> RemovePurchaseApprover(EmployeemappingtopurchaseModel model);
        Task<PACreditDaysApproverModel> GetPACreditDaysApproverById(int ApprovalId);
        Task<EmployeModel> GetEmployeeMappings(PAConfigurationModel model);
        DataSet GetEmployeeMappings1(PAConfigurationModel model);

        //Task<List<LoadItemsByID>> GetItemsByMasterIDs(PADetailsModel masters);
        List<LoadItemsByID> GetItemsByMasterIDs(PADetailsModel masters);
        Task<List<DepartmentModel>> GetAllDepartments();
        Task<List<PAAuthorizationLimitModel>> GetSlabsByDepartmentID(int DeptID);
        Task<List<EmployeModel>> GetAllEmployee();
        Task<List<PAAuthorizationLimitModel>> GetAllCredits();
        Task<List<PACreditDaysMasterModel>> GetAllCreditDays();
        Task<List<MPRPAPurchaseModesModel>> GetAllMprPAPurchaseModes();
        Task<List<MPRPAPurchaseTypesModel>> GetAllMprPAPurchaseTypes();
        Task<statuscheckmodel> InsertPurchaseAuthorization(MPRPADetailsModel model);
        Task<MPRPADetailsModel> GetMPRPADeatilsByPAID(int PID);
        Task<List<MPRPADetailsModel>> GetAllMPRPAList();
        Task<List<PAFunctionalRolesModel>> GetAllPAFunctionalRoles();
        Task<List<EmployeemappingtocreditModel>> GetCreditSlabsandemployees();
        Task<List<EmployeemappingtopurchaseModel>> GetPurchaseSlabsandMappedemployees();
        Task<List<ProjectManagerModel>> LoadAllProjectManagers();
        Task<List<VendormasterModel>> LoadVendorByMprDetailsId(List<int?> MPRItemDetailsid);
        Task<List<MPRPAApproversModel>> GetAllApproversList();
        Task<List<GetmprApproverdeatil>> GetMprApproverDetailsBySearch(PAApproverDetailsInputModel model);
        Task<statuscheckmodel> UpdateMprpaApproverStatus(MPRPAApproversModel model);
        Task<List<DisplayRfqTermsByRevisionId>> getrfqtermsbyrevisionid(List<int> RevisionId);
        Task<List<Employeemappingtopurchase>> GetPurchaseSlabsandMappedemployeesByDeptId(EmployeeFilterModel model);
        Task<statuscheckmodel> InsertPaitems(List<ItemsViewModel> paitem);
        Task<List<GetMappedSlab>> GetAllMappedSlabs();
        Task<statuscheckmodel> RemoveMappedSlab(PAAuthorizationLimitModel model);
        Task<List<GetMprPaDetailsByFilter>> getMprPaDetailsBySearch(PADetailsModel model);
        Task<List<MPRDate>> GetPaStatusReports(PAReportInputModel model);
        Task<statuscheckmodel> UpdateApproverforRequest(MPRPAApproversModel model);
    }
}
