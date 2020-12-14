using System;
using SCMModels;
using SCMModels.MPRMasterModels;
using SCMModels.RFQModels;
using SCMModels.SCMModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using BALayer.PurchaseAuthorization;
using System.Data;
using System.Web;
using System.IO;
using System.Data.OleDb;
using System.Globalization;
using System.Net.Http;
using System.Data.SqlClient;
using System.Linq;

namespace SCMAPI.Controllers
{
	[RoutePrefix("Api/PA")]
	public class PAController : ApiController
	{
		private readonly IPurchaseAuthorizationBA _paBusenessAcess;
		public PAController(IPurchaseAuthorizationBA purchase)
		{
			this._paBusenessAcess = purchase;
		}
		[Route("InsertPAAuthorizationLimits")]
		[ResponseType(typeof(statuscheckmodel))]
		public async Task<IHttpActionResult> InsertPAAuthorizationLimits(PAAuthorizationLimitModel model)
		{
			statuscheckmodel status = new statuscheckmodel();
			status = await _paBusenessAcess.InsertPAAuthorizationLimits(model);
			return Ok(status);
		}
		[Route("GetPAAuthorizationLimitById")]
		[ResponseType(typeof(PAAuthorizationLimitModel))]
		public async Task<IHttpActionResult> GetPAAuthorizationLimitById(int deptid)
		{
			PAAuthorizationLimitModel status = new PAAuthorizationLimitModel();
			status = await _paBusenessAcess.GetPAAuthorizationLimitById(deptid);
			return Ok(status);
		}
		[Route("CreatePAAuthirizationEmployeeMapping")]
		[ResponseType(typeof(statuscheckmodel))]
		public async Task<IHttpActionResult> CreatePAAuthirizationEmployeeMapping(PAAuthorizationEmployeeMappingModel model)
		{
			statuscheckmodel status = new statuscheckmodel();
			status = await _paBusenessAcess.CreatePAAuthirizationEmployeeMapping(model);
			return Ok(status);
		}
		[Route("GetMappingEmployee")]
		[ResponseType(typeof(PAAuthorizationEmployeeMappingModel))]
		public async Task<IHttpActionResult> GetMappingEmployee(PAAuthorizationLimitModel model)
		{
			PAAuthorizationEmployeeMappingModel status = new PAAuthorizationEmployeeMappingModel();
			status = await _paBusenessAcess.GetMappingEmployee(model);
			return Ok(status);
		}
		[Route("CreatePACreditDaysmaster")]
		[ResponseType(typeof(statuscheckmodel))]
		public async Task<IHttpActionResult> CreatePACreditDaysmaster(PACreditDaysMasterModel model)
		{
			statuscheckmodel status = new statuscheckmodel();
			status = await _paBusenessAcess.CreatePACreditDaysmaster(model);
			return Ok(status);
		}
		[Route("GetCreditdaysMasterByID")]
		[ResponseType(typeof(PACreditDaysMasterModel))]
		public async Task<IHttpActionResult> GetCreditdaysMasterByID(int creditdaysid)
		{
			PACreditDaysMasterModel status = new PACreditDaysMasterModel();
			status = await _paBusenessAcess.GetCreditdaysMasterByID(creditdaysid);
			return Ok(status);
		}
		[Route("AssignCreditdaysToEmployee")]
		[ResponseType(typeof(statuscheckmodel))]
		public async Task<IHttpActionResult> AssignCreditdaysToEmployee(PACreditDaysApproverModel model)
		{
			statuscheckmodel status = new statuscheckmodel();
			status = await _paBusenessAcess.AssignCreditdaysToEmployee(model);
			return Ok(status);
		}
		[Route("RemovePAAuthorizationLimitsByID")]
		[ResponseType(typeof(statuscheckmodel))]
		public async Task<IHttpActionResult> RemovePAAuthorizationLimitsByID(int authid)
		{
			statuscheckmodel status = new statuscheckmodel();
			status = await _paBusenessAcess.RemovePAAuthorizationLimitsByID(authid);
			return Ok(status);
		}
		[Route("RemovePACreditDaysMaster")]
		[ResponseType(typeof(statuscheckmodel))]
		public async Task<IHttpActionResult> RemovePACreditDaysMaster(int creditid)
		{
			statuscheckmodel status = new statuscheckmodel();
			status = await _paBusenessAcess.RemovePACreditDaysMaster(creditid);
			return Ok(status);
		}
		[Route("GetPAAuthorizationLimitsByDeptId")]
		[ResponseType(typeof(List<PAAuthorizationLimitModel>))]
		public async Task<IHttpActionResult> GetPAAuthorizationLimitsByDeptId(int departmentid)
		{
			List<PAAuthorizationLimitModel> model = new List<PAAuthorizationLimitModel>();
			model = await _paBusenessAcess.GetPAAuthorizationLimitsByDeptId(departmentid);
			return Ok(model);
		}
		[HttpPost]
		[Route("RemovePACreditDaysApprover")]
		[ResponseType(typeof(statuscheckmodel))]
		public async Task<IHttpActionResult> RemovePACreditDaysApprover(EmployeemappingtocreditModel model)
		{
			statuscheckmodel status = new statuscheckmodel();
			status = await _paBusenessAcess.RemovePACreditDaysApprover(model);
			return Ok(model);
		}
		[Route("GetPACreditDaysApproverById")]
		[ResponseType(typeof(PACreditDaysApproverModel))]
		public async Task<IHttpActionResult> GetPACreditDaysApproverById(int ApprovalId)
		{
			PACreditDaysApproverModel status = new PACreditDaysApproverModel();
			status = await _paBusenessAcess.GetPACreditDaysApproverById(ApprovalId);
			return Ok(status);
		}

