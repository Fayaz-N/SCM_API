using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCMAPI.Models
{
    public class GroupMasterViewModel
    {
        public int AccessGroupId { get; set; }
        public string GroupName { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string DeletedBy { get; set; }
        public Nullable<System.DateTime> DeleteDate { get; set; }
        public string DeletedRemarks { get; set; }
        public bool DeleteFlag { get; set; }
    }

    public class GroupMasterDomainModel
    {
        public GroupMasterDomainModel()
        {
            this.AccessName = new HashSet<GroupAccessNameModel>();
        }
        public int AccessGroupId { get; set; }
        public string GroupName { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string DeletedBy { get; set; }
        public Nullable<System.DateTime> DeleteDate { get; set; }
        public string DeletedRemarks { get; set; }
        public bool DeleteFlag { get; set; }
        public virtual ICollection<GroupAccessNameModel> AccessName { get; set; }
    }

    public class UpdateGroupMasterViewModel
    {
        public string GroupName { get; set; }
        public int AccessGroupId { get; set; }
    }

    public class UpdateGroupMasterDomainModel
    {
        public string GroupName { get; set; }
        public int AccessGroupId { get; set; }
    }


    public class DeleteGroupMasterViewModel
    {
        public bool DeleteFlag { get; set; }
        public string GroupName { get; set; }
    }

    public class DeleteGroupMasterDomainModel
    {
        public bool DeleteFlag { get; set; }
        public string GroupName { get; set; }
    }

    public class AccessViewModel
    {
        //public int AccessNameID { get; set; }
        public bool DeleteFlag { get; set; }
        public string GroupName { get; set; }
        public DateTime updatedDate { get; set; }
        public int AccessGroupId { get; set; }
        public string updatedBy { get; set; }
        public string AccessName { get; set; }
        public int AccessNameID { get; set; }
    }

    public class DeleteGroupAccessDomainModel
    {
        public bool DeleteFlag { get; set; }
        public string AccessName { get; set; }
        public int AccessNameID { get; set; }
    }

    public class GroupAccessNameModel
    {
        public int AccessNameID { get; set; }
        public int AccessGroupId { get; set; }
        public string AccessName { get; set; }
        public DateTime updatedDate { get; set; }
        public bool DeleteFlag { get; set; }
       // public bool    { get; set; }
        public string updatedBy { get; set; }
        public int RoleId { get; set; }
        public virtual GroupMasterDomainModel AccessGroupMaster { get; set; }
        public virtual AuthorizationItemsModel AuthorizationItems { get; set; }


    }

    public class AddRoleViewModel
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> updatedDate { get; set; }
        public string DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public string DeletedRemarks { get; set; }
        public bool DeleteFlag { get; set; }
    }

    public class AddRoleDomainModel
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> updatedDate { get; set; }
        public string DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public string DeletedRemarks { get; set; }
        public bool DeleteFlag { get; set; }
    }

    
    public class UpdateAccessNameDomainModel
    {
        public string AccessName { get; set; }
        public int AccessNameID { get; set; }
        public string updatedBy { get; set; }
        public DateTime updatedDate { get; set; }

    }

    public class DeleteAccessNameDomainModel
    {
        //public string AccessName { get; set; }
        public int AccessNameID { get; set; }
        public string DeletedBy { get; set; }
        public DateTime DeletedDate { get; set; }
           public bool DeleteFlag { get; set; }

    }

    public class AuthorizationItemsModel
    {
        public int RoleAccessNameid { get; set; }
        public int RoleId { get; set; }
        public int AccessNamesId { get; set; }
        public bool DeleteFlag { get; set; }
      

    }

    public class AutorizationGroupModel
    {
        public string RoleName { get; set; }
        public int RoleId { get; set; }

    }

    public class GetAllNameModel
    {
        public string GroupName { get; set; }
        public int RoleAccessNameid { get; set; }
        public int RoleId { get; set; }
        public int AccessNamesId { get; set; }
        public int AccessGroupId { get; set; }
        public string AccessName { get; set; }
        //public bool AccessNameStatus { get; set; }
        public virtual GroupAccessNameModel AccessViewModel { get; set; }
        public virtual AutorizationGroupModel AutorizationGroupModel { get; set; }
        


    }

    public class AddAccessDomainModel
    {
        public string AccessName { get; set; }
        public int AccessNameID { get; set; }
        public int  RoleId { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime updatedDate { get; set; }
        public bool DeleteFlag { get; set; }

    }
    public class AddAccessViewModel
    {
        public string AccessName { get; set; }
        public int AccessNameID { get; set; }
        public int RoleId { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime updatedDate { get; set; }
        public bool DeleteFlag { get; set; }

    }

    public class UpdateAuthGroupDomainModel
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public DateTime updatedDate { get; set; }
    }

    public class DeleteAuthGroupDomainModel
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string DeletedBy { get; set; }
        public DateTime DeletedDate { get; set; }
        public bool DeleteFlag { get; set; }
    }

    public class  checkboxSelect{
        public int roleId { get; set; }
        public int [] resultText { get; set; }
        public int[] uncheckResult { get; set; }

    }
}