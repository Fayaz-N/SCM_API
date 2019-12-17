using DALayer.MPR;
using SCMAPI.Models;
using SCMModels.SCMModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BALayer.MPR
{
    public class ConfigAccessRepoBA : IConfigAccessInterfaceBA
    {
        public readonly IConfigAccessInterfaceDA _iConfigAccessInterfaceDA;
        public ConfigAccessRepoBA(IConfigAccessInterfaceDA configAccess)
        {
            this._iConfigAccessInterfaceDA = configAccess;
        }

        public List<GroupMasterDomainModel> getGroupMasterDetail()
        {
            return this._iConfigAccessInterfaceDA.getGroupMasterDetail();
        }

        public Task<string> AddNewGroupMaster(GroupMasterDomainModel model)
        {
            return _iConfigAccessInterfaceDA.AddNewGroupMaster(model);
        }

        public Task<string> UpdateGroupMaster(UpdateGroupMasterDomainModel model)
        {
            return _iConfigAccessInterfaceDA.UpdateGroupMaster(model);
        }

        public Task<string> DeleteGroupMaster(DeleteGroupMasterDomainModel model)
        {
            return _iConfigAccessInterfaceDA.DeleteGroupMaster(model);
        }

        public List<GroupMasterDomainModel> getGroupNameDetail()
        {
            return this._iConfigAccessInterfaceDA.getGroupNameDetail();
        }
        public List<GroupAccessNameModel> getAccessNameById(int accessGroupId)
        {
            return this._iConfigAccessInterfaceDA.getAccessNameById(accessGroupId);
        }
        //public  GroupAccessNameModel getAccessName(string AccessGroupId)
        //{
        //    return this._iConfigAccessInterfaceDA.getAccessName(AccessGroupId); 
        //}

        public Task<string> AddRoleAccess(AddRoleDomainModel model)
        {
            return _iConfigAccessInterfaceDA.AddRoleAccess(model);
        }
        public string AddAccessRole(checkboxSelect selectCheckbox)
        {
            return _iConfigAccessInterfaceDA.AddAccessRole(selectCheckbox);
        }

        public List<AddRoleDomainModel> getAllRoleDetail()
        {
            return this._iConfigAccessInterfaceDA.getAllRoleDetail();
        }

        public Task<string> DeleteGroupAccess(DeleteGroupAccessDomainModel model)
        {
            return _iConfigAccessInterfaceDA.DeleteGroupAccess(model);
        }

        public Task<string> AddNewAccessName(GroupAccessNameModel model)
        {
            return _iConfigAccessInterfaceDA.AddNewAccessName(model);
        }

        public Task<string> UpdateAccessName(UpdateAccessNameDomainModel model)
        {
            return _iConfigAccessInterfaceDA.UpdateAccessName(model);
        }

        public List<GetAllNameModel> getAllGroupById(int roleId)
        {
            return this._iConfigAccessInterfaceDA.getAllGroupById(roleId);
        }

        public List<AutorizationGroupModel> getAllRoleName()
        {
            return this._iConfigAccessInterfaceDA.getAllRoleName();
        }

        public List<GroupAccessNameModel> getGroupAccessNameById(int accessGroupId)
        {
            return this._iConfigAccessInterfaceDA.getGroupAccessNameById(accessGroupId);
        }
        public Task<string> AddAccess(AddAccessDomainModel model)
        {
            return this._iConfigAccessInterfaceDA.AddAccess(model);
        }

        public Task<string> UpdateAccess(AuthorizationItemsModel model)
        {
            return _iConfigAccessInterfaceDA.UpdateAccess(model);
        }
        public List<AuthorizationItemsModel> getAuthorizationItemDetailById(int roleid)
        {
            return _iConfigAccessInterfaceDA.getAuthorizationItemDetailById(roleid);
        }
       public List<AuthorizationItemsModel> getAuthorizationItemDetail()
        {
            return _iConfigAccessInterfaceDA.getAuthorizationItemDetail();
        }
        public Task<string> UpdateAuthRole(UpdateAuthGroupDomainModel model)
        {
            return _iConfigAccessInterfaceDA.UpdateAuthRole(model);
        }
       public List<GroupAccessNameModel> getAllAccessNameData()
        {
            return _iConfigAccessInterfaceDA.getAllAccessNameData();
        }

        public Task<string> DeleteAuthRole(DeleteAuthGroupDomainModel model)
        {
            return _iConfigAccessInterfaceDA.DeleteAuthRole(model);
        }

       public Task<string> DeleteAccessName(DeleteAccessNameDomainModel model)
        {
            return _iConfigAccessInterfaceDA.DeleteAccessName(model);
        }
        public List<GroupAccessNameModel> getAllAccessName()
        {
            return _iConfigAccessInterfaceDA.getAllAccessName();
        }

        public List<GroupAccessNameModel> getAccessNameRoleId(int roleid)
        {
            return _iConfigAccessInterfaceDA.getAccessNameRoleId(roleid);
        }
    }
}

