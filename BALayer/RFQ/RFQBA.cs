using DALayer.RFQ;
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

namespace BALayer.RFQ
{
    public class RFQBA : IRFQBA
    {
        public readonly IRFQDA _rfqDataAcess;
        public RFQBA(IRFQDA RFQDA)
        {
            this._rfqDataAcess = RFQDA;
        }
        public List<RFQQuoteView> getRFQItems(int RevisionId)
        {
            return this._rfqDataAcess.getRFQItems(RevisionId);
        }
        public bool updateVendorQuotes(List<RFQQuoteView> RFQQuoteViewList, List<YILTermsandCondition> termsList)
        {
            return this._rfqDataAcess.updateVendorQuotes(RFQQuoteViewList, termsList);

        }
        public DataSet getRFQCompareItems(int RevisionId)
        {
            return this._rfqDataAcess.getRFQCompareItems(RevisionId);

        }
        public bool rfqStatusUpdate(List<RFQQuoteView> vendorList)
        {
            return this._rfqDataAcess.rfqStatusUpdate(vendorList);

        }

        ///rfqmodule
        public statuscheckmodel CommunicationAdd(RfqCommunicationModel model)
        {
            return _rfqDataAcess.CommunicationAdd(model);
        }
        public string UpdateVendorCommunication(RfqCommunicationModel model)
        {
            return _rfqDataAcess.UpdateVendorCommunication(model);
        }
        public int addNewRfqRevision(int rfqrevisionId)
        {
            return _rfqDataAcess.addNewRfqRevision(rfqrevisionId);

        }

        public async Task<statuscheckmodel> CreateRfQ(RfqRevisionModel model)
        {
            return await _rfqDataAcess.CreateRfQ(model);
        }

        public statuscheckmodel DeleteBulkItemsByItemId(List<int> id)
        {
            return _rfqDataAcess.DeleteBulkItemsByItemId(id);
        }

        public statuscheckmodel DeleteRfqById(int rfqmasterid)
        {
            return _rfqDataAcess.DeleteRfqById(rfqmasterid);
        }

        public statuscheckmodel DeleteRfqItemById(int id)
        {
            return _rfqDataAcess.DeleteRfqItemById(id);
        }

        public statuscheckmodel DeleteRfqRevisionbyId(int id)
        {
            return _rfqDataAcess.DeleteRfqRevisionbyId(id);
        }

        public async Task<List<RfqRevisionModel>> GetAllrevisionRFQs()
        {
            return await _rfqDataAcess.GetAllrevisionRFQs();
        }

        public async Task<List<RFQMasterModel>> getallrfqlist()
        {
            return await _rfqDataAcess.getallrfqlist();
        }

        //public async Task<List<RFQRevision>> GetAllRFQs()
        //{
        //    return await _rfqdata.GetAllRFQs();
        //}

        public List<VendormasterModel> GetAllvendorList()
        {
            return _rfqDataAcess.GetAllvendorList();
        }

        public async Task<RfqItemModel> GetItemsByItemId(int id)
        {
            return await _rfqDataAcess.GetItemsByItemId(id);
        }

        public async Task<List<RfqItemModel>> GetItemsByRevisionId(int revisionid)
        {
            return await _rfqDataAcess.GetItemsByRevisionId(revisionid);
        }

        public async Task<RFQMasterModel> GetRFQById(int masterID)
        {
            return await _rfqDataAcess.GetRFQById(masterID);
        }

