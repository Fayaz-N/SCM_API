using BALayer;
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
    public class LoginController : ApiController
    {
        YSCMEntities DB = new YSCMEntities();
        private readonly ILoginBA _mprLoginAccess;
        public LoginController(ILoginBA loginBA)
        {
            this._mprLoginAccess = loginBA;
        }

        [HttpPost]
        [Route("ValidateLoginCredentials")]
        public IHttpActionResult ValidateLoginCredentials([FromBody] DynamicSearchResult Result)
        {
            return Ok(this._mprLoginAccess.ValidateLoginCredentials(Result));
        }
    }
}