		[HttpPost]
		[Route("GetEmployeeMappings")]
		[ResponseType(typeof(EmployeModel))]
		public async Task<IHttpActionResult> GetEmployeeMappings(PAConfigurationModel model)
		{
			EmployeModel employee = new EmployeModel();
			employee = await _paBusenessAcess.GetEmployeeMappings(model);
			return Ok(employee);
		}
		[HttpPost]
		[Route("GetEmployeeMappings1")]
		[ResponseType(typeof(DataSet))]
		public DataSet GetEmployeeMappings1(PAConfigurationModel model)
		{
			DataSet ds = new DataSet();
			ds = _paBusenessAcess.GetEmployeeMappings1(model);
			return ds;
		}

		//[HttpPost]
		//[Route("GetItemsByMasterIDs")]
		//[ResponseType(typeof(List<LoadItemsByID>))]
		//public async Task<IHttpActionResult> GetItemsByMasterIDs(PADetailsModel masters)
		//{

		//    List<LoadItemsByID> model = new List<LoadItemsByID>();
		//    model = await _rfqBusenessAcess.GetItemsByMasterIDs(masters);
		//    return Ok(model);
		//}
		[HttpPost]
		[Route("GetItemsByMasterIDs")]
		[ResponseType(typeof(List<loadtaxesbyitemwise>))]
		public IHttpActionResult GetItemsByMasterIDs(PADetailsModel masters)
		{
			return Ok(this._paBusenessAcess.GetItemsByMasterIDs(masters));
		}
		[HttpGet]
		[Route("GetAllDepartments")]
		[ResponseType(typeof(List<DepartmentModel>))]
		public async Task<IHttpActionResult> GetAllDepartments()
		{
			List<DepartmentModel> model = new List<DepartmentModel>();
			model = await _paBusenessAcess.GetAllDepartments();
			return Ok(model);
		}
		[HttpGet]
		[Route("GetSlabsByDepartmentID/{DeptID}")]
		[ResponseType(typeof(List<PAAuthorizationLimitModel>))]
		public async Task<IHttpActionResult> GetSlabsByDepartmentID(int DeptID)
		{
			List<PAAuthorizationLimitModel> model = new List<PAAuthorizationLimitModel>();
			model = await _paBusenessAcess.GetSlabsByDepartmentID(DeptID);
			return Ok(model);
		}
		[HttpGet]
		[Route("GetAllEmployee")]
		[ResponseType(typeof(List<EmployeeModel>))]
		public async Task<IHttpActionResult> GetAllEmployee()
		{
			List<EmployeModel> model = new List<EmployeModel>();
			model = await _paBusenessAcess.GetAllEmployee();
			return Ok(model);
		}
		[HttpGet]
		[Route("GetAllCredits")]
		[ResponseType(typeof(List<PAAuthorizationLimitModel>))]
		public async Task<IHttpActionResult> GetAllCredits()
		{
			List<PAAuthorizationLimitModel> model = new List<PAAuthorizationLimitModel>();
			model = await _paBusenessAcess.GetAllCredits();
			return Ok(model);
		}
		[HttpGet]
		[Route("GetAllCreditDays")]
		[ResponseType(typeof(List<PACreditDaysMasterModel>))]
		public async Task<IHttpActionResult> GetAllCreditDays()
		{
			List<PACreditDaysMasterModel> model = new List<PACreditDaysMasterModel>();
			model = await _paBusenessAcess.GetAllCreditDays();
			return Ok(model);
		}
		[HttpGet]
		[Route("GetAllMprPAPurchaseModes")]
		[ResponseType(typeof(List<MPRPAPurchaseModesModel>))]
		public async Task<IHttpActionResult> GetAllMprPAPurchaseModes()
		{
			List<MPRPAPurchaseModesModel> model = new List<MPRPAPurchaseModesModel>();
			model = await _paBusenessAcess.GetAllMprPAPurchaseModes();
			return Ok(model);
		}
		[HttpGet]
		[Route("GetAllMprPAPurchaseTypes")]
		[ResponseType(typeof(List<MPRPAPurchaseTypesModel>))]
		public async Task<IHttpActionResult> GetAllMprPAPurchaseTypes()
		{
			List<MPRPAPurchaseTypesModel> model = new List<MPRPAPurchaseTypesModel>();
			model = await _paBusenessAcess.GetAllMprPAPurchaseTypes();
			return Ok(model);
		}
		[HttpPost]
		[Route("InsertPurchaseAuthorization")]
		[ResponseType(typeof(statuscheckmodel))]
		public async Task<IHttpActionResult> InsertPurchaseAuthorization(MPRPADetailsModel model)
		{
			statuscheckmodel status = new statuscheckmodel();
			status = await _paBusenessAcess.InsertPurchaseAuthorization(model);
			return Ok(status);
		}
		[HttpPost]
		[Route("UpdatePurchaseAuthorization")]
		[ResponseType(typeof(statuscheckmodel))]
		public async Task<IHttpActionResult> UpdatePurchaseAuthorization(MPRPADetailsModel model)
		{
			statuscheckmodel status = new statuscheckmodel();
			status = await _paBusenessAcess.UpdatePurchaseAuthorization(model);
			return Ok(status);
		}
		[HttpPost]
		[Route("finalpa")]
		[ResponseType(typeof(statuscheckmodel))]
		public async Task<IHttpActionResult> finalpa(MPRPADetailsModel model)
		{
			statuscheckmodel status = new statuscheckmodel();
			status = await _paBusenessAcess.finalpa(model);
			return Ok(status);
		}
		[HttpGet]
		[Route("GetMPRPADeatilsByPAID/{PID}")]
		[ResponseType(typeof(MPRPADetailsModel))]
		public async Task<IHttpActionResult> GetMPRPADeatilsByPAID(int PID)
		{
			MPRPADetailsModel model = new MPRPADetailsModel();
			model = await _paBusenessAcess.GetMPRPADeatilsByPAID(PID);
			return Ok(model);
		}
		[HttpGet]
		[Route("GetAllMPRPAList")]
		[ResponseType(typeof(List<MPRPADetailsModel>))]
		public async Task<IHttpActionResult> GetAllMPRPAList()
		{
			List<MPRPADetailsModel> model = new List<MPRPADetailsModel>();
			model = await _paBusenessAcess.GetAllMPRPAList();
			return Ok(model);
		}
		[HttpGet]
		[Route("GetAllPAFunctionalRoles")]
		[ResponseType(typeof(List<PAFunctionalRolesModel>))]
		public async Task<IHttpActionResult> GetAllPAFunctionalRoles()
		{
			List<PAFunctionalRolesModel> model = new List<PAFunctionalRolesModel>();
			model = await _paBusenessAcess.GetAllPAFunctionalRoles();
			return Ok(model);
		}
		[HttpGet]
		[Route("GetCreditSlabsandemployees")]
		[ResponseType(typeof(List<EmployeemappingtocreditModel>))]
		public async Task<IHttpActionResult> GetCreditSlabsandemployees()
		{
			List<EmployeemappingtocreditModel> model = new List<EmployeemappingtocreditModel>();
			model = await _paBusenessAcess.GetCreditSlabsandemployees();
			return Ok(model);
		}
		[HttpGet]
		[Route("GetPurchaseSlabsandMappedemployees")]
		[ResponseType(typeof(List<EmployeemappingtopurchaseModel>))]
		public async Task<IHttpActionResult> GetPurchaseSlabsandMappedemployees()
		{
			List<EmployeemappingtopurchaseModel> model = new List<EmployeemappingtopurchaseModel>();
			model = await _paBusenessAcess.GetPurchaseSlabsandMappedemployees();
			return Ok(model);
		}
		[HttpPost]
		[Route("RemovePurchaseApprover")]
		[ResponseType(typeof(statuscheckmodel))]
		public async Task<IHttpActionResult> RemovePurchaseApprover(EmployeemappingtopurchaseModel model)
		{
			statuscheckmodel status = new statuscheckmodel();
			status = await _paBusenessAcess.RemovePurchaseApprover(model);
			return Ok(model);
		}
		[HttpGet]
		[Route("GetAllProjectManagers")]
		[ResponseType(typeof(List<ProjectManagerModel>))]
		public async Task<IHttpActionResult> GetAllProjectManagers()
		{
			List<ProjectManagerModel> model = new List<ProjectManagerModel>();
			model = await _paBusenessAcess.LoadAllProjectManagers();
			return Ok(model);
		}
		[HttpPost]
		[Route("LoadVendorByMprDetailsId")]
		[ResponseType(typeof(List<VendormasterModel>))]
		public async Task<IHttpActionResult> LoadVendorByMprDetailsId(List<int?> MPRItemDetailsid)
		{
			List<VendormasterModel> model = new List<VendormasterModel>();
			model = await _paBusenessAcess.LoadVendorByMprDetailsId(MPRItemDetailsid);
			return Ok(model);
		}
		[HttpGet]
		[Route("GetAllApproversList")]
		[ResponseType(typeof(List<MPRPAApproversModel>))]
		public async Task<IHttpActionResult> GetAllApproversList()
		{
			List<MPRPAApproversModel> model = new List<MPRPAApproversModel>();
			model = await _paBusenessAcess.GetAllApproversList();
			return Ok(model);
		}
		[HttpPost]
		[Route("GetMprApproverDetailsBySearch")]
		[ResponseType(typeof(List<GetmprApproverdeatil>))]
		public async Task<IHttpActionResult> GetMprApproverDetailsBySearch(PAApproverDetailsInputModel model)
		{
			List<mprApproverdetailsview> details = new List<mprApproverdetailsview>();
			details = await _paBusenessAcess.GetMprApproverDetailsBySearch(model);
			return Ok(details);
		}
		[HttpPost]
		[Route("UpdateMprpaApproverStatus")]
		[ResponseType(typeof(statuscheckmodel))]
		public async Task<IHttpActionResult> UpdateMprpaApproverStatus(MPRPAApproversModel model)
		{
			statuscheckmodel status = new statuscheckmodel();
			status = await _paBusenessAcess.UpdateMprpaApproverStatus(model);
			return Ok(model);
		}
		[HttpPost]
		[Route("getrfqtermsbyrevisionid")]
		[ResponseType(typeof(List<DisplayRfqTermsByRevisionId>))]
		public async Task<IHttpActionResult> getrfqtermsbyrevisionid(List<int> RevisionId)
		{
			List<DisplayRfqTermsByRevisionId> details = new List<DisplayRfqTermsByRevisionId>();
			details = await _paBusenessAcess.getrfqtermsbyrevisionid(RevisionId);
			return Ok(details);
		}
		[HttpPost]
		[Route("GetPurchaseSlabsandMappedemployeesByDeptId")]
		[ResponseType(typeof(List<Employeemappingtopurchase>))]
		public async Task<IHttpActionResult> GetPurchaseSlabsandMappedemployeesByDeptId(EmployeeFilterModel model)
		{
			List<Employeemappingtopurchase> purchase = new List<Employeemappingtopurchase>();
			purchase = await _paBusenessAcess.GetPurchaseSlabsandMappedemployeesByDeptId(model);
			return Ok(purchase);
		}
		[HttpPost]
		[Route("InsertPaitems")]
		[ResponseType(typeof(statuscheckmodel))]
		public async Task<IHttpActionResult> InsertPaitems(List<ItemsViewModel> paitem)
		{
			statuscheckmodel model = new statuscheckmodel();
			model = await _paBusenessAcess.InsertPaitems(paitem);
			return Ok(model);
		}
		[HttpGet]
		[Route("GetAllMappedSlabs")]
		[ResponseType(typeof(List<GetMappedSlab>))]
		public async Task<IHttpActionResult> GetAllMappedSlabs()
		{
			List<GetMappedSlab> model = new List<GetMappedSlab>();
			model = await _paBusenessAcess.GetAllMappedSlabs();
			return Ok(model);
		}
		[HttpPost]
		[Route("RemoveMappedSlab")]
		[ResponseType(typeof(statuscheckmodel))]
		public async Task<IHttpActionResult> RemoveMappedSlab(PAAuthorizationLimitModel model)
		{
			statuscheckmodel status = new statuscheckmodel();
			status = await _paBusenessAcess.RemoveMappedSlab(model);
			return Ok(status);
		}
		[HttpPost]
		[Route("getMprPaDetailsBySearch")]
		[ResponseType(typeof(List<NewGetMprPaDetailsByFilter>))]
		public async Task<IHttpActionResult> getMprPaDetailsBySearch(PADetailsModel model)
		{
			List<NewGetMprPaDetailsByFilter> filter = new List<NewGetMprPaDetailsByFilter>();
			filter = await _paBusenessAcess.getMprPaDetailsBySearch(model);
			return Ok(filter);
		}
		[HttpPost]
		[Route("GetPaStatusReports")]
		[ResponseType(typeof(List<PAReport>))]
		public async Task<IHttpActionResult> GetPaStatusReports(PAReportInputModel model)
		{
			List<PAReport> filter = new List<PAReport>();
			filter = await _paBusenessAcess.GetPaStatusReports(model);
			return Ok(filter);
		}
		[HttpPost]
		[Route("UpdateApproverforRequest")]
		[ResponseType(typeof(statuscheckmodel))]
		public async Task<IHttpActionResult> UpdateApproverforRequest(MPRPAApproversModel model)
		{
			statuscheckmodel status = new statuscheckmodel();
			status = await _paBusenessAcess.UpdateApproverforRequest(model);
			return Ok(status);
		}

