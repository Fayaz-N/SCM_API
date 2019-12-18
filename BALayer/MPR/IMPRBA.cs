using SCMModels;
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
		DataTable GetListItems(DynamicSearchResult Result);
		bool deleteMPRDocument(MPRDocument mprDocument);
		bool deleteMPRItemInfo(MPRItemInfo mprItemInfo);
		bool deleteMPRVendor(MPRVendorDetail mprVendor);
		bool deleteMPRDocumentation(MPRDocumentation MPRDocumentation);
		MPRRevision getMPRRevisionDetails(int RevisionId);
		List<MPRRevisionDetail> getMPRList(mprFilterParams mprfilterparams);
		List<Employee> getEmployeeList();
		List<MPRRevisionDetail> getMPRRevisionList(int RequisitionId);
		MPRRevision statusUpdate(MPRStatusUpdate mprStatus);
		List<SCMStatu> getStatusList();
		List<MPRVendorDetail> updateMPRVendor(List<MPRVendorDetail> MPRVendorDetails, int RevisionId);

	}

}
