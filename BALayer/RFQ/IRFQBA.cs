using SCMModels.MPRMasterModels;
using SCMModels.RFQModels;
using SCMModels.SCMModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;
namespace BALayer.RFQ
{
    public interface IRFQBA
    {

        List<RFQQuoteView> getRFQItems(int RevisionId);
        bool updateVendorQuotes(List<RFQQuoteView> RFQQuoteViewList, List<YILTermsandCondition> termsList);
        DataTable getRFQCompareItems(int RevisionId);
        bool rfqStatusUpdate(List<RFQItem> RFQQuoteViewList);
        //rfqmodule
        Task<RFQMasterModel> GetRFQById(int masterID);
        Task<statuscheckmodel> CreateRfQ(RfqRevisionModel model);
        //Task<List<RfqRevisionModel>> GetAllRFQs();
        Task<List<RFQMasterModel>> getallrfqlist();
        Task<RfqRevisionModel> GetRfqDetailsById(int revisionId);
        Task<statuscheckmodel> UpdateRfqRevision(RfqRevisionModel model);
        Task<statuscheckmodel> UpdateRfqItemByBulk(RfqItemModel model);
        Task<statuscheckmodel> UpdateSingleRfqItem(RfqItemModel model);
        Task<statuscheckmodel> UpdateBulkRfqRevision(RfqRevisionModel model);
        statuscheckmodel DeleteRfqById(int rfqmasterid);
        statuscheckmodel DeleteRfqRevisionbyId(int id);
        statuscheckmodel DeleteRfqItemById(int id);
        statuscheckmodel DeleteBulkItemsByItemId(List<int> id);
        statuscheckmodel InsertDocument(RfqDocumentsModel model);
        statuscheckmodel CommunicationAdd(RfqCommunicationModel model);
        Task<List<RfqItemModel>> GetItemsByRevisionId(int revisionid);
        Task<List<RfqRevisionModel>> GetAllrevisionRFQs();
        Task<RfqItemModel> GetItemsByItemId(int id);
        List<VendormasterModel> GetAllvendorList();
        Task<statuscheckmodel> CreateNewRfq(RFQMasterModel model);
        Task<VendormasterModel> GetvendorById(int id);
        Task<statuscheckmodel> InsertVendorterms(VendorRfqtermModel vendor);
        Task<statuscheckmodel> InsertRfqItemInfo(RfqItemModel model);
        Task<statuscheckmodel> DeleteRfqIteminfoByid(List<int> id);
        Task<statuscheckmodel> DeleteRfqitemandinfosById(int id);
        Task<statuscheckmodel> UpdateRfqItemInfoById(RfqItemInfoModel model);
        Task<RfqItemModel> GetRfqItemByMPrId(int id);
        Task<statuscheckmodel> InsertSingleIteminfos(RfqItemInfoModel model);
        Task<statuscheckmodel> InsertBulkItemInfos(List<RfqItemInfoModel> model);
        Task<List<UnitMasterModel>> GetUnitMasterList();
        Task<statuscheckmodel> InsertRfqRemainder(RfqRemainderTrackingModel model);
        Task<RfqRemainderTrackingModel> getrfqremaindersById(int id);
        Task<statuscheckmodel> Insertrfqvendorterms(RfqVendorTermModel model);
        Task<RfqVendorTermModel> getRfqVendorById(int id);
        Task<statuscheckmodel> RemoveRfqVendorTermsById(int id);
        Task<statuscheckmodel> RemoveVendorRfqByid(int id);
        Task<statuscheckmodel> InsertNewCurrencyMaster(CurrencyMasterModel model);
        Task<statuscheckmodel> UpdateNewCurrencyMaster(CurrencyMasterModel model);
        Task<statuscheckmodel> InsertCurrentCurrencyHistory(CurrencyHistoryModel model);
        Task<statuscheckmodel> UpdateCurrentCurrencyHistory(CurrencyHistoryModel model);
        Task<List<CurrencyMasterModel>> GetAllMasterCurrency();
        Task<CurrencyMasterModel> GetMasterCurrencyById(int currencyId);
        Task<statuscheckmodel> RemoveMasterCurrencyById(int currencyId);
        Task<CurrencyHistoryModel> GetcurrencyHistoryById(int currencyId);
        Task<MPRBuyerGroupModel> GetMPRBuyerGroupsById(int id);
        Task<List<MPRBuyerGroupModel>> GetAllMPRBuyerGroups();
        Task<MPRApproverModel> GetMPRApprovalsById(int id);
        Task<List<MPRApproverModel>> GetAllMPRApprovals();
        Task<List<MPRDepartmentModel>> GetAllMPRDepartments();
        Task<MPRDepartmentModel> GetMPRDepartmentById(int id);
        Task<List<MPRDispatchLocationModel>> GetAllMPRDispatchLocations();
        Task<MPRDispatchLocationModel> GetMPRDispatchLocationById(int id);
        Task<List<MPRCustomsDutyModel>> GetAllCustomDuty();
        Task<statuscheckmodel> InsertYILTerms(YILTermsandConditionModel model);
        Task<statuscheckmodel> InsertYILTermsGroup(YILTermsGroupModel model);
        Task<statuscheckmodel> InsertRFQTerms(RFQTermsModel model);
        Task<statuscheckmodel> UpdateRFQTerms(RFQTermsModel model);
        Task<YILTermsandConditionModel> GetYILTermsByBuyerGroupID(int id);
        Task<YILTermsGroupModel> GetYILTermsGroupById(int id);
        Task<RFQTermsModel> GetRfqTermsById(int termsid);
        Task<RfqItemModel> GetItemByItemId(int id);
        Task<List<RFQMasterModel>> GetRfqByVendorId(int vendorid);
    }
}