		[HttpPost]
		[Route("UploadFile")]
		public IHttpActionResult UploadPADocuments()
		{
			var httpRequest = HttpContext.Current.Request;
			var serverPath = HttpContext.Current.Server.MapPath("~/PADocuments");
			string parsedFileName = "";
			var revisionId = httpRequest.Files.AllKeys[0];
			if (httpRequest.Files.Count > 0)
			{
				foreach (string file in httpRequest.Files)
				{
					var postedFile = httpRequest.Files[file];
					byte[] fileData = null;
					using (var binaryReader = new BinaryReader(postedFile.InputStream))
					{
						fileData = binaryReader.ReadBytes(postedFile.ContentLength);
					}

					GC.Collect();
					parsedFileName = string.Format(DateTime.Now.Year.ToString() + "\\" + DateTime.Now.ToString("MMM") + "\\" + revisionId + "\\" + ToValidFileName(postedFile.FileName));
					serverPath = serverPath + string.Format("\\" + DateTime.Now.Year.ToString() + "\\" + DateTime.Now.ToString("MMM")) + "\\" + revisionId;
					var path = Path.Combine(serverPath, ToValidFileName(postedFile.FileName));
					if (!Directory.Exists(serverPath))
						Directory.CreateDirectory(serverPath);
					var memory = new MemoryStream();
					FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
					var updatedStream = new MemoryStream(fileData);
					updatedStream.Seek(0, SeekOrigin.Begin);
					updatedStream.CopyToAsync(fs).Wait();
					fs.Flush();
					GC.Collect();
				}
			}
			return Ok(parsedFileName);

		}

