using SCMModels.RFQModels;
using SCMModels.SCMModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BALayer.RFQ
{
	public interface IRFQBA
	{

		List<RFQQuoteView> getRFQItems(int RevisionId);


        //rfqmodule
        Task<RFQMasterModel> GetRFQById(int id);
        Task<statuscheckmodel> CreateRfQ(RfqRevisionModel model);
        //Task<List<RfqRevisionModel>> GetAllRFQs();
        Task<List<RFQMasterModel>> getallrfqlist();
        Task<RfqRevisionModel> GetRfqDetailsById(int revisionId);
        Task<statuscheckmodel> UpdateRfqRevision(RfqRevisionModel model);
        Task<statuscheckmodel> UpdateRfqItemByBulk(RfqItemModel model);
        Task<statuscheckmodel> UpdateSingleRfqItem(RfqItemModel model);
        Task<statuscheckmodel> UpdateBulkRfqRevision(RfqRevisionModel model);
        statuscheckmodel DeleteRfqById(int id);
        statuscheckmodel DeleteRfqRevisionbyId(int id);
        statuscheckmodel DeleteRfqItemById(int id);
        statuscheckmodel DeleteBulkItemsByItemId(List<int> id);
        statuscheckmodel InsertDocument(RfqDocumentsModel model);
        statuscheckmodel CommunicationAdd(RfqCommunicationModel model);
        Task<List<RfqItemModel>> GetItemsByRevisionId(int id);
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
    }
}
