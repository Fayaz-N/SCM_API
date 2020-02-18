using SCMModels;
using SCMModels.RFQModels;
using SCMModels.SCMModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BALayer.MPR
{
	public interface IMPRBA
	{
		DataTable getDBMastersList(DynamicSearchResult Result);
		bool addDataToDBMasters(DynamicSearchResult Result);
		bool updateDataToDBMasters(DynamicSearchResult Result);
		MPRRevision updateMPR(MPRRevision mpr);
        MPRRevision copyMprRevision(MPRRevision mpr, bool repeatOrder);
        
        int addNewVendor(VendormasterModel vendor);
        DataTable GetListItems(DynamicSearchResult Result);
		bool deleteMPRDocument(MPRDocument mprDocument);
		bool deleteMPRItemInfo(MPRItemInfo mprItemInfo);
		bool deleteMPRVendor(MPRVendorDetail mprVendor);
		bool deleteMPRDocumentation(MPRDocumentation MPRDocumentation);
		MPRRevision getMPRRevisionDetails(int RevisionId);
		DataTable getMPRList(mprFilterParams mprfilterparams);
        int getMPRPendingListCnt(string preparedBy);
        List<Employee> getEmployeeList();
		List<MPRRevisionDetails_woItems> getMPRRevisionList(int RequisitionId);
		MPRRevision statusUpdate(MPRStatusUpdate mprStatus);
		List<SCMStatu> getStatusList();
        List<UserPermission> getAccessList(int RoleId);
        bool updateMPRVendor(List<MPRVendorDetail> MPRVendorDetails, int RevisionId);

	}

}