		[HttpPost]
		[Route("uploadExcel")]
		public IHttpActionResult uploadExcel()
		{
			var paid = "";
			int documentid = 0;
			var httpRequest = HttpContext.Current.Request;
			var serverPath = HttpContext.Current.Server.MapPath("~/PADocuments");
			string parsedFileName = "";
			string filename = "";

			if (httpRequest.Files.Count > 0)
			{
				paid = httpRequest.Files.AllKeys[0];
				//string employeeno = httpRequest.Files.AllKeys[1];
				var postedFile = httpRequest.Files[0];
				parsedFileName = string.Format(DateTime.Now.Year.ToString() + "\\" + DateTime.Now.ToString("MMM") + "\\" + paid + "\\" + ToValidFileName(postedFile.FileName));
				serverPath = serverPath + string.Format("\\" + DateTime.Now.Year.ToString() + "\\" + DateTime.Now.ToString("MMM")) + "\\" + paid;
				var filePath = Path.Combine(serverPath, ToValidFileName(postedFile.FileName));
				if (!Directory.Exists(serverPath))
					Directory.CreateDirectory(serverPath);
				postedFile.SaveAs(filePath);
				try
				{
					YSCMEntities entities = new YSCMEntities();

					var data = new MPRPADocument();
					data.Filename = postedFile.FileName;
					data.Filepath = parsedFileName;
					data.uploadeddate = System.DateTime.Now;
					data.paid = Convert.ToInt32(paid);
					data.deleteflag = false;
					entities.MPRPADocuments.Add(data);
					entities.SaveChanges();
					documentid = data.DocumentId;
					filename = data.Filename;

					//entities.MPRPADocuments.Add(new MPRPADocument
					//{
					//    Filename = postedFile.FileName,
					//    Filepath = parsedFileName,
					//    uploadeddate = System.DateTime.Now,
					//    paid = Convert.ToInt32(paid)
					//});
					//entities.SaveChanges();

					// int succRecs = iSucceRows;
				}
				catch (Exception e)
				{
					throw e;
				}
				//for (int i = 0; i < httpRequest.Files.Count; i++)
				//{


				//}


			}
			return Ok(filename);

		}
		private static string ToValidFileName(string fileName)
		{
			fileName = fileName.ToLower().Replace(" ", "_").Replace("(", "_").Replace(")", "_").Replace("&", "_").Replace("*", "_").Replace("-", "_").Replace("+", "_");
			return string.Join("_", fileName.Split(Path.GetInvalidFileNameChars()));
		}
		[HttpPost]
		[Route("DeletePAByPAid")]
		[ResponseType(typeof(statuscheckmodel))]
		public async Task<IHttpActionResult> DeletePAByPAid(padeletemodel model)
		{
			statuscheckmodel status = new statuscheckmodel();
			status = await _paBusenessAcess.DeletePAByPAid(model);
			return Ok(status);
		}
		[HttpPost]
		[Route("LoadIncompletedPAlist")]
		[ResponseType(typeof(List<IncompletedPAlist>))]
		public async Task<IHttpActionResult> LoadIncompletedPAlist(painputmodel model)
		{
			List<IncompletedPAlist> status = new List<IncompletedPAlist>();
			status = await _paBusenessAcess.GetIncompletedPAlist(model);
			return Ok(status);
		}
		//[HttpPost]
		//[Route("FileUploading1")]
		//public async Task<string> FileUploading1()
		//{
		//    var ctx = HttpContext.Current.Request;
		//    var root = HttpContext.Current.Server.MapPath("~/PADocuments");
		//    string path = "";
		//    string parsedFileName = "";
		//    DateTime dateUpload;
		//    string FileName = "";
		//    int paid = 61;
		//    var provider = new MultipartFormDataStreamProvider(root);
		//    try
		//    {
		//        await Request.Content.ReadAsMultipartAsync(provider);
		//        string FileType = ctx.Files.AllKeys[0];
		//        if (ctx.Files.Count > 0)
		//        {
		//            foreach (var file in provider.FileData)
		//            {
		//                var postedfile = ctx.Files[0];
		//                var name = file.Headers.ContentDisposition.FileName;
		//                name = name.Trim('"');
		//                //string extension = System.IO.Path.GetExtension(name);
		//                //string result = name.Substring(0, name.Length - extension.Length);
		//                //FileName = result;
		//                dateUpload = DateTime.Now;
		//                parsedFileName = string.Format(DateTime.Now.Year.ToString() + "\\" + DateTime.Now.ToString("MMM") + "\\" + paid + "\\" + ToValidFileName(postedfile.FileName));
		//                root = root + string.Format("\\" + DateTime.Now.Year.ToString() + "\\" + DateTime.Now.ToString("MMM")) + "\\" + paid;
		//                var filePath = Path.Combine(root, ToValidFileName(postedfile.FileName));
		//                if (!Directory.Exists(root))
		//                    Directory.CreateDirectory(root);
		//                postedfile.SaveAs(filePath);
		//            }
		//        }

