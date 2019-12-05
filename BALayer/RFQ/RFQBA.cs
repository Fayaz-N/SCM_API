using DALayer.RFQ;
using SCMModels.MPRMasterModels;
using SCMModels.RFQModels;
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
		public List<RFQQuoteView> getRFQItems(int RevisionId)
		{
			return this._rfqDataAcess.getRFQItems(RevisionId);
		}
        public bool updateVendorQuotes(List<RFQQuoteView> RFQQuoteViewList)
        {
            return this._rfqDataAcess.updateVendorQuotes(RFQQuoteViewList);

        }

        ///rfqmodule
        public statuscheckmodel CommunicationAdd(RfqCommunicationModel model)
        {
            return _rfqDataAcess.CommunicationAdd(model);
        }

        public async Task<statuscheckmodel> CreateRfQ(RfqRevisionModel model)
        {
            return await _rfqDataAcess.CreateRfQ(model);
        }

        public statuscheckmodel DeleteBulkItemsByItemId(List<int> id)
        {
            return _rfqDataAcess.DeleteBulkItemsByItemId(id);
        }

        public statuscheckmodel DeleteRfqById(int id)
        {
            return _rfqDataAcess.DeleteRfqById(id);
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

        public async Task<List<RfqItemModel>> GetItemsByRevisionId(int id)
        {
            return await _rfqDataAcess.GetItemsByRevisionId(id);
        }

        public async Task<RFQMasterModel> GetRFQById(int id)
        {
            return await _rfqDataAcess.GetRFQById(id);
        }

        public Task<RfqRevisionModel> getrfqrevisionbyid(int id)
        {
            throw new NotImplementedException();
        }

        public statuscheckmodel InsertDocument(RfqDocumentsModel model)
        {
            return _rfqDataAcess.InsertDocument(model);
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
    }
}
