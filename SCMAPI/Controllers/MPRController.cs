using BALayer.MPR;
using SCMModels;
using SCMModels.SCMModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace SCMAPI.Controllers
{
	[RoutePrefix("Api/MPR")]
	public class MPRController : ApiController
	{
		private readonly IMPRBA _mprBusenessAcess;
		public MPRController(IMPRBA mprBA)
		{
			this._mprBusenessAcess = mprBA;
		}

		[HttpPost]
		[Route("getDBMastersList")]
		public IHttpActionResult getDBMastersList([FromBody] DynamicSearchResult Result)
		{
			return Ok(this._mprBusenessAcess.getDBMastersList(Result));
		}

		[HttpPost]
		[Route("addDataToDBMasters")]
		public IHttpActionResult addDataToDBMasters([FromBody] DynamicSearchResult Result)
		{
			return Ok(this._mprBusenessAcess.addDataToDBMasters(Result));
		}

		[HttpPost]
		[Route("updateDataToDBMasters")]
		public IHttpActionResult updateDataToDBMasters([FromBody] DynamicSearchResult Result)
		{
			return Ok(this._mprBusenessAcess.updateDataToDBMasters(Result));
		}

		[HttpPost]
		[Route("GetListItems")]
		public IHttpActionResult GetListItems([FromBody] DynamicSearchResult Result)
		{
			return Ok(this._mprBusenessAcess.GetListItems(Result));
		}

		[HttpPost]
		[Route("UpdateMPR")]
		public IHttpActionResult updateMPR([FromBody] MPRRevision mpr)
		{
			return Ok(this._mprBusenessAcess.updateMPR(mpr));
		}
		[HttpPost]
		[Route("deleteMPRDocument")]
		public IHttpActionResult deleteMPRDocument([FromBody] MPRDocument mprDocument)
		{
			return Ok(this._mprBusenessAcess.deleteMPRDocument(mprDocument));
		}

		[HttpPost]
		[Route("deleteMPRItemInfo")]
		public IHttpActionResult deleteMPRItemInfo([FromBody] MPRItemInfo mprItemInfo)
		{
			return Ok(this._mprBusenessAcess.deleteMPRItemInfo(mprItemInfo));
		}

		[HttpPost]
		[Route("deleteMPRVendor")]
		public IHttpActionResult deleteMPRVendor([FromBody] MPRVendorDetail mprVendor)
		{
			return Ok(this._mprBusenessAcess.deleteMPRVendor(mprVendor));
		}

		[HttpPost]
		[Route("deleteMPRDocumentation")]
		public IHttpActionResult deleteMPRDocumentation([FromBody] MPRDocumentation MPRDocumentation)
		{
			return Ok(this._mprBusenessAcess.deleteMPRDocumentation(MPRDocumentation));
		}

		[HttpGet]
		[Route("getMPRRevisionDetails/{RevisionId}")]
		public IHttpActionResult getMPRRevisionDetails(int RevisionId)
		{
			return Ok(this._mprBusenessAcess.getMPRRevisionDetails(RevisionId));
		}

		[HttpPost]
		[Route("getMPRList")]
		public IHttpActionResult getMPRList(mprFilterParams mprfilterparams)
		{
			return Ok(this._mprBusenessAcess.getMPRList(mprfilterparams));
		}
		[HttpGet]
		[Route("getEmployeeList")]
		public IHttpActionResult getEmployeeList()
		{
			return Ok(this._mprBusenessAcess.getEmployeeList());
		}

		[HttpGet]
		[Route("getMPRRevisionList/{RequisitionId}")]
		public IHttpActionResult getMPRRevisionList(int RequisitionId)
		{
			return Ok(this._mprBusenessAcess.getMPRRevisionList(RequisitionId));
		}
		[HttpPost]
		[Route("statusUpdate")]
		public IHttpActionResult statusUpdate(MPRStatusUpdate mprStatus)
		{
			return Ok(this._mprBusenessAcess.statusUpdate(mprStatus));
		}
		[HttpGet]
		[Route("getStatusList")]
		public IHttpActionResult getStatusList()
		{
			return Ok(this._mprBusenessAcess.getStatusList());
		}
		[HttpPost]
		[Route("updateMPRVendor/{RevisionId}")]
		public IHttpActionResult updateMPRVendor(List<MPRVendorDetail> MPRVendorDetails, int RevisionId)
		{
			return Ok(this._mprBusenessAcess.updateMPRVendor(MPRVendorDetails, RevisionId));

		}

	}
}