		//    }
		//    catch (Exception e)
		//    {
		//        return $"Error:{e.Message}";
		//    }
		//    return "File Uploaded!";
		//}
		[HttpPost]
		[Route("getrfqtermsbyrevisionsid1")]
		[ResponseType(typeof(DataTable))]
		public DataTable getrfqtermsbyrevisionsid1(List<int> revisionid)
		{
			DataTable ds = new DataTable();
			ds = _paBusenessAcess.getrfqtermsbyrevisionsid1(revisionid);
			return ds;
		}
		[HttpPost]
		[Route("DeletePADocument")]
		[ResponseType(typeof(statuscheckmodel))]
		public async Task<IHttpActionResult> DeletePADocument(PADocumentsmodel model)
		{
			statuscheckmodel status = new statuscheckmodel();
			status = await _paBusenessAcess.DeletePADocument(model);
			return Ok(status);
		}
		[HttpPost]
		[Route("getTokuchuReqList")]
		public IHttpActionResult getTokuchuReqList(tokuchuFilterParams tokuchufilterparams)
		{
			return Ok(this._paBusenessAcess.getTokuchuReqList(tokuchufilterparams));
		}

		[HttpGet]
		[Route("GetTokuchuDetailsByPAID/{PID}/{TokuchRequestid}")]
		public IHttpActionResult GetTokuchuDetailsByPAID(int? PID, int? TokuchRequestid)
		{
			var result = _paBusenessAcess.GetTokuchuDetailsByPAID(PID, TokuchRequestid);
			return Ok(result);
		}

