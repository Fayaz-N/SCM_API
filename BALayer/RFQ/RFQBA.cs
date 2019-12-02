using DALayer.RFQ;
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
    }
}
