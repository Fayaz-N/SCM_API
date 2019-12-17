using SCMAPI.Models;
using SCMModels.SCMModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALayer.MPR
{
   public class ConfigAccessRepoDA : IConfigAccessInterfaceDA
    {
        YSCMEntities DB = new YSCMEntities();
       

        public List<GroupMasterDomainModel> getGroupMasterDetail()
        {
            using(var _context = new YSCMEntities())
            {
                try
                {
                    var result = from accs in _context.AccessGroupMasters
                                 where accs.DeleteFlag == false

                                 select new GroupMasterDomainModel()
                                 {
                                     GroupName = accs.GroupName,
                                     AccessGroupId = accs.AccessGroupId
                                 };

                    return result.ToList();        

                }
                catch (Exception ex)
                { throw ex; }
            }
           
        }

        public async Task<string> AddNewGroupMaster(GroupMasterDomainModel model)
        {
            using (var _context = new YSCMEntities())
            {
                try
                {
                    if (model.GroupName != "")
                    { var some1 = _context.AccessGroupMasters.Where(x => model.GroupName.Contains(x.GroupName)).ToList();
                        if (some1.Count == 0)
                        {
                            _context.AccessGroupMasters.Add(new AccessGroupMaster
                        {
                            GroupName = model.GroupName,
                            UpdatedBy = model.UpdatedBy,
                            UpdatedDate = model.UpdatedDate
                        });
                        await _context.SaveChangesAsync();
                        
                    }
                        else
                        {
                            return "Group Name Exist Please try with other Group Name";
                        }
                        return "Data save sucessfully";
                    }
                    else
                    {
                        return "Can't save null data";
                    }
                }
                catch (Exception ex)
                { throw ex; }
            }
        }

      
        public async Task<string> UpdateGroupMaster(UpdateGroupMasterDomainModel model)
        {
            using (var _context = new YSCMEntities())
            {
                try
                {
                    var existingGroup = _context.AccessGroupMasters.Where(e => e.AccessGroupId == model.AccessGroupId).FirstOrDefault<AccessGroupMaster>();


                   if (existingGroup != null)
                    {
                        existingGroup.UpdatedBy = "190455";
                        existingGroup.UpdatedDate = DateTime.Now;
                        existingGroup.GroupName = model.GroupName;
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return "No Record Found";
                    }
                    return "Data Updated Successfully";
                }
                catch (Exception ex)
                { throw ex; }
            }
        }

      
        public async Task<string> DeleteGroupMaster(DeleteGroupMasterDomainModel model)
        {
            using (var _context = new YSCMEntities())
            {
                try
                {
                    var res = _context.AccessGroupMasters.Where(e => e.GroupName == model.GroupName).FirstOrDefault<AccessGroupMaster>();
                  if(res != null)
                    {
                        res.DeleteFlag = true;
                        res.DeleteDate = DateTime.Now;
                        res.DeletedBy = "190455";
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return "No Record Found";
                    }
                    return "Data Updated SuccessFully";
                   
                }
                catch (Exception ex)
                { throw ex; }
            }
        }

        public List<GroupMasterDomainModel> getGroupNameDetail()
        {
            using (var _context = new YSCMEntities())
            {
                try
                {
                    var result = from ag in _context.AccessGroupMasters
                                 where ag.DeleteFlag == false

                                 select new GroupMasterDomainModel()
                                 {
                                     AccessGroupId = ag.AccessGroupId,
                                     GroupName = ag.GroupName
                                 };

                    return result.ToList();

                }
                catch (Exception ex)
                { throw ex; }
            }

        }


        public async Task<string> AddNewAccessName(GroupAccessNameModel model)
        {
            using (var _context = new YSCMEntities())
            {
                try
                {
                    if (model.AccessName != "")
                    {
                        var some1 = _context.AccessNames.Where(x => model.AccessName.Contains(x.AccessName1)).ToList();
                        if (some1.Count == 0)
                        {
                            _context.AccessNames.Add(new AccessName
                            {
                                AccessName1 = model.AccessName,
                                AccessGroupId = model.AccessGroupId,
                                updatedBy = "190455",
                                updatedDate = DateTime.Now
                            });
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            return "Access Name Exist Please try with other Access Name";
                        }
                        return "Data save sucessfully";
                    }
                    else
                    {
                        return "Can't save null data";
                    }
                }
                catch (Exception ex)
                { throw ex; }
            }
        }

       //Updated AccessName on edit Click
        public async Task<string> UpdateAccessName(UpdateAccessNameDomainModel model)
        {
            using (var _context = new YSCMEntities())
            {
                try
                {
                    var obj = _context.AccessNames.Where(e => e.AccessNameID == model.AccessNameID).FirstOrDefault<AccessName>();


                    if (obj != null)
                    {
                        obj.AccessName1 = model.AccessName;
                        obj.updatedBy = "190455";
                        obj.updatedDate = DateTime.Now;
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return "No Record Found";
                    }
                    return "Data Updated Successfully";
                }
                catch (Exception ex)
                { throw ex; }
            }
        }

        //Delete AccessName on delete Click
        public async Task<string> DeleteAccessName(DeleteAccessNameDomainModel model)
        {
            using (var _context = new YSCMEntities())
            {
                try
                {
                    var obj = _context.AccessNames.Where(e => e.AccessNameID == model.AccessNameID).FirstOrDefault<AccessName>();


                    if (obj != null)
                    {
                        //obj.AccessName = model.AccessName;
                        obj.DeletedBy = "190455";
                        obj.DeleteFlag = true;
                        obj.DeletedDate = DateTime.Now;
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return "No Record Found";
                    }
                    return "Data Updated Successfully";
                }
                catch (Exception ex)
                { throw ex; }
            }
        }

        public List<GroupAccessNameModel> getAccessNameById(int accessGroupId)
        {
            //List<GroupAccessNameModel> grpAcsModel = new List<GroupAccessNameModel>();
            using(var _context = new YSCMEntities())
            {
                try
                {
                    var result = from an in _context.AccessNames
                                 join agm in _context.AccessGroupMasters on an.AccessGroupId equals agm.AccessGroupId
                                 where an.AccessGroupId == accessGroupId && an.DeleteFlag == false
                                 select new GroupAccessNameModel()
                                 {
                                     AccessNameID = an.AccessNameID,
                                     AccessName = an.AccessName1,
                                     AccessGroupId = an.AccessGroupId,
                                     AccessGroupMaster = new GroupMasterDomainModel()
                                     {
                                         GroupName = agm.GroupName,
                                         AccessGroupId = agm.AccessGroupId
                                     },
                                     AuthorizationItems = new AuthorizationItemsModel()
                                     {
                                         RoleId = 0,
                                         DeleteFlag = false,
                                         
                                 }
                                 };
                    
                    //grpAcsModel.Add(result.ToList());
                   return result.ToList();


                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }

        }
       
        public List<GetAllNameModel> getAllGroupById(int roleId)
        {

            using (var _context = new YSCMEntities())
            {
                try
                {
                    var result = from agm in _context.AccessGroupMasters
                                 join an in _context.AccessNames on agm.AccessGroupId equals an.AccessGroupId
                                 join ai in _context.AuthorizationItems on an.AccessNameID equals ai.AccessNamesId
                                 into temp from NetTcpStyleUriParser in temp.DefaultIfEmpty()
                                 join ag in _context.AutorizationGroups on NetTcpStyleUriParser.RoleId equals ag.RoleId
                                 where ag.RoleId == roleId && ag.DeleteFlag == false 
                                 select new GetAllNameModel()
                                 {
                                        GroupName = agm.GroupName,
                                     //AccessViewModel = new GroupAccessNameModel
                                     //{
                                         AccessNamesId = an.AccessNameID,
                                         AccessGroupId = an.AccessGroupId,
                                         AccessName = an.AccessName1,
                                        // AccessNameStatus = an.AccessNameStatus

                                     //}

                                 };
         



                    // };
                    return result.Distinct()
                        .ToList();


                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }

        public async Task<string> DeleteGroupAccess(DeleteGroupAccessDomainModel model)
        {
            using (var _context = new YSCMEntities())
            {
                try
                {
                    var res = _context.AccessNames.Where(e => e.AccessName1 == model.AccessName).FirstOrDefault<AccessName>();
                    if (res != null)
                    {
                        res.DeleteFlag = true;
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return "No Record Found";
                    }
                    return "Data Updated SuccessFully";

                }
                catch (Exception ex)
                { throw ex; }
            }
        }
        public async Task<string> AddRoleAccess(AddRoleDomainModel model)
        {
            using (var _context = new YSCMEntities())
            {
                try
                {
                    if (model.RoleName != "")
                    {
                        var some1 = _context.AutorizationGroups.Where(x => model.RoleName.Contains(x.RoleName)).ToList();
                        if (some1.Count == 0)
                        {
                            _context.AutorizationGroups.Add(new AutorizationGroup
                            {
                                RoleName = model.RoleName,
                                UpdatedBy = model.UpdatedBy,
                                updatedDate = model.updatedDate
                            });
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            return "Role Name Exist Please try with other Role Name";
                        }
                        return "Data save sucessfully";
                    }
                    else
                    {
                        return "Can't save null data";
                    }
                }
                catch (Exception ex)
                { throw ex; }
            }
        }

        public string AddAccessRole(checkboxSelect selectCheckbox)
        {
            using (var _context = new YSCMEntities())
            {
                try
                {
                    if (selectCheckbox.uncheckResult != null)
                    {
                        var some1 = _context.AuthorizationItems.Where(x => selectCheckbox.uncheckResult.Contains(x.AccessNamesId)).ToList();
                        if (some1 != null)
                        {
                            foreach (var data1 in some1)
                            {


                                //AccessNamesId = data2.AccessNameID,
                                //RoleId = roleid,
                                data1.DeletedBy = "190455";
                                data1.Deleteddate = DateTime.Now;
                                data1.DeleteFlag = true;

                                _context.SaveChanges();

                            }
                        }
                    }
                    if (selectCheckbox.resultText != null)
                    {
                        var some = _context.AuthorizationItems.Where(x => selectCheckbox.resultText.Contains(x.AccessNamesId)).ToList();
                        if (some != null)
                        {
                            foreach (var data2 in selectCheckbox.resultText)
                            {


                                _context.AuthorizationItems.Add(new AuthorizationItem
                                {

                                    AccessNamesId = data2,
                                    RoleId = selectCheckbox.roleId,
                                    UpdatedBy = "190455",
                                    Updateddate = DateTime.Now,
                                    DeleteFlag = false
                                });
                                _context.SaveChanges();

                            }

                        }
                    }
                    //some.ForEach(a => a.AccessNameStatus = true);
                    return "Data save sucessfully";
                }
                catch (Exception ex)
                { throw ex; }
            }
        }

        public List<AddRoleDomainModel> getAllRoleDetail()
        {
            using (var _context = new YSCMEntities())
            {
                try
                {
                    var result = from ag in _context.AutorizationGroups
                                 where ag.DeleteFlag == false

                                 select new AddRoleDomainModel()
                                 {
                                     RoleName = ag.RoleName,
                                     RoleId = ag.RoleId
                                 };

                    return result.ToList();

                }
                catch (Exception ex)
                { throw ex; }
            }

        }

        public List<AuthorizationItemsModel> getAuthorizationItemDetailById(int roleid)
        {
            using (var _context = new YSCMEntities())
            {
                try
                {
                    var result = from auth in _context.AuthorizationItems
                                 where auth.DeleteFlag == false && auth.RoleId == roleid

                                 select new AuthorizationItemsModel()
                                 {
                                     RoleAccessNameid =auth.RoleAccessNameid,
                                     RoleId = auth.RoleId,
                                     AccessNamesId = auth.AccessNamesId,
                                     DeleteFlag = auth.DeleteFlag
                                 };

                    return result.ToList();

                }
                catch (Exception ex)
                { throw ex; }
            }

        }

        public List<AuthorizationItemsModel> getAuthorizationItemDetail()
        {
            using (var _context = new YSCMEntities())
            {
                try
                {
                    var result = from auth in _context.AuthorizationItems
                                 where auth.DeleteFlag == false
                                

                                 select new AuthorizationItemsModel()
                                 {
                                     RoleAccessNameid = auth.RoleAccessNameid,
                                     RoleId = auth.RoleId,
                                     AccessNamesId = auth.AccessNamesId,
                                     DeleteFlag = auth.DeleteFlag
                                 };

                    return result.ToList();

                }
                catch (Exception ex)
                { throw ex; }
            }

        }


        public List<AutorizationGroupModel> getAllRoleName()
        {
            using (var _context = new YSCMEntities())
            {
                try
                {
                    var result = from ag in _context.AutorizationGroups
                                 where ag.DeleteFlag == false

                                 select new AutorizationGroupModel()
                                 {
                                     RoleName = ag.RoleName,
                                     RoleId = ag.RoleId
                                 };

                    return result.ToList();

                }
                catch (Exception ex)
                { throw ex; }
            }

        }

        public List<GroupAccessNameModel> getGroupAccessNameById(int accessGroupId)
        {

            using (var _context = new YSCMEntities())
            {
                try
                {
                    var result = from an in _context.AccessNames
                                 join agm in _context.AccessGroupMasters on an.AccessGroupId equals agm.AccessGroupId
                                 join auth in _context.AuthorizationItems on an.AccessNameID equals auth.AccessNamesId
                                 where an.AccessGroupId == accessGroupId && an.DeleteFlag == false
                                 select new GroupAccessNameModel()
                                 {
                                     AccessNameID = an.AccessNameID,
                                     AccessName = an.AccessName1,
                                     AccessGroupId = an.AccessGroupId,

                                     AccessGroupMaster = new GroupMasterDomainModel()
                                     {
                                         GroupName = agm.GroupName,
                                         AccessGroupId = agm.AccessGroupId
                                     },
                                     AuthorizationItems = new AuthorizationItemsModel()
                                     {
                                         RoleId = auth.RoleId,
                                         AccessNamesId = auth.AccessNamesId,
                                         DeleteFlag = auth.DeleteFlag
                                     }


                                 };
                    
                    return result.ToList();
                    

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }

      
        
        //Add access to authitem tbl
        public async Task<string> AddAccess(AddAccessDomainModel model)
        {
            using (var _context = new YSCMEntities())
            {
                try
                {
                    _context.AuthorizationItems.Add(new AuthorizationItem
                    {
                        
                        AccessNamesId = model.AccessNameID,
                        RoleId =model.RoleId,
                        UpdatedBy = model.UpdatedBy,
                        Updateddate = model.updatedDate,
                        DeleteFlag = model.DeleteFlag
                    });
                    await _context.SaveChangesAsync();
                    return "Data save sucessfully";
                }
                catch (Exception ex)
                { throw ex; }
            }
        }

        //Update access to authitem tbl
        //public string UpdateAccess(AuthorizationItemsModel model)
        //{
        //    using (var _context = new YSCMEntities())
        //    {
        //        try
        //        {
        //            var some = _context.AuthorizationItems.Where(x => model.Contains(x.AccessNamesId)).ToList();
        //            some.ForEach(a => a.DeleteFlag = true);
        //            _context.SaveChanges();
        //            return "Data save sucessfully";
        //        }
        //        catch (Exception ex)
        //        { throw ex; }
        //    }
        //}

        public async Task<string> UpdateAccess(AuthorizationItemsModel model)
        {
            using (var _context = new YSCMEntities())
            {
                try
                {
                    var obj = _context.AuthorizationItems.Where(e => e.AccessNamesId == model.AccessNamesId).FirstOrDefault<AuthorizationItem>();


                    if (obj != null)
                    {
                        obj.DeleteFlag = model.DeleteFlag;
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return "No Record Found";
                    }
                    return "Data Updated Successfully";
                }
                catch (Exception ex)
                { throw ex; }
            }
        }

        //Update AuthRole
        public async Task<string> UpdateAuthRole(UpdateAuthGroupDomainModel model)
        {
            using (var _context = new YSCMEntities())
            {
                try
                {
                    var existingGroup = _context.AutorizationGroups.Where(e => e.RoleId == model.RoleId).FirstOrDefault<AutorizationGroup>();


                    if (existingGroup != null)
                    {
                        existingGroup.RoleName = model.RoleName;
                        existingGroup.UpdatedBy = "190455";
                        existingGroup.updatedDate = DateTime.Now;
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return "No Record Found";
                    }
                    return "Data Updated Successfully";
                }
                catch (Exception ex)
                { throw ex; }
            }
        }


        //Delete AuthRole
        public async Task<string> DeleteAuthRole(DeleteAuthGroupDomainModel model)
        {
            using (var _context = new YSCMEntities())
            {
                try
                {
                    var res = _context.AutorizationGroups.Where(e => e.RoleId == model.RoleId).FirstOrDefault<AutorizationGroup>();
                    if (res != null)
                    {
                        res.DeleteFlag = true;
                        res.DeletedDate = DateTime.Now;
                        res.DeletedBy = "190455";
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return "No Record Found";
                    }
                    return "Data Updated SuccessFully";

                }
                catch (Exception ex)
                { throw ex; }
            }
        }


        public List<GroupAccessNameModel> getAllAccessName()
        {

            using (var _context = new YSCMEntities())
            {
                try
                {
                    var result = from an in _context.AccessNames
                                 join agm in _context.AccessGroupMasters on an.AccessGroupId equals agm.AccessGroupId
                                 where  an.DeleteFlag == false && agm.DeleteFlag==false orderby an.AccessGroupId ascending
                                 select new GroupAccessNameModel()
                                 {
                                     AccessNameID = an.AccessNameID,
                                     AccessName = an.AccessName1,
                                     AccessGroupId = an.AccessGroupId,
                                     AccessGroupMaster = new GroupMasterDomainModel()
                                     {
                                         GroupName = agm.GroupName,
                                         AccessGroupId = agm.AccessGroupId
                                     },
                                     AuthorizationItems = new AuthorizationItemsModel()
                                     {
                                         RoleId = 0,
                                         DeleteFlag = false,

                                     }
                                 };
                    return result.ToList();


                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }

        //Get all accessname by roleid for view access page
        public List<GroupAccessNameModel> getAccessNameRoleId(int roleid)
        {
            using (var _context = new YSCMEntities())
            {
                try
                {
                    var result = from an in _context.AccessNames                                
                                 join agr in _context.AccessGroupMasters on an.AccessGroupId equals agr.AccessGroupId
                                 join ai in _context.AuthorizationItems on an.AccessNameID equals ai.AccessNamesId
                                 where ai.RoleId == roleid && ai.DeleteFlag == false
                                 select new GroupAccessNameModel()
                                 {
                                     AccessNameID = an.AccessNameID,
                                     AccessName = an.AccessName1,
                                     AccessGroupId = an.AccessGroupId,
                                     AccessGroupMaster = new GroupMasterDomainModel()
                                     {
                                         GroupName = agr.GroupName,
                                         AccessGroupId = agr.AccessGroupId
                                     },
                                     AuthorizationItems = new AuthorizationItemsModel()
                                     {
                                         RoleId = ai.RoleId,
                                         AccessNamesId = ai.AccessNamesId,
                                         DeleteFlag = ai.DeleteFlag

                                     }
                                 };

                    //grpAcsModel.Add(result.ToList());
                    return result.ToList();


                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }

        public List<GroupAccessNameModel> getAllAccessNameData()
        {

            using (var _context = new YSCMEntities())
            {
                try
                {
                    var result = from an in _context.AccessNames
                                 join agm in _context.AccessGroupMasters on an.AccessGroupId equals agm.AccessGroupId
                                 where an.DeleteFlag == false && agm.DeleteFlag == false
                                 orderby an.AccessGroupId ascending
                                 select new GroupAccessNameModel()
                                 {
                                     AccessNameID = an.AccessNameID,
                                     AccessName = an.AccessName1,
                                     AccessGroupId = an.AccessGroupId,
                                     AccessGroupMaster = new GroupMasterDomainModel()
                                     {
                                         GroupName = agm.GroupName,
                                         AccessGroupId = agm.AccessGroupId
                                     },
                                     AuthorizationItems = new AuthorizationItemsModel()
                                     {
                                         RoleId = 0,
                                         DeleteFlag = false,

                                     }
                                 };
                    return result.ToList();


                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }
    }
}