		[HttpPost]
		[Route("updateTokuchuRequest/{typeOfuser}/{revisionId}")]
		public IHttpActionResult getDBMastersList([FromBody] TokuchuRequest Result, string typeOfuser,int revisionId)
		{
			return Ok(this._paBusenessAcess.updateTokuchuRequest(Result, typeOfuser, revisionId));
		}

        [HttpPost]
        [Route("Getmprstatus")]
        public IHttpActionResult Getmprstatus()
        {
            return Ok(this._paBusenessAcess.Getmprstatus());
        }

        [HttpPost]
        [Route("GetmprstatusReport")]
        [ResponseType(typeof(DataSet))]
        public DataSet GetmprstatusReport(ReportInputModel model)
        {
            DataSet ds = new DataSet();
            SqlParameter[] Param = new SqlParameter[5];
            string data = "";
            YSCMEntities obj = new YSCMEntities();
            if (model.OrgDepartmentId != 0)
            {
                List<int> departments = obj.MPRDepartments.Where(x => x.ORgDepartmentid == model.OrgDepartmentId).Select(x => (int)x.DepartmentId).ToList();
                data = string.Join(" , ", departments);
            }
            if (model.BuyerGroupId ==0)
            {
                Param[0] = new SqlParameter("buyergroupid", SqlDbType.VarChar);
                Param[0].Value = DBNull.Value;
                Param[1] = new SqlParameter("@fromdate", model.Fromdate);
                Param[2] = new SqlParameter("@todate", model.Todate);
                Param[3] = new SqlParameter("@DepartmentId", data);
                Param[4] = new SqlParameter("@issuepurpose", model.Issuepurposeid);
            }
            else
            {
                //string region = (string.Join(",", model.multiregion.Select(x => x.Region.ToString()).ToArray()));
                Param[0] = new SqlParameter("@BuyerGroupId", model.BuyerGroupId);
                Param[1] = new SqlParameter("@fromdate", model.Fromdate);
                Param[2] = new SqlParameter("@todate", model.Todate);
                Param[3] = new SqlParameter("@DepartmentId", data);
                Param[4] = new SqlParameter("@issuepurpose", model.Issuepurposeid);
            }
            ds = _paBusenessAcess.GetmprstatusReport("newmprstatuareport", Param);
            return ds;
        }
        [HttpPost]
        [Route("GetMprstatuswisereport")]
        [ResponseType(typeof(DataSet))]
        public DataSet GetMprstatuswisereport(ReportInputModel model)
        {
            DataSet ds = new DataSet();
            SqlParameter[] Param = new SqlParameter[6];
            string data = "";
            YSCMEntities obj = new YSCMEntities();
            if (model.DepartmentId == 0)
            {
                List<int> departments = obj.MPRDepartments.Where(x => x.ORgDepartmentid == model.OrgDepartmentId).Select(x => (int)x.DepartmentId).ToList();
                data = string.Join(" , ", departments);
            }
            else
            {
                data = string.Join(",", model.DepartmentId);
            }
            if (model.BuyerGroupId == 0)
            {
                Param[0] = new SqlParameter("buyergroupid", SqlDbType.VarChar);
                Param[0].Value = DBNull.Value;
                Param[1] = new SqlParameter("@fromdate", model.Fromdate);
                Param[2] = new SqlParameter("@todate", model.Todate);
                Param[3] = new SqlParameter("@ProjectManager", model.ProjectManager);
                Param[4] = new SqlParameter("@SaleOrderNo", model.SaleOrderNo);
                Param[5] = new SqlParameter("@Departmentid", data);
            }
            else
            {
                //string region = (string.Join(",", model.multiregion.Select(x => x.Region.ToString()).ToArray()));
                Param[0] = new SqlParameter("@BuyerGroupId", model.BuyerGroupId);
                Param[1] = new SqlParameter("@fromdate", model.Fromdate);
                Param[2] = new SqlParameter("@todate", model.Todate);
                Param[3] = new SqlParameter("@ProjectManager", model.ProjectManager);
                Param[4] = new SqlParameter("@SaleOrderNo", model.SaleOrderNo);
                Param[5] = new SqlParameter("@Departmentid", data);
            }
            ds = _paBusenessAcess.GetMprstatuswisereport("Mprwisereport",Param);
            return ds;
        }
        [HttpPost]
        [Route("GetmprRequisitionReport")]
        public IHttpActionResult GetmprRequisitionReport(ReportInputModel input)
        {
            return Ok(this._paBusenessAcess.GetmprRequisitionReport(input));
        }
        [HttpGet]
        [Route("GetmprRequisitionfilters")]
        [ResponseType(typeof(ReportFilterModel))]
        public IHttpActionResult GetmprRequisitionfilters()
        {
            ReportFilterModel status = new ReportFilterModel();
            status =  _paBusenessAcess.GetmprRequisitionfilters();
            return Ok(status);
        }
        [HttpGet]
        [Route("Loadprojectmanagersforreport")]
        public IHttpActionResult Loadprojectmanagersforreport()
        {
            return Ok(this._paBusenessAcess.Loadprojectmanagersforreport());
        }
        [HttpPost]
        [Route("Loadprojectcodewisereport")]
       
