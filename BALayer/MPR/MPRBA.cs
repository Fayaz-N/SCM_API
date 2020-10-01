using DALayer.MPR;
using SCMModels;
using SCMModels.RemoteModel;
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

	public class MPRBA : IMPRBA
	{
		public readonly IMPRDA _mprDataAcess;
		public MPRBA(IMPRDA MPRDA)
		{
			this._mprDataAcess = MPRDA;
		}
		public DataTable getDBMastersList(DynamicSearchResult Result)
		{
			return this._mprDataAcess.getDBMastersList(Result);

		}
		public bool addDataToDBMasters(DynamicSearchResult Result)
		{
			return this._mprDataAcess.addDataToDBMasters(Result);

		}
		public bool updateDataToDBMasters(DynamicSearchResult Result)
		{
			return this._mprDataAcess.updateDataToDBMasters(Result);

		}
		public MPRRevision updateMPR(MPRRevision mpr)
		{
			return this._mprDataAcess.updateMPR(mpr);

		}
		public MPRRevision copyMprRevision(MPRRevision mpr, bool repeatOrder, bool revise)
		{
			return this._mprDataAcess.copyMprRevision(mpr, repeatOrder, revise);
		}
		public int addNewVendor(VendormasterModel vendor)
		{
			return this._mprDataAcess.addNewVendor(vendor);
		}
		public DataTable GetListItems(DynamicSearchResult Result)
		{
			return this._mprDataAcess.GetListItems(Result);
		}
		public bool deleteMPRDocument(MPRDocument mprDocument)
		{
			return this._mprDataAcess.deleteMPRDocument(mprDocument);
		}

		public bool deleteMPRItemInfo(MPRItemInfo mprItemInfo)
		{
			return this._mprDataAcess.deleteMPRItemInfo(mprItemInfo);
		}
		public string addMprItemInfo(MPRItemInfo mprItemInfo)
		{
			return this._mprDataAcess.addMprItemInfo(mprItemInfo);
		}
		public bool deleteMPRVendor(MPRVendorDetail mprVendor)
		{
			return this._mprDataAcess.deleteMPRVendor(mprVendor);
		}

		public bool deleteMPRDocumentation(MPRDocumentation MPRDocumentation)
		{
			return this._mprDataAcess.deleteMPRDocumentation(MPRDocumentation);
		}
		public MPRRevision getMPRRevisionDetails(int RevisionId)
		{
			return this._mprDataAcess.getMPRRevisionDetails(RevisionId);
		}
		public DataTable getMPRList(mprFilterParams mprfilterparams)
		{
			return this._mprDataAcess.getMPRList(mprfilterparams);
		}
		public DataTable getSavingsReport(mprFilterParams mprfilterparams)
		{
			return this._mprDataAcess.getSavingsReport(mprfilterparams);
		}
		public int getMPRPendingListCnt(string preparedBy)
		{
			return this._mprDataAcess.getMPRPendingListCnt(preparedBy);
		}
		public List<Employee> getEmployeeList()
		{
			return this._mprDataAcess.getEmployeeList();
		}
		public List<MPRRevisionDetails_woItems> getMPRRevisionList(int RequisitionId)
		{
			return this._mprDataAcess.getMPRRevisionList(RequisitionId);
		}
		public MPRRevision statusUpdate(MPRStatusUpdate mprStatus)
		{
			return this._mprDataAcess.statusUpdate(mprStatus);
		}
		public List<SCMStatu> getStatusList()
		{
			return this._mprDataAcess.getStatusList();
		}
		public List<UserPermission> getAccessList(int RoleId)
		{
			return this._mprDataAcess.getAccessList(RoleId);
		}
		public bool updateMPRVendor(List<MPRVendorDetail> MPRVendorDetails, int RevisionId)
		{
			return this._mprDataAcess.updateMPRVendor(MPRVendorDetails, RevisionId);
		}
		public bool deleteMPR(DeleteMpr deleteMprInfo)
		{
			return this._mprDataAcess.deleteMPR(deleteMprInfo);
		}
		public string updateItemId(materialUpdate mPRItemInfo)
		{
			return this._mprDataAcess.updateItemId(mPRItemInfo);

		}

		public List<loadloction> Loadstoragelocationsbydepartment()
		{
			return this._mprDataAcess.Loadstoragelocationsbydepartment();
		}

		public SaleorderDetail LoadJobCodesbysaleorder(string saleorder)
		{
			return this._mprDataAcess.LoadJobCodesbysaleorder(saleorder);
		}
		public VendorRegApprovalProcess updateVendorRegProcess(VendorRegApprovalProcessData model, string typeOfuser)
		{
			return this._mprDataAcess.updateVendorRegProcess(model, typeOfuser);
		}
		public List<VendorRegProcessView> getVendorReqList(vendorRegfilters getVendorReqList)
		{
			return this._mprDataAcess.getVendorReqList(getVendorReqList);
		}

		public List<RemoteStateMaster> StateNameList()
		{
			return this._mprDataAcess.StateNameList();
		}
		public List<VendorRegisterDocumenetMaster> DocumentMasterList()
		{
			return this._mprDataAcess.DocumentMasterList();
		}
		public List<NatureOfBusinessMaster> natureOfBusinessesList()
		{
			return this._mprDataAcess.natureOfBusinessesList();
		}
		public VendorRegistrationModel GetVendorDetails(int vendorId)
		{
			return this._mprDataAcess.GetVendorDetails(vendorId);
		}

		public bool SaveVendorDetails(VendorRegistrationModel model)
		{
			return this._mprDataAcess.SaveVendorDetails(model);
		}

		public bool DeletefileAttached(VendorRegisterDocumentDetail model)
		{
			return this._mprDataAcess.DeletefileAttached(model);
		}

		public List<YILTermsGroup> GetYILTermGroups()
		{
			return this._mprDataAcess.GetYILTermGroups();
		}

		public bool UpdateYILTermsGroup(YILTermsGroup yilTermGroups)
		{
			return this._mprDataAcess.UpdateYILTermsGroup(yilTermGroups);
		}
		public bool UpdateYILTermsAndConditions(YILTermsandCondition yilTermandconditions)
		{
			return this._mprDataAcess.UpdateYILTermsAndConditions(yilTermandconditions);
		}
		public bool DeleteTermGroup(int TermGroupId, string DeletedBy)
		{
			return this._mprDataAcess.DeleteTermGroup(TermGroupId, DeletedBy);
		}
		public bool DeleteTermsAndConditions(int TermId, string DeletedBy)
		{
			return this._mprDataAcess.DeleteTermsAndConditions(TermId, DeletedBy);
		}


	}
}
