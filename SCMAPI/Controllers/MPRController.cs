using BALayer.MPR;
using SCMModels;
using SCMModels.SCMModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Web;
using System.Web.Http;

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
        [Route("addNewVendor")]
        public IHttpActionResult addNewVendor([FromBody] VendorMaster vendor)
        {
            return Ok(this._mprBusenessAcess.addNewVendor(vendor));
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

        [HttpPost]
        [Route("getMPRPendingListCnt/{preparedBy}")]
        public IHttpActionResult getMPRPendingListCnt(string preparedBy)
        {
            return Ok(this._mprBusenessAcess.getMPRPendingListCnt(preparedBy));
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
        [HttpGet]
        [Route("getAccessList/{RoleId}")]
        public IHttpActionResult getAccessList(int RoleId)
        {
            return Ok(this._mprBusenessAcess.getAccessList(RoleId));
        }
        [HttpPost]
        [Route("updateMPRVendor/{RevisionId}")]
        public IHttpActionResult updateMPRVendor(List<MPRVendorDetail> MPRVendorDetails, int RevisionId)
        {
            var result = this._mprBusenessAcess.updateMPRVendor(MPRVendorDetails, RevisionId);

            return Ok(result);

        }
        [Route("UploadFile")]
        [HttpPost]
        public IHttpActionResult UploadFile()
        {
            var httpRequest = HttpContext.Current.Request;
            var serverPath = HttpContext.Current.Server.MapPath("~/SCMDocs");
            string parsedFileName = "";
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
                    parsedFileName = string.Format(DateTime.Now.Year.ToString() + "\\" + "Jan" + "\\" + ToValidFileName(postedFile.FileName));
                    serverPath = serverPath + string.Format("\\" + DateTime.Now.Year.ToString() + "\\" + "Jan");
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

        private static string ToValidFileName(string fileName)
        {
            fileName = fileName.ToLower().Replace(" ", "_").Replace("(", "_").Replace(")", "_").Replace("&", "_").Replace("*", "_").Replace("-", "_");
            return string.Join("_", fileName.Split(Path.GetInvalidFileNameChars()));
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
    }
}