        public IHttpActionResult Loadprojectcodewisereport(ReportInputModel model)
        {
           //List<Reportbyprojectcode> status = new List<Reportbyprojectcode>();
           // status = _paBusenessAcess.Loadprojectcodewisereport(model);
            return Ok(_paBusenessAcess.Loadprojectcodewisereport(model));
        }
        [HttpPost]
        [Route("LoadprojectDurationwisereport")]
        [ResponseType(typeof(ReportbyprojectDuration))]
        public IHttpActionResult LoadprojectDurationwisereport(ReportInputModel model)
        {
            List<ReportbyprojectDuration> status = new List<ReportbyprojectDuration>();
            status = _paBusenessAcess.LoadprojectDurationwisereport(model);
            return Ok(status);
        }
        [HttpGet]
        [Route("Loadjobcodes")]
        public IHttpActionResult Loadjobcodes()
        {
            return Ok(this._paBusenessAcess.Loadjobcodes());
        }
        [HttpGet]
        [Route("GETApprovernamesbydepartmentid/{departmentid}")]
        [ResponseType(typeof(DataTable))]
        public DataTable GETApprovernamesbydepartmentid(int departmentid)
        {
            DataTable ds = new DataTable();
            ds = _paBusenessAcess.GETApprovernamesbydepartmentid(departmentid);
            return ds;
        }
        [HttpGet]
        [Route("Loadsaleorder")]
        public IHttpActionResult Loadsaleorder()
        {
            return Ok(this._paBusenessAcess.Loadsaleorder());
        }
    }
}
