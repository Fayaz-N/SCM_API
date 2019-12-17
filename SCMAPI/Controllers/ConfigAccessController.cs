using BALayer.MPR;
using SCMAPI.Models;
using SCMModels.SCMModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace SCMAPI.Controllers
{
    [RoutePrefix("Api/ConfigAccess")]
    public class ConfigAccessController : ApiController
    {

        YSCMEntities DB = new YSCMEntities();
        private readonly IConfigAccessInterfaceBA _iConfigAccessInterface;
        public ConfigAccessController(IConfigAccessInterfaceBA configAccess)
        {
            this._iConfigAccessInterface = configAccess;
        }
        //Group Label access adding
        [HttpGet]
        [Route("getAllGroupMaster")]

        public List<GroupMasterDomainModel> getAllGroupMaster()
        {
            var result = this._iConfigAccessInterface.getGroupMasterDetail();
            return result;
        }
        //public IHttpActionResult getAllGroupMaster()
        //{
        //    return Ok(this._iConfigAccessInterface.getGroupMasterDetail());
        //}



        [HttpPost]
        [Route("AddNewGroupMaster")]
        public Task<string> AddNewGroupMaster([FromBody]GroupMasterViewModel groupMaster)
        {
            GroupMasterDomainModel groupMasterDomainModel = new GroupMasterDomainModel();

            groupMasterDomainModel.GroupName = groupMaster.GroupName;
            groupMasterDomainModel.UpdatedBy = "190455";
            groupMasterDomainModel.DeleteFlag = groupMaster.DeleteFlag;
            groupMasterDomainModel.UpdatedDate = DateTime.Now;

            var result = _iConfigAccessInterface.AddNewGroupMaster(groupMasterDomainModel);
            return result;
        }


        [HttpPost]
        [Route("UpdateGroupMaster")]
        public async Task<string> UpdateGroupMaster([FromBody]UpdateGroupMasterDomainModel groupMaster)
        {
          

            var result = await _iConfigAccessInterface.UpdateGroupMaster(groupMaster);
            return result;
        }

        [HttpPut]
        [Route("DeleteGroupMaster")]
        public async Task<string> DeleteGroupMaster([FromBody]DeleteGroupMasterDomainModel groupMaster)
        {
                       
            var result = await _iConfigAccessInterface.DeleteGroupMaster(groupMaster);
            return result;
        }

        //giving  access on group label

        //[HttpGet]
        //[Route("getAllGroupName")]
        //public IHttpActionResult getAllGroupName()
        //{
        //    return Ok(this._iConfigAccessInterface.getGroupNameDetail());
        //}

        [HttpGet]
        [Route("getGroupNameDetail")]
        public List<GroupMasterDomainModel> getGroupNameDetail()
        {
            var result = this._iConfigAccessInterface.getGroupNameDetail();
            return result;
        }


        [HttpPut]
        [Route("DeleteGroupAccess")]
        public async Task<string> DeleteGroupAccess([FromBody]DeleteGroupAccessDomainModel model)
        {

            var result = await _iConfigAccessInterface.DeleteGroupAccess(model);
            return result;
        }

        [HttpGet]
        [Route("getAccessNameById")]

        public List<GroupAccessNameModel> getAccessNameById(int accessGroupId)
        {
            var result = this._iConfigAccessInterface.getAccessNameById(accessGroupId);
            return result;
        }

        [HttpPost]
        [Route("AddNewAccessName")]
        public Task<string> AddNewAccessName([FromBody]GroupAccessNameModel model)
        {
           

            var result = _iConfigAccessInterface.AddNewAccessName(model);
            return result;
        }


        //Updated AccessName on edit Click
        [HttpPost]
        [Route("UpdateAccessName")]
        public async Task<string> UpdateAccessName([FromBody]UpdateAccessNameDomainModel model)
        {
            var result = await _iConfigAccessInterface.UpdateAccessName(model);
            return result;
        }

        //Delete AccessName on delete Click
        [HttpPost]
        [Route("DeleteAccessName")]
        public async Task<string> DeleteAccessName([FromBody]DeleteAccessNameDomainModel model)
        {
            var result = await _iConfigAccessInterface.DeleteAccessName(model);
            return result;
        }

        //Adding Access on Role Label

        [HttpPost]
        [Route("AddNewRole")]
        public Task<string> AddRoleAccess([FromBody]AddRoleViewModel model)
        {
            AddRoleDomainModel dmodel = new AddRoleDomainModel();

            dmodel.RoleName = model.RoleName;
            dmodel.UpdatedBy = "190455";
            dmodel.DeleteFlag = model.DeleteFlag;
            dmodel.updatedDate = DateTime.Now;

            var result = _iConfigAccessInterface.AddRoleAccess(dmodel);
            return result;
        }

        [HttpPost]
        [Route("AddAccessRole")]
        public string AddAccessRole(checkboxSelect selectCheckbox)
        {
            

            var result = _iConfigAccessInterface.AddAccessRole(selectCheckbox);
            return result;
        }

        [HttpGet]
        [Route("getAllRole")]
        public List<AddRoleDomainModel> getAllRole()
        {
            var result = this._iConfigAccessInterface.getAllRoleDetail();
            return result;
        }

        [HttpGet]
        [Route("getAuthorizationItemDetailById")]
        public List<AuthorizationItemsModel> getAuthorizationItemDetailById(int roleid)
        {
            var result = this._iConfigAccessInterface.getAuthorizationItemDetailById(roleid);
            return result;
        }

        [HttpGet]
        [Route("getAuthorizationItemDetail")]
        public List<AuthorizationItemsModel> getAuthorizationItemDetail()
        {
            var result = this._iConfigAccessInterface.getAuthorizationItemDetail();
            return result;
        }

        [HttpGet]
        [Route("getAllGroupById")]

        public List<GetAllNameModel> getAllGroupById(int roleId)
        {
            var result = this._iConfigAccessInterface.getAllGroupById(roleId);
            return result;
        }

        //Authorization Item Access to MasterGroup

        [HttpGet]
        [Route("getAllRoleName")]
        public List<AutorizationGroupModel> getAllRoleName()
        {
            var result = this._iConfigAccessInterface.getAllRoleName();
            return result;
        }


        [HttpGet]
        [Route("getAllAccessNameData")]
        public List<GroupAccessNameModel> getAllAccessNameData()
        {
            var result = this._iConfigAccessInterface.getAllAccessNameData();
            return result;
        }



        [HttpGet]
        [Route("getGroupAccessNameById")]

        public List<GroupAccessNameModel> getGroupAccessNameById(int accessGroupId)
        {
            var result = this._iConfigAccessInterface.getGroupAccessNameById(accessGroupId);
            return result;
        }

        [HttpPost]
        [Route("AddAccess")]
        public Task<string> AddAccess([FromBody]AddAccessViewModel model)
        {
            AddAccessDomainModel dmodel = new AddAccessDomainModel();

            dmodel.AccessNameID = model.AccessNameID;
            dmodel.RoleId = model.RoleId;
            dmodel.UpdatedBy = "190455";
            dmodel.DeleteFlag = model.DeleteFlag;
            dmodel.updatedDate = DateTime.Now;

            var result = _iConfigAccessInterface.AddAccess(dmodel);
            return result;
        }

        [HttpPost]
        [Route("UpdateAccess")]
        public Task<string> UpdateAccess([FromBody]AuthorizationItemsModel model)
        {
            var result = _iConfigAccessInterface.UpdateAccess(model);
            return result;
        }

        //AuthGroup Table Methods


        [HttpPost]
        [Route("UpdateAuthRole")]
        public async Task<string> UpdateAuthRole([FromBody]UpdateAuthGroupDomainModel model)
        {


            var result = await _iConfigAccessInterface.UpdateAuthRole(model);
            return result;
        }

        [HttpPost]
        [Route("DeleteAuthRole")]
        public async Task<string> DeleteAuthRole([FromBody]DeleteAuthGroupDomainModel model)
        {

            var result = await _iConfigAccessInterface.DeleteAuthRole(model);
            return result;
        }


        [HttpGet]
        [Route("getAllAccessName")]

        public List<GroupAccessNameModel> getAllAccessName()
        {
            var result = this._iConfigAccessInterface.getAllAccessName();
            return result;
        }
        //Get all accessname by roleid for view access page
        [HttpGet]
        [Route("getAccessNameRoleId")]

        public List<GroupAccessNameModel> getAccessNameRoleId(int roleid)
        {
            var result = this._iConfigAccessInterface.getAccessNameRoleId(roleid);
            return result;
        }


    }
}