        public Task<RfqRevisionModel> getrfqrevisionbyid(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<statuscheckmodel> InsertDocument(RfqDocumentsModel model)
        {
            return await _rfqDataAcess.InsertDocument(model);
        }
        public async Task<statuscheckmodel> RemovePurchaseApprover(EmployeemappingtopurchaseModel model)
        {
            return await _rfqDataAcess.RemovePurchaseApprover(model);
        }

        public async Task<statuscheckmodel> UpdateBulkRfqRevision(RfqRevisionModel model)
        {
            return await _rfqDataAcess.UpdateBulkRfqRevision(model);
        }

        public statuscheckmodel UpdateRFQ(RFQMasterModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<statuscheckmodel> UpdateRfqItemByBulk(RfqItemModel model)
        {
            return await _rfqDataAcess.UpdateRfqItemByBulk(model);
        }

        public async Task<statuscheckmodel> UpdateRfqRevision(RfqRevisionModel model)
        {
            return await _rfqDataAcess.UpdateRfqRevision(model);
        }

        public async Task<statuscheckmodel> UpdateSingleRfqItem(RfqItemModel model)
        {
            return await _rfqDataAcess.UpdateSingleRfqItem(model);
        }

        public async Task<statuscheckmodel> CreateNewRfq(RFQMasterModel model)
        {
            return await _rfqDataAcess.CreateNewRfq(model);
        }

        public async Task<RfqRevisionModel> GetRfqDetailsById(int revisionId)
        {
            return await _rfqDataAcess.GetRfqDetailsById(revisionId);
        }
        public bool updateRfqDocStatus(List<RFQDocument> rfqDocs)
        {
            return _rfqDataAcess.updateRfqDocStatus(rfqDocs);
        }
        public async Task<VendormasterModel> GetvendorById(int id)
        {
            return await _rfqDataAcess.GetvendorById(id);
        }

        public async Task<statuscheckmodel> InsertVendorterms(VendorRfqtermModel vendor)
        {
            return await _rfqDataAcess.InsertVendorterms(vendor);
        }

        public async Task<statuscheckmodel> InsertRfqItemInfo(RfqItemModel model)
        {
            return await _rfqDataAcess.InsertRfqItemInfo(model);
        }

        public async Task<statuscheckmodel> DeleteRfqIteminfoByid(List<int> id)
        {
            return await _rfqDataAcess.DeleteRfqIteminfoByid(id);
        }

        public async Task<statuscheckmodel> DeleteRfqitemandinfosById(int id)
        {
            return await _rfqDataAcess.DeleteRfqitemandinfosById(id);
        }

        public async Task<statuscheckmodel> UpdateRfqItemInfoById(RfqItemInfoModel model)
        {
            return await _rfqDataAcess.UpdateRfqItemInfoById(model);
        }

        public async Task<RfqItemModel> GetRfqItemByMPrId(int id)
        {
            return await _rfqDataAcess.GetRfqItemByMPrId(id);
        }

        public async Task<statuscheckmodel> InsertSingleIteminfos(RfqItemInfoModel model)
        {
            return await _rfqDataAcess.InsertSingleIteminfos(model);
        }

        public async Task<statuscheckmodel> InsertBulkItemInfos(List<RfqItemInfoModel> model)
        {
            return await _rfqDataAcess.InsertBulkItemInfos(model);
        }

        public async Task<List<UnitMasterModel>> GetUnitMasterList()
        {
            return await _rfqDataAcess.GetUnitMasterList();
        }
        public async Task<statuscheckmodel> InsertRfqRemainder(RfqRemainderTrackingModel model)
        {
            return await _rfqDataAcess.InsertRfqRemainder(model);
        }

        public async Task<RfqRemainderTrackingModel> getrfqremaindersById(int id)
        {
            return await _rfqDataAcess.getrfqremaindersById(id);
        }

        public async Task<RfqVendorTermModel> getRfqVendorById(int id)
        {
            return await _rfqDataAcess.getRfqVendorById(id);
        }

        public async Task<statuscheckmodel> RemoveRfqVendorTermsById(int id)
        {
            return await _rfqDataAcess.RemoveRfqVendorTermsById(id);
        }

        public async Task<statuscheckmodel> RemoveVendorRfqByid(int id)
        {
            return await _rfqDataAcess.RemoveVendorRfqByid(id);
        }

        public async Task<statuscheckmodel> InsertNewCurrencyMaster(CurrencyMasterModel model)
        {
            return await _rfqDataAcess.InsertNewCurrencyMaster(model);
        }

        public async Task<statuscheckmodel> UpdateNewCurrencyMaster(CurrencyMasterModel model)
        {
            return await _rfqDataAcess.UpdateNewCurrencyMaster(model);
        }

        public async Task<statuscheckmodel> InsertCurrentCurrencyHistory(CurrencyHistoryModel model)
        {
            return await _rfqDataAcess.InsertCurrentCurrencyHistory(model);
        }

        public async Task<statuscheckmodel> UpdateCurrentCurrencyHistory(CurrencyHistoryModel model)
        {
            return await _rfqDataAcess.UpdateCurrentCurrencyHistory(model);
        }

        public async Task<List<CurrencyMasterModel>> GetAllMasterCurrency()
        {
            return await _rfqDataAcess.GetAllMasterCurrency();
        }

        public async Task<CurrencyMasterModel> GetMasterCurrencyById(int currencyId)
        {
            return await _rfqDataAcess.GetMasterCurrencyById(currencyId);
        }

        public async Task<statuscheckmodel> RemoveMasterCurrencyById(int currencyId)
        {
            return await _rfqDataAcess.RemoveMasterCurrencyById(currencyId);
        }

        public async Task<CurrencyHistoryModel> GetcurrencyHistoryById(int currencyId)
        {
            return await _rfqDataAcess.GetcurrencyHistoryById(currencyId);
        }

        public async Task<statuscheckmodel> Insertrfqvendorterms(RfqVendorTermModel model)
        {
            return await _rfqDataAcess.Insertrfqvendorterms(model);
        }

        public async Task<MPRBuyerGroupModel> GetMPRBuyerGroupsById(int id)
        {
            return await _rfqDataAcess.GetMPRBuyerGroupsById(id);
        }

        public async Task<List<MPRBuyerGroupModel>> GetAllMPRBuyerGroups()
        {
            return await _rfqDataAcess.GetAllMPRBuyerGroups();
        }
        public async Task<List<MPRApproversViewModel>> GetAllMPRApprovers()
        {
            return await _rfqDataAcess.GetAllMPRApprovers();
        }
        public async Task<statuscheckmodel> InsertMprBuyerGroups(MPRBuyerGroupModel model)
        {
            return await _rfqDataAcess.InsertMprBuyerGroups(model);
        }
        public async Task<statuscheckmodel> UpdateMprBuyerGroups(MPRBuyerGroupModel model)
        {
            return await _rfqDataAcess.UpdateMprBuyerGroups(model);
        }
        public async Task<statuscheckmodel> InsertMPRApprover(MPRApproverModel model)
        {
            return await _rfqDataAcess.InsertMPRApprover(model);
        }
        public async Task<MPRApproverModel> GetMPRApprovalsById(int id)
        {
            return await _rfqDataAcess.GetMPRApprovalsById(id);
        }

        public async Task<List<MPRApproverModel>> GetAllMPRApprovals()
        {
            return await _rfqDataAcess.GetAllMPRApprovals();
        }

        public async Task<List<MPRDepartmentModel>> GetAllMPRDepartments()
        {
            return await _rfqDataAcess.GetAllMPRDepartments();
        }

        public async Task<MPRDepartmentModel> GetMPRDepartmentById(int id)
        {
            return await _rfqDataAcess.GetMPRDepartmentById(id);
        }

        public async Task<List<MPRDispatchLocationModel>> GetAllMPRDispatchLocations()
        {
            return await _rfqDataAcess.GetAllMPRDispatchLocations();
        }

        public async Task<MPRDispatchLocationModel> GetMPRDispatchLocationById(int id)
        {
            return await _rfqDataAcess.GetMPRDispatchLocationById(id);
        }

        public async Task<List<MPRCustomsDutyModel>> GetAllCustomDuty()
        {
            return await _rfqDataAcess.GetAllCustomDuty();
        }

        public async Task<statuscheckmodel> InsertYILTerms(YILTermsandConditionModel model)
        {
            return await _rfqDataAcess.InsertYILTerms(model);
        }

        public async Task<statuscheckmodel> InsertYILTermsGroup(YILTermsGroupModel model)
        {
            return await _rfqDataAcess.InsertYILTermsGroup(model);
        }

        public async Task<statuscheckmodel> InsertRFQTerms(RFQTermsModel model)
        {
            return await _rfqDataAcess.InsertRFQTerms(model);
        }

        public async Task<RFQTermsModel> GetRfqTermsById(int termsid)
        {
            return await _rfqDataAcess.GetRfqTermsById(termsid);
        }

        public async Task<YILTermsandConditionModel> GetYILTermsByBuyerGroupID(int id)
        {
            return await _rfqDataAcess.GetYILTermsByBuyerGroupID(id);
        }

        public async Task<YILTermsGroupModel> GetYILTermsGroupById(int id)
        {
            return await _rfqDataAcess.GetYILTermsGroupById(id);
        }

        public async Task<RfqItemModel> GetItemByItemId(int id)
        {
            return await _rfqDataAcess.GetItemByItemId(id);
        }

        public async Task<List<RFQMasterModel>> GetRfqByVendorId(int vendorid)
        {
            return await _rfqDataAcess.GetRfqByVendorId(vendorid);
        }

        public async Task<statuscheckmodel> UpdateRFQTerms(RFQTermsModel model)
        {
            return await _rfqDataAcess.UpdateRFQTerms(model);
        }
        public List<RFQListView> getRFQList(rfqFilterParams Rfqfilterparams)
        {
            return _rfqDataAcess.getRFQList(Rfqfilterparams);
        }
        //pa authorization
        public async Task<statuscheckmodel> InsertPAAuthorizationLimits(PAAuthorizationLimitModel model)
        {
            return await _rfqDataAcess.InsertPAAuthorizationLimits(model);
        }

        public async Task<PAAuthorizationLimitModel> GetPAAuthorizationLimitById(int deptid)
        {
            return await _rfqDataAcess.GetPAAuthorizationLimitById(deptid);
        }

        public async Task<statuscheckmodel> CreatePAAuthirizationEmployeeMapping(PAAuthorizationEmployeeMappingModel model)
        {
            return await _rfqDataAcess.CreatePAAuthirizationEmployeeMapping(model);
        }

        public async Task<PAAuthorizationEmployeeMappingModel> GetMappingEmployee(PAAuthorizationLimitModel limit)
        {
            return await _rfqDataAcess.GetMappingEmployee(limit);
        }

        public async Task<statuscheckmodel> CreatePACreditDaysmaster(PACreditDaysMasterModel model)
        {
            return await _rfqDataAcess.CreatePACreditDaysmaster(model);
        }

        public async Task<PACreditDaysMasterModel> GetCreditdaysMasterByID(int creditdaysid)
        {
            return await _rfqDataAcess.GetCreditdaysMasterByID(creditdaysid);
        }

        public async Task<statuscheckmodel> AssignCreditdaysToEmployee(PACreditDaysApproverModel model)
        {
            return await _rfqDataAcess.AssignCreditdaysToEmployee(model);
        }

        public async Task<statuscheckmodel> RemovePAAuthorizationLimitsByID(int authid)
        {
            return await _rfqDataAcess.RemovePAAuthorizationLimitsByID(authid);
        }

        public async Task<statuscheckmodel> RemovePACreditDaysMaster(int creditid)
        {
            return await _rfqDataAcess.RemovePACreditDaysMaster(creditid);
        }

        public async Task<List<PAAuthorizationLimitModel>> GetPAAuthorizationLimitsByDeptId(int departmentid)
        {
            return await _rfqDataAcess.GetPAAuthorizationLimitsByDeptId(departmentid);
        }

        public async Task<statuscheckmodel> RemovePACreditDaysApprover(EmployeemappingtocreditModel model)
        {
            return await _rfqDataAcess.RemovePACreditDaysApprover(model);
        }

        public async Task<PACreditDaysApproverModel> GetPACreditDaysApproverById(int ApprovalId)
        {
            return await _rfqDataAcess.GetPACreditDaysApproverById(ApprovalId);
        }

        public async Task<EmployeModel> GetEmployeeMappings(PAConfigurationModel model)
        {
            return await _rfqDataAcess.GetEmployeeMappings(model);
        }

        public async Task<List<RfqItemModel>> GetRfqItemsByRevisionId(int revisionid)
        {
            return await _rfqDataAcess.GetRfqItemsByRevisionId(revisionid);
        }

        public List<LoadItemsByID> GetItemsByMasterIDs(PADetailsModel masters)
        {
            return _rfqDataAcess.GetItemsByMasterIDs(masters);
        }

        public async Task<List<DepartmentModel>> GetAllDepartments()
        {
            return await _rfqDataAcess.GetAllDepartments();
        }

        public async Task<List<PAAuthorizationLimitModel>> GetSlabsByDepartmentID(int DeptID)
        {
            return await _rfqDataAcess.GetSlabsByDepartmentID(DeptID);
        }

        public async Task<List<EmployeModel>> GetAllEmployee()
        {
            return await _rfqDataAcess.GetAllEmployee();
        }

        public async Task<List<PAAuthorizationLimitModel>> GetAllCredits()
        {
            return await _rfqDataAcess.GetAllCredits();
        }

        public async Task<List<PACreditDaysMasterModel>> GetAllCreditDays()
        {
            return await _rfqDataAcess.GetAllCreditDays();
        }

        public async Task<List<MPRPAPurchaseModesModel>> GetAllMprPAPurchaseModes()
        {
            return await _rfqDataAcess.GetAllMprPAPurchaseModes();
        }

        public async Task<List<MPRPAPurchaseTypesModel>> GetAllMprPAPurchaseTypes()
        {
            return await _rfqDataAcess.GetAllMprPAPurchaseTypes();
        }

        public async Task<statuscheckmodel> InsertPurchaseAuthorization(MPRPADetailsModel model)
        {
            return await _rfqDataAcess.InsertPurchaseAuthorization(model);
        }

        public async Task<MPRPADetailsModel> GetMPRPADeatilsByPAID(int PID)
        {
            return await _rfqDataAcess.GetMPRPADeatilsByPAID(PID);
        }

        public async Task<List<MPRPADetailsModel>> GetAllMPRPAList()
        {
            return await _rfqDataAcess.GetAllMPRPAList();
        }

        public async Task<List<PAFunctionalRolesModel>> GetAllPAFunctionalRoles()
        {
            return await _rfqDataAcess.GetAllPAFunctionalRoles();
        }

        public async Task<List<EmployeemappingtocreditModel>> GetCreditSlabsandemployees()
        {
            return await _rfqDataAcess.GetCreditSlabsandemployees();
        }

        public async Task<List<EmployeemappingtopurchaseModel>> GetPurchaseSlabsandMappedemployees()
        {
            return await _rfqDataAcess.GetPurchaseSlabsandMappedemployees();
        }

        public async Task<List<ProjectManagerModel>> LoadAllProjectManagers()
        {
            return await _rfqDataAcess.LoadAllProjectManagers();
        }

        public async Task<List<VendormasterModel>> LoadVendorByMprDetailsId(List<int?> MPRItemDetailsid)
        {
            return await _rfqDataAcess.LoadVendorByMprDetailsId(MPRItemDetailsid);
        }

        public async Task<List<MPRPAApproversModel>> GetAllApproversList()
        {
            return await _rfqDataAcess.GetAllApproversList();
        }

        public async Task<List<GetmprApproverdeatil>> GetMprApproverDetailsBySearch(PAApproverDetailsInputModel model)
        {
            return await _rfqDataAcess.GetMprApproverDetailsBySearch(model);
        }

        public async Task<statuscheckmodel> UpdateMprpaApproverStatus(MPRPAApproversModel model)
        {
            return await _rfqDataAcess.UpdateMprpaApproverStatus(model);
        }

        public async Task<List<DisplayRfqTermsByRevisionId>> getrfqtermsbyrevisionid(List<int> RevisionId)
        {
            return await _rfqDataAcess.getrfqtermsbyrevisionid(RevisionId);
        }

        public async Task<List<EmployeemappingtopurchaseModel>> GetPurchaseSlabsandMappedemployeesByDeptId(int deptid)
        {
            return await _rfqDataAcess.GetPurchaseSlabsandMappedemployeesByDeptId(deptid);
        }

        public async Task<statuscheckmodel> InsertPaitems(ItemsViewModel paitem)
        {
            return await _rfqDataAcess.InsertPaitems(paitem);
        }
    }
}
