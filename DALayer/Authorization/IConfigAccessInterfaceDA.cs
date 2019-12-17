using SCMModels.SCMModels;
using System;
using System.Collections.Generic;
using System.Linq;
using SCMAPI.Models;
using System.Text;
using System.Threading.Tasks;

namespace DALayer.MPR
{
   public interface IConfigAccessInterfaceDA
    {
        List<GroupMasterDomainModel> getGroupMasterDetail();
        Task<string> AddNewGroupMaster(GroupMasterDomainModel model);
        Task<string> UpdateGroupMaster(UpdateGroupMasterDomainModel model);
        Task<string> DeleteGroupMaster(DeleteGroupMasterDomainModel model);
        List<GroupMasterDomainModel> getGroupNameDetail();
        //GroupAccessNameModel getAccessName(string AccessGroupId);
        Task<string> AddRoleAccess(AddRoleDomainModel model);
        List<AddRoleDomainModel> getAllRoleDetail();
        List<GroupAccessNameModel> getAccessNameById(int accessGroupId);
        Task<string> DeleteGroupAccess(DeleteGroupAccessDomainModel model);
        Task<string> AddNewAccessName(GroupAccessNameModel model);
        Task<string> UpdateAccessName(UpdateAccessNameDomainModel model);
        List<GetAllNameModel> getAllGroupById(int roleId);
        List<AutorizationGroupModel> getAllRoleName();
        string AddAccessRole(checkboxSelect selectCheckbox); 
        List<GroupAccessNameModel> getGroupAccessNameById(int accessGroupId);
        Task<string> AddAccess(AddAccessDomainModel model);
        Task<string> UpdateAccess(AuthorizationItemsModel model);
        List<AuthorizationItemsModel> getAuthorizationItemDetailById(int roleid);
        List<AuthorizationItemsModel> getAuthorizationItemDetail();
        Task<string> UpdateAuthRole(UpdateAuthGroupDomainModel model);
        Task<string> DeleteAuthRole(DeleteAuthGroupDomainModel model);
        Task<string> DeleteAccessName(DeleteAccessNameDomainModel model);
        List<GroupAccessNameModel> getAllAccessName();
        List<GroupAccessNameModel> getAccessNameRoleId(int roleid);
        List<GroupAccessNameModel> getAllAccessNameData();
    }
}
