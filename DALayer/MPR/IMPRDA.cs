using SCMModels;
using SCMModels.SCMModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALayer.MPR
{
    public interface IMPRDA
    {

        DataTable getDBMastersList(DynamicSearchResult Result);
        bool addDataToDBMasters(DynamicSearchResult Result);
        bool updateDataToDBMasters(DynamicSearchResult Result);
        MPRRevision updateMPR(MPRRevision mpr);
        int addNewVendor(VendorMaster vendor);

        DataTable GetListItems(DynamicSearchResult Result);
        bool deleteMPRDocument(MPRDocument mprDocument);
        bool deleteMPRItemInfo(MPRItemInfo mprItemInfo);
        bool deleteMPRVendor(MPRVendorDetail mprVendor);
        bool deleteMPRDocumentation(MPRDocumentation MPRDocumentation);
        MPRRevision getMPRRevisionDetails(int RevisionId);
        List<MPRRevisionDetail> getMPRList(mprFilterParams mprfilterparams);
        int getMPRPendingListCnt(string preparedBy);
        List<Employee> getEmployeeList();
        List<MPRRevisionDetail> getMPRRevisionList(int RequisitionId);
        MPRRevision statusUpdate(MPRStatusUpdate mprStatus);
        List<SCMStatu> getStatusList();
        List<UserPermission> getAccessList(int RoleId);
        bool updateMPRVendor(List<MPRVendorDetail> MPRVendorDetails, int RevisionId);
        void updateMprstatusTrack(MPRStatusTrack mprStatusTrackDetails);

    }
}
