using DALayer.Emails;
using DALayer.MPR;
using DALayer.PAEmailDA;
using SCMModels;
using SCMModels.MPRMasterModels;
using SCMModels.RemoteModel;
using SCMModels.RFQModels;
using SCMModels.SCMModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALayer.PurchaseAuthorization
{
    public class PurchaseAuthorizationDA : IPurchaseAuthorizationDA
    {
        private IPAEmailDA emailDA = default(IPAEmailDA);
        public PurchaseAuthorizationDA(IPAEmailDA EmailDA)
        {
            this.emailDA = EmailDA;
        }
        VSCMEntities vscm = new VSCMEntities();
        YSCMEntities obj = new YSCMEntities();
        //inserting pa limits
        public async Task<statuscheckmodel> InsertPAAuthorizationLimits(PAAuthorizationLimitModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                var data = new PAAuthorizationLimit();
                if (model != null)
                {
                    data.DeptId = model.DeptId;
                    data.AuthorizationType = model.AuthorizationType;
                    data.MinPAValue = model.MinPAValue;
                    data.MaxPAValue = model.MaxPAValue;
                    data.CreatedBy = model.CreatedBy;
                    data.CreatedDate = System.DateTime.Now;
                    data.DeletedBY = model.DeletedBY;
                    data.DeletedDate = model.DeletedDate;
                }
                obj.PAAuthorizationLimits.Add(data);
                obj.SaveChanges();
                int id = data.Authid;
                status.Sid = id;
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //bulk insert palimits
        public async Task<statuscheckmodel> BulkInsertPAAuthorizationLimits(List<PAAuthorizationLimitModel> model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                var data = new PAAuthorizationLimit();
                foreach (var item in model)
                {
                    data.AuthorizationType = item.AuthorizationType;
                    data.DeptId = item.DeptId;
                    data.MinPAValue = item.MinPAValue;
                    data.MaxPAValue = item.MaxPAValue;
                    data.CreatedBy = item.CreatedBy;
                    data.CreatedDate = item.CreatedDate;
                    obj.PAAuthorizationLimits.Add(data);
                    obj.SaveChanges();
                }
                int id = data.Authid;
                status.Sid = id;
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //get authorization limits by deptid
        public async Task<PAAuthorizationLimitModel> GetPAAuthorizationLimitById(int deptid)
        {
            PAAuthorizationLimitModel model = new PAAuthorizationLimitModel();
            try
            {
                var data = obj.PAAuthorizationLimits.Where(x => x.Authid == deptid && x.DeleteFlag == false).FirstOrDefault();
                model.DeptId = data.DeptId;
                model.MaxPAValue = data.MaxPAValue;
                model.MinPAValue = data.MinPAValue;
                model.AuthorizationType = data.AuthorizationType;
                model.DeletedBY = data.DeletedBY;
                model.DeletedDate = data.DeletedDate;
                model.CreatedBy = data.CreatedBy;
                model.CreatedDate = data.CreatedDate;
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //employee mapping to the palimits
        public async Task<statuscheckmodel> CreatePAAuthirizationEmployeeMapping(PAAuthorizationEmployeeMappingModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                var mapping = new PAAuthorizationEmployeeMapping();
                if (model != null)
                {
                    mapping.Authid = model.Authid;
                    mapping.FunctionalRoleId = model.FunctionalRoleId;
                    mapping.CreatedBY = model.CreatedBY;
                    mapping.CreatedDate = System.DateTime.Now;
                    mapping.Employeeid = model.Employeeid;
                    mapping.LessBudget = model.LessBudget;
                    mapping.MoreBudget = model.MoreBudget;
                    mapping.DeletedBy = model.DeletedBy;
                    mapping.DeletedDate = model.DeletedDate;
                }
                obj.PAAuthorizationEmployeeMappings.Add(mapping);
                obj.SaveChanges();
                int mapid = mapping.PAmapid;
                status.Sid = mapid;
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<PAAuthorizationEmployeeMappingModel> GetMappingEmployee(PAAuthorizationLimitModel limit)
        {
            PAAuthorizationEmployeeMappingModel model = new PAAuthorizationEmployeeMappingModel();
            try
            {
                var authdata = obj.PAAuthorizationLimits.Where(x => x.Authid == limit.Authid && x.MinPAValue >= limit.MinPAValue && x.MaxPAValue <= limit.MaxPAValue).FirstOrDefault();
                var mappingdata = obj.PAAuthorizationEmployeeMappings.Where(x => x.Authid == authdata.Authid && x.DeleteFlag == false).FirstOrDefault();
                var employeedata = obj.Employees.Where(x => x.EmployeeNo == mappingdata.Employeeid).FirstOrDefault();
                model.Employeeid = mappingdata.Employeeid;
                model.Employeename = employeedata.Name;
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //adding credit days
        public async Task<statuscheckmodel> CreatePACreditDaysmaster(PACreditDaysMasterModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            var credit = new PACreditDaysMaster();
            try
            {
                if (model != null)
                {
                    credit.MinDays = model.MinDays;
                    credit.MaxDays = model.MaxDays;
                    credit.DeletedBy = model.DeletedBy;
                    credit.DeletedDate = model.DeletedDate;
                    credit.CreatedBy = model.CreatedBy;
                    credit.CreatedDate = System.DateTime.Now;
                }
                obj.PACreditDaysMasters.Add(credit);
                obj.SaveChanges();
                int creditid = credit.CreditDaysid;
                status.Sid = creditid;
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //get credit days by credit id
        public async Task<PACreditDaysMasterModel> GetCreditdaysMasterByID(int creditdaysid)
        {
            PACreditDaysMasterModel model = new PACreditDaysMasterModel();
            try
            {
                var creditdata = obj.PACreditDaysMasters.Where(x => x.CreditDaysid == creditdaysid).FirstOrDefault();
                if (creditdata != null)
                {
                    model.CreditDaysid = creditdata.CreditDaysid;
                    model.MinDays = creditdata.MinDays;
                    model.MaxDays = creditdata.MaxDays;
                    model.CreatedBy = creditdata.CreatedBy;
                    model.CreatedDate = creditdata.CreatedDate;
                    model.DeletedBy = creditdata.DeletedBy;
                    model.DeletedDate = creditdata.DeletedDate;
                }
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //mapping credit days to employee
        public async Task<statuscheckmodel> AssignCreditdaysToEmployee(PACreditDaysApproverModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                ////var data = obj.PAAuthorizationLimits.Where(x => x.Authid == model.AuthId).FirstOrDefault();
                var creditapprover = new PACreditDaysApprover();
                creditapprover.AuthId = model.AuthId;
                creditapprover.EmployeeNo = model.EmployeeNo;
                creditapprover.CreditdaysId = Convert.ToByte(model.CreditdaysId);
                creditapprover.Createdby = model.Createdby;
                creditapprover.CreatedDate = System.DateTime.Now;
                creditapprover.DeletedBy = model.DeletedBy;
                creditapprover.DeletedDate = model.DeletedDate;
                obj.PACreditDaysApprovers.Add(creditapprover);
                obj.SaveChanges();
                int approvalid = creditapprover.CRApprovalId;
                status.Sid = approvalid;
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //remove pa limits by Auth id
        public async Task<statuscheckmodel> RemovePAAuthorizationLimitsByID(int authid)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                var removedata = obj.PAAuthorizationLimits.Where(x => x.Authid == authid && x.DeleteFlag == false).FirstOrDefault();
                if (removedata != null)
                {
                    removedata.DeleteFlag = true;
                    obj.SaveChanges();
                }
                var mappingdata = obj.PAAuthorizationEmployeeMappings.Where(x => x.Authid == removedata.Authid && x.DeleteFlag == false).ToList();
                if (mappingdata != null)
                {
                    foreach (var item in mappingdata)
                    {
                        item.DeleteFlag = true;
                        obj.SaveChanges();
                    }
                }
                int id = removedata.Authid;
                status.Sid = id;
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //removing pa creditdays by creditid
        public async Task<statuscheckmodel> RemovePACreditDaysMaster(int creditid)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                var creditdata = obj.PACreditDaysMasters.Where(x => x.CreditDaysid == creditid && x.DeleteFlag == false).FirstOrDefault();
                if (creditdata != null)
                {
                    creditdata.DeleteFlag = true;
                    obj.SaveChanges();
                }
                int id = creditdata.CreditDaysid;
                status.Sid = id;
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<PAAuthorizationLimitModel>> GetPAAuthorizationLimitsByDeptId(int departmentid)
        {
            List<PAAuthorizationLimitModel> model = new List<PAAuthorizationLimitModel>();
            try
            {
                model = obj.PAAuthorizationLimits.Where(x => x.DeptId == departmentid && x.DeleteFlag == false).Select(x => new PAAuthorizationLimitModel
                {
                    MinPAValue = x.MinPAValue,
                    MaxPAValue = x.MaxPAValue
                }).ToList();
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //removing employee assigned to pa credit days
        public async Task<statuscheckmodel> RemovePACreditDaysApprover(EmployeemappingtocreditModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                var data = obj.PACreditDaysApprovers.Where(x => x.CRApprovalId == model.CRApprovalId).FirstOrDefault();
                if (data != null)
                {
                    data.DeleteFlag = true;
                    obj.SaveChanges();
                }
                status.Sid = data.AuthId;
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<statuscheckmodel> RemovePurchaseApprover(EmployeemappingtopurchaseModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                var data = obj.PAAuthorizationEmployeeMappings.Where(x => x.PAmapid == model.PAmapid).FirstOrDefault();
                if (data != null)
                {
                    data.DeleteFlag = true;
                    obj.SaveChanges();
                }
                status.Sid = data.Authid;
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //getting credit days approver by approval id
        public async Task<PACreditDaysApproverModel> GetPACreditDaysApproverById(int ApprovalId)
        {
            PACreditDaysApproverModel model = new PACreditDaysApproverModel();
            try
            {
                var data = obj.PACreditDaysApprovers.Where(x => x.CRApprovalId == ApprovalId).FirstOrDefault();
                if (data != null)
                {
                    model.AuthId = data.AuthId;
                    model.EmployeeNo = data.EmployeeNo;
                    model.CreditdaysId = data.CreditdaysId;
                    model.Createdby = data.Createdby;
                    model.CreatedDate = data.CreatedDate;
                    model.DeletedBy = data.DeletedBy;
                    model.DeletedDate = data.DeletedDate;
                    model.CRApprovalId = data.CRApprovalId;
                }
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //public async Task<List<EmployeModel>> GetEmployeeMappings(PAConfigurationModel model)
        //{
        //    List<EmployeModel> employee = new List<EmployeModel>();
        //    model.PAValue = model.UnitPrice;
        //    if (model.PAValue > model.TargetSpend)
        //    {
        //        model.Budgetvalue = false;
        //    }
        //    else
        //    {
        //        model.Budgetvalue = true;
        //    }
        //    try
        //    {
        //        if (model != null)
        //        {
        //            //var padata = obj.PAAuthorizationLimits.Where(x => x.MinPAValue >= model.PAValue && x.MaxPAValue <= model.PAValue).FirstOrDefault();
        //            var padata = obj.PAAuthorizationLimits.Where(x => x.MinPAValue.CompareTo(model.PAValue) <= 0 && x.MaxPAValue.CompareTo(model.PAValue) >= 0 && x.DeptId==model.DeptId).FirstOrDefault();
        //            if (padata != null)
        //            {
        //                //string.Equals(padata.AuthorizationType,"pa", StringComparison.CurrentCultureIgnoreCase);
        //                //padata.AuthorizationType.Contains( StringComparison.CurrentCultureIgnoreCase);
        //                if (padata.AuthorizationType.ToLower().Equals("pa"))
        //                {
        //                    var mappingdata = obj.PAAuthorizationEmployeeMappings.Where(x => x.Authid == padata.Authid).FirstOrDefault();
        //                    if (mappingdata != null)
        //                    {
        //                        var employeedata = obj.Employees.Where(x => x.EmployeeNo == mappingdata.Employeeid).ToList();
        //                        employee = employeedata.Select(x => new EmployeModel()
        //                        {
        //                            EmployeeNo = x.EmployeeNo,
        //                            Name = x.Name
        //                        }).ToList();
        //                    }
        //                }
        //                else
        //                {
        //                    var creditdata = obj.PACreditDaysApprovers.Where(x => x.AuthId == padata.Authid).FirstOrDefault();
        //                    var employeedata = obj.Employees.Where(x => x.EmployeeNo == creditdata.EmployeeNo).ToList();
        //                    if (creditdata != null)
        //                    {
        //                        var creditmasterdata = obj.PACreditDaysMasters.Where(x => x.CreditDaysid == creditdata.CreditdaysId).FirstOrDefault();
        //                    }
        //                    employee = employeedata.Select(x => new EmployeModel()
        //                    {
        //                        EmployeeNo = x.EmployeeNo,
        //                        Name = x.Name
        //                    }).ToList();
        //                }
        //            }
        //            else
        //            {
        //                return employee;
        //            }

        //        }
        //        return employee;
        //    }
        //    catch (Exception ex)  
        //    {
        //        throw;
        //    }
        //}

        //getting approvers based on toatl unitprice and target spend
        public DataSet GetEmployeeMappings1(PAConfigurationModel model)
        {
            SqlConnection Conn1 = new SqlConnection(@"Data Source=10.29.15.183;User ID=sa;Password=yil@1234;initial catalog=YSCM;Integrated Security=false;");
            EmployeModel employee = new EmployeModel();
            DataSet Ds = new DataSet();
            string data = string.Join(",", model.MPRItemDetailsid);
            model.PAValue = model.UnitPrice;
            int Termscode = 0;
            if (model.PAValue > model.TargetSpend)
                model.LessBudget = false;
            else
                model.LessBudget = true;

            if (model.PaymentTermCode != null && model.PaymentTermCode.Length>=2)
                Termscode = Convert.ToInt32(model.PaymentTermCode.Substring(model.PaymentTermCode.Length - 2, 2));
            else if (model.PaymentTermCode != null && model.PaymentTermCode.Length < 2 && model.PaymentTermCode!="")
                Termscode = Convert.ToInt32(model.PaymentTermCode);
            else
                Termscode = 0;

            try
            {
                SqlParameter[] Param = new SqlParameter[5];
                Param[0] = new SqlParameter("@itemid", data);
                Param[1] = new SqlParameter("@PAvalue", model.PAValue);
                Param[2] = new SqlParameter("@TargetSpend", model.TargetSpend);
                Param[3] = new SqlParameter("@creditdays", Termscode);
                Param[4] = new SqlParameter("@departmentid", model.DeptId);
                string spname = "PAApprovers";
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter Adp = new SqlDataAdapter();
                cmd = new SqlCommand();
                cmd.Connection = Conn1;
                cmd.CommandText = spname;
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;

                if (Param != null)
                {
                    foreach (SqlParameter sqlParam in Param)
                    {
                        cmd.Parameters.Add(sqlParam);
                    }
                }

                Adp = new SqlDataAdapter(cmd);
                Ds = new DataSet();

                Adp.Fill(Ds);
                cmd.Parameters.Clear();
                //Ds.Clear();
                return Ds;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<EmployeModel> GetEmployeeMappings(PAConfigurationModel model)
        {
            EmployeModel employee = new EmployeModel();
            model.PAValue = model.UnitPrice;
            int Termscode = 0;
            if (model.PAValue > model.TargetSpend)
            {
                model.LessBudget = false;
                //model.MoreBudget = true;
            }
            else
            {
                //model.MoreBudget = false;
                model.LessBudget = true;
            }
            if (model.PaymentTermCode != null)
            {
                Termscode = Convert.ToInt32(model.PaymentTermCode.Substring(model.PaymentTermCode.Length - 2, 2));
            }
            else
            {
                Termscode = 0;
            }

            try
            {
                var BuyerManagers = obj.LoadBuyerManagers.Where(x => model.MPRItemDetailsid.Contains(x.Itemdetailsid) && x.BoolValidRevision == true).FirstOrDefault();
                if (BuyerManagers != null)
                {
                    employee.BuyerGroupManager = BuyerManagers.EmployeeName;
                    employee.BuyerGroupNo = BuyerManagers.EmpNo;
                    employee.BGRole = BuyerManagers.Role;
                }
                var projectmanagers = obj.LoadProjectManagers.Where(x => model.MPRItemDetailsid.Contains(x.Itemdetailsid) && x.BoolValidRevision == true).FirstOrDefault();
                if (projectmanagers != null)
                {
                    employee.ProjectManager = projectmanagers.EmployeeName;
                    employee.ProjectMangerNo = projectmanagers.EmpNo;
                    employee.PMRole = projectmanagers.Role;
                }

                var sqlquery = "";
                sqlquery = "select distinct EmployeeNo,name,* from PAandCRMapping where (departmentid=' " + model.DeptId + "' and '" + model.PAValue + "' between minpavalue and maxpavalue and AuthorizationType='PA'and (LessBudget= case when (" + model.TargetSpend + " -" + model.PAValue + ")>=0 then 1 else 0 end or MOREbUDGET= CASE when (" + model.TargetSpend + " - " + model.PAValue + " <0) then 1 else 0 end)) or ('" + model.PAValue + "' between minpavalue and maxpavalue  and '" + Termscode + "' between MinDays and maxdays and AuthorizationType = 'CR')";

                //PAandCRmapping= obj.PAandCRMappings.Where(x => x.DepartmentId == model.DeptId && x.minpavalue.CompareTo(model.PAValue) <= 0 && x.maxpavalue.CompareTo(model.PAValue) >= 0 && x.lessbudget == model.LessBudget && x.MinDays.CompareTo(Termscode) <= 0 && x.MaxDays.CompareTo(Termscode) >= 0).OrderBy(x => x.roleorder).ToList();

                var PAandCRmapping = obj.Database.SqlQuery<PAandCRMapping>(sqlquery).ToList();
                //else
                //{
                //    PAandCRmapping = obj.PAandCRMappings.Where(x => x.DepartmentId == model.DeptId && x.minpavalue.CompareTo(model.PAValue) <= 0 && x.maxpavalue.CompareTo(model.PAValue) >= 0 && x.morebudget == model.MoreBudget && x.MinDays.CompareTo(Termscode) <= 0 && x.MaxDays.CompareTo(Termscode) >= 0).OrderBy(x => x.roleorder).ToList();
                //}
                employee.Approvers = PAandCRmapping.Select(x => new PurchaseCreditApproversModel()
                {
                    ApproverName = x.Name,
                    AuthorizationType = x.AuthorizationType,
                    RoleName = x.role,
                    EmployeeNo = x.EmployeeNo,
                    RoleId = x.FunctionalRoleId,
                    roleorder = x.roleorder
                }).ToList();

                return employee;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //public async Task<EmployeeModel> GetEmployeeMappingsList(List<PAConfigurationModel> model)
        //{
        //    EmployeeModel employee = new EmployeeModel();
        //    model.PAValue = model.UnitPrice;
        //    try
        //    {
        //        if (model != null)
        //        {
        //            //var padata = obj.PAAuthorizationLimits.Where(x => x.MinPAValue >= model.PAValue && x.MaxPAValue <= model.PAValue).FirstOrDefault();
        //            var padata = obj.PAAuthorizationLimits.Where(x => x.MinPAValue.CompareTo(model.PAValue) <= 0 && x.MaxPAValue.CompareTo(model.PAValue) >= 0 && x.DeptId == model.DeptId).FirstOrDefault();
        //            if (padata != null)
        //            {
        //                var mappingdata = obj.PAAuthorizationEmployeeMappings.Where(x => x.Authid == padata.Authid && x.LessBudget == true).FirstOrDefault();
        //                if (mappingdata != null)
        //                {
        //                    var res = obj.PACreditDaysApprovers.ToList();
        //                    var creditapprovaldata = obj.PACreditDaysApprovers.Where(x => x.AuthId == padata.Authid && x.EmployeeNo == mappingdata.Employeeid).FirstOrDefault();
        //                    if (creditapprovaldata != null)
        //                    {
        //                        var creditmasterdata = obj.PACreditDaysMasters.Where(x => x.CreditDaysid == creditapprovaldata.CreditdaysId).FirstOrDefault();
        //                    }
        //                    var employeedata = obj.Employees.Where(x => x.EmployeeNo == creditapprovaldata.EmployeeNo).ToList();
        //                    foreach (var item in employeedata)
        //                    {
        //                        employee.EmployeeNo = item.EmployeeNo;
        //                        employee.Name = item.Name;
        //                    }
        //                }
        //            }
        //        }
        //        return employee;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        //getting approved items and also go by filters to get approved items
        public List<LoadItemsByID> GetItemsByMasterIDs(PADetailsModel masters)
        {
            //List<LoadItemsByID> view = new List<LoadItemsByID>();
            try
            {
                using (YSCMEntities yscm = new YSCMEntities())
                {
                    var sqlquery = "";
                    sqlquery = "select * from LoadItemsByID where itemstatus='Approved' and Mprrfqsplititemid not in(select Mprrfqsplititemid  from PAItem inner join MPRPADetails on PAItem.PAID = MPRPADetails.PAId and(DeleteFlag = 0 or DeleteFlag is null)  and Mprrfqsplititemid is not null) ";
                    if (masters.venderid != 0)
                        sqlquery += " and VendorId='" + masters.venderid + "'";
                    if (masters.RevisionId != 0)
                        sqlquery += " and MPRRevisionId='" + masters.RevisionId + "'";
                    if (masters.RFQNo != null && masters.RFQNo!="")
                        sqlquery += " and RFQNo='" + masters.RFQNo + "'";
                    if (masters.DocumentNumber != null && masters.DocumentNumber!="")
                        sqlquery += " and DocumentNo='" + masters.DocumentNumber + "'";
                    if (masters.BuyerGroupId != 0)
                        sqlquery += " and BuyerGroupId='" + masters.BuyerGroupId + "'";
                    if (masters.SaleOrderNo != null && masters.SaleOrderNo != "")
                        sqlquery += " and SaleOrderNo='" + masters.SaleOrderNo + "'";
                    if (masters.DepartmentId != 0)
                        sqlquery += " and DepartmentId='" + masters.DepartmentId + "'";
                    //if (masters.EmployeeNo != null)
                    //    sqlquery += " and DepartmentId='" + masters.EmployeeNo + "'";
                    if (masters.vendorProjectManager != null && masters.vendorProjectManager != "")
                        sqlquery += " and ProjectManager='" + masters.vendorProjectManager + "'";

                    return yscm.Database.SqlQuery<LoadItemsByID>(sqlquery).ToList();
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<RfqRevisionModel> GetLocalRfqDetailsById(int revisionId)
        {
            RfqRevisionModel revision = new RfqRevisionModel();
            try
            {
                var localrevision = obj.RFQRevisions_N.Where(x => x.rfqRevisionId == revisionId && x.DeleteFlag == false).Include(x => x.RFQMaster).Include(x => x.RFQItems_N).FirstOrDefault();
                if (localrevision != null)
                {
                    revision.RfqMasterId = localrevision.rfqMasterId;
                    revision.RfqRevisionNo = localrevision.RevisionNo;
                    revision.CreatedBy = localrevision.CreatedBy;
                    revision.CreatedDate = localrevision.CreatedDate;
                    revision.PackingForwading = localrevision.PackingForwarding;
                    revision.salesTax = localrevision.SalesTax;
                    revision.Insurance = localrevision.Insurance;
                    revision.CustomsDuty = localrevision.CustomsDuty;
                    revision.PaymentTermDays = localrevision.PaymentTermDays;
                    revision.PaymentTermRemarks = localrevision.PaymentTermRemarks;
                    revision.BankGuarantee = localrevision.BankGuarantee;
                    revision.DeliveryMaxWeeks = localrevision.DeliveryMaxWeeks;
                    revision.DeliveryMinWeeks = localrevision.DeliveryMinWeeks;


                    var rfqmasters = obj.RFQMasters.Where(x => x.RfqMasterId == localrevision.rfqMasterId).ToList();
                    var masters = new RFQMasterModel();
                    var vendors = new VendormasterModel();
                    if (rfqmasters != null)
                    {
                        foreach (var item in rfqmasters)
                        {
                            masters.RfqMasterId = item.RfqMasterId;
                            masters.RFQNo = item.RFQNo;
                            masters.RfqUniqueNo = item.RFQUniqueNo;
                            masters.VendorId = item.VendorId;
                            var vendormasters = obj.VendorMasters.Where(x => x.Vendorid == masters.VendorId).FirstOrDefault();
                            masters.Vendor = new VendormasterModel()
                            {
                                ContactNumber = vendormasters.ContactNo,
                                VendorCode = vendormasters.VendorCode,
                                VendorName = vendormasters.VendorName,
                                Emailid = vendormasters.Emailid,
                                Street = vendormasters.Street,
                            };
                            masters.MPRRevisionId = (int)item.MPRRevisionId;
                            masters.CreatedBy = item.CreatedBy;
                        }
                    }
                    revision.rfqmaster = masters;
                    var vendordata = obj.VendorMasters.Where(x => x.Vendorid == masters.VendorId).FirstOrDefault();
                    if (vendordata != null)
                    {
                        revision.VendorName = vendordata.VendorName;
                    }

                    var rfqitemss = obj.RFQItems.Where(x => x.RFQRevisionId == localrevision.rfqRevisionId).ToList();
                    if (rfqitemss != null)
                    {
                        foreach (var item in rfqitemss)
                        {
                            RfqItemModel rfqitems = new RfqItemModel();
                            rfqitems.HSNCode = item.HSNCode;
                            rfqitems.MRPItemsDetailsID = item.MPRItemDetailsid;
                            rfqitems.QuotationQty = item.QuotationQty;
                            rfqitems.RFQRevisionId = item.RFQRevisionId;
                            rfqitems.RFQItemID = item.RFQItemsId;
                            revision.rfqitem.Add(rfqitems);
                        }
                    }
                }
                var rfqterms = obj.RFQTerms.Where(x => x.RFQrevisionId == revisionId).ToList();

                RFQTermsModel terms = new RFQTermsModel();
                foreach (var item in rfqterms)
                {
                    terms.termsid = item.termsid;
                    terms.RfqTermsid = item.RfqTermsid;
                    terms.Remarks = item.Remarks;
                    terms.VendorResponse = item.VendorResponse;
                    revision.RFQTerms.Add(terms);
                }
                return revision;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //getting all departments
        public async Task<List<DepartmentModel>> GetAllDepartments()
        {
            List<DepartmentModel> model = new List<DepartmentModel>();
            try
            {
                var departments = obj.Departments.ToList();
                foreach (var item in departments)
                {
                    model.Add(new DepartmentModel()
                    {
                        DepartmentID = item.DepartmentId,
                        DepartmentName = item.DepartmentName
                    });
                }
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //getting mapped slabs to department by department id
        public async Task<List<PAAuthorizationLimitModel>> GetSlabsByDepartmentID(int DeptID)
        {
            List<PAAuthorizationLimitModel> model = new List<PAAuthorizationLimitModel>();
            try
            {
                var data = obj.PAAuthorizationLimits.Where(x => x.DeptId == DeptID && x.DeleteFlag == false).ToList();
                if (data != null)
                {
                    model = data.Select(x => new PAAuthorizationLimitModel()
                    {
                        MaxPAValue = x.MaxPAValue,
                        MinPAValue = x.MinPAValue,
                        Authid = x.Authid
                    }).ToList();
                }
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //getting all the employees
        public async Task<List<EmployeModel>> GetAllEmployee()
        {
            List<EmployeModel> model = new List<EmployeModel>();
            try
            {
                var data = obj.Employees.ToList();
                if (data != null)
                {
                    model = data.Select(x => new EmployeModel()
                    {
                        EmployeeNo = x.EmployeeNo,
                        Name = x.Name
                    }).ToList();
                }
                return model;
            }
            catch (Exception)
            {

                throw;
            }
        }

        //getting all the credit days mapped
        public async Task<List<PAAuthorizationLimitModel>> GetAllCredits()
        {
            List<PAAuthorizationLimitModel> model = new List<PAAuthorizationLimitModel>();
            try
            {
                //padata.AuthorizationType.ToLower().Equals("pa")
                var data = obj.PAAuthorizationLimits.Where(x => x.AuthorizationType.ToLower() == "cr").ToList();
                //var mappingdata = obj.PAAuthorizationEmployeeMappings.ToList();
                foreach (var item in data)
                {
                    model.Add(new PAAuthorizationLimitModel()
                    {
                        Authid = item.Authid,
                        MinPAValue = item.MinPAValue,
                        MaxPAValue = item.MaxPAValue,
                        //PAAuthorizationEmployeeMappings=mappingdata.Where(x=>x.Authid== item.Authid).Select(x=>new PAAuthorizationEmployeeMappingModel()
                        //{
                        //    Employeeid=x.Employeeid,
                        //    AuthLevel=x.AuthLevel
                        //}).ToList()
                    });
                }
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<PACreditDaysMasterModel>> GetAllCreditDays()
        {
            List<PACreditDaysMasterModel> model = new List<PACreditDaysMasterModel>();
            try
            {
                var credit = obj.PACreditDaysMasters.ToList();
                if (credit != null)
                {
                    model = credit.Select(x => new PACreditDaysMasterModel()
                    {
                        CreditDaysid = x.CreditDaysid,
                        MinDays = x.MinDays,
                        MaxDays = x.MaxDays
                    }).ToList();
                }
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //getting the mpr purcahse modes
        public async Task<List<MPRPAPurchaseModesModel>> GetAllMprPAPurchaseModes()
        {
            List<MPRPAPurchaseModesModel> model = new List<MPRPAPurchaseModesModel>();
            try
            {
                var data = obj.MPRPAPurchaseModes.Where(x => x.BoolInUse == true).ToList();
                model = data.Select(x => new MPRPAPurchaseModesModel()
                {
                    PurchaseModeId = x.PurchaseModeId,
                    PurchaseMode = x.PurchaseMode,
                    XOrder = x.XOrder
                }).ToList();

                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //getting mpr purchase types
        public async Task<List<MPRPAPurchaseTypesModel>> GetAllMprPAPurchaseTypes()
        {
            List<MPRPAPurchaseTypesModel> model = new List<MPRPAPurchaseTypesModel>();
            try
            {
                var data = obj.MPRPAPurchaseTypes.Where(x => x.BoolInUse == true).ToList();
                model = data.Select(x => new MPRPAPurchaseTypesModel()
                {
                    PurchaseTypeId = x.PurchaseTypeId,
                    PurchaseType = x.PurchaseType,
                    XOrder = x.XOrder
                }).ToList();
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //generating the purchase authorization for approve ditems
        public async Task<statuscheckmodel> InsertPurchaseAuthorization(MPRPADetailsModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                var dateAndTime = DateTime.Now;
                var authorization = new MPRPADetail();
                if (model != null)
                {
                    authorization.PurchaseTypeId = model.PurchaseTypeId;
                    authorization.PurchaseModeId = model.PurchaseModeId;
                    authorization.RequestedOn = dateAndTime.Date;
                    authorization.RequestedBy = model.RequestedBy;
                    authorization.DepartmentID = model.DepartmentID;
                    authorization.BuyerGroupId = model.BuyerGroupId;
                    authorization.ProjectCode = model.ProjectCode;
                    authorization.ProjectName = model.ProjectName;
                    authorization.PackagingForwarding = model.PackagingForwarding;
                    authorization.Taxes = model.Taxes;
                    authorization.Freight = model.Freight;
                    authorization.Insurance = model.Insurance;
                    authorization.DeliveryCondition = model.DeliveryCondition;
                    authorization.ShipmentMode = model.ShipmentMode;
                    authorization.PaymentTerms = model.PaymentTerms;
                    authorization.CreditDays = model.CreditDays;
                    authorization.Warranty = model.Warranty;
                    authorization.BankGuarantee = model.BankGuarantee;
                    authorization.LDPenaltyTerms = model.LDPenaltyTerms;
                    authorization.SpecialInstructions = model.SpecialInstructions;
                    authorization.FactorsForImports = model.FactorsForImports;
                    authorization.SpecialRemarks = model.SpecialRemarks;
                    authorization.SuppliersReference = model.SuppliersReference;
                    authorization.VendorId = model.VendorId;
                    authorization.PAStatus = "Inprogress";
                    authorization.DeleteFlag = false;
                    obj.MPRPADetails.Add(authorization);
                    obj.SaveChanges();
                    status.Sid = authorization.PAId;

                    List<int> revisionid = model.Item.Select(x => x.MPRRevisionId).Distinct().ToList();
                    foreach (var item in revisionid)
                    {
                        var data = new MPRStatusTrack();
                        data.StatusId = 11;
                        int requisitionid = obj.MPRRevisions.Where(x => x.RevisionId == item).FirstOrDefault().RequisitionId;
                        data.RequisitionId = requisitionid;
                        data.RevisionId = item;
                        data.UpdatedDate = dateAndTime.Date;
                        data.UpdatedBy = model.LoginEmployee;
                        obj.MPRStatusTracks.Add(data);
                        obj.SaveChanges();

                        var data1 = new MPRRevision();
                        data1 = obj.MPRRevisions.Where(x => x.RevisionId == item).FirstOrDefault();
                        data1.StatusId = Convert.ToByte(data.StatusId);
                        obj.SaveChanges();
                    }
                    //model.Item.Select(x => x.MPRItemDetailsid).FirstOrDefault()
                    foreach (var item in model.Item.GroupBy(n => n.RFQItemsId).Select(x => x.FirstOrDefault()))
                    {
                        var itemdata = obj.RFQItemsInfo_N.Where(x => x.RFQItemsId == item.RFQItemsId).ToList();
                        foreach (var items in itemdata)
                        {
                            PAItem paitem = new PAItem()
                            {
                                PAID = status.Sid,
                                RfqSplitItemId = items.RFQSplitItemId,
                                Mprrfqsplititemid=item.Mprrfqsplititemid,
                                MPRItemDetailsId = item.MPRItemDetailsid
                            };
                            obj.PAItems.Add(paitem);
                            obj.SaveChanges();
                        }

                    }
                }
                else
                {

                }

                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<statuscheckmodel> finalpa(MPRPADetailsModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                var dateAndTime = DateTime.Now;
                var authorization = new MPRPADetail();
                if (model != null)
                {
                    var Approveritem = new MPRPAApprover();
                    foreach (var item in model.ApproversList)
                    {

                        Approveritem.PAId = model.PAId;
                        Approveritem.ApproverLevel = 1;
                        Approveritem.RoleName = item.rolename;
                        Approveritem.Approver = item.Approver;
                        Approveritem.ApproversRemarks = item.ApproversRemarks;
                        Approveritem.ApprovalStatus = "submitted";
                        Approveritem.ApprovedOn = dateAndTime.Date;

                        obj.MPRPAApprovers.Add(Approveritem);
                        obj.SaveChanges();
                    }

                     this.emailDA.PAEmailRequest(model.PAId, model.LoginEmployee);

                    var data = obj.MPRPADetails.Where(x => x.PAId == model.PAId).FirstOrDefault();
                    data.PAStatus = "Submitted";
                    obj.SaveChanges();

                }
                else
                {

                }

                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        //getting the generated purchase authorization by pa id
        public async Task<MPRPADetailsModel> GetMPRPADeatilsByPAID(int PID)
        {
            MPRPADetailsModel model = new MPRPADetailsModel();
            try
            {
                var data = obj.MPRPADetails.Where(x => x.PAId == PID).FirstOrDefault();

                if (data != null)
                {
                    model.PurchaseTypeId = data.PurchaseTypeId;
                    var purchasetype = obj.MPRPAPurchaseTypes.Where(x => x.PurchaseTypeId == model.PurchaseTypeId).FirstOrDefault();
                    model.purchasetypes.PurchaseType = purchasetype.PurchaseType;
                    model.PurchaseModeId = data.PurchaseModeId;
                    var purchasemode = obj.MPRPAPurchaseModes.Where(x => x.PurchaseModeId == model.PurchaseModeId).FirstOrDefault();
                    model.purchasemodes.PurchaseMode = purchasemode.PurchaseMode;
                    model.RequestedOn = data.RequestedOn;
                    model.RequestedBy = data.RequestedBy;
                    model.DepartmentID = data.DepartmentID;
                    var department = obj.MPRDepartments.Where(x => x.DepartmentId == model.DepartmentID).FirstOrDefault();
                    model.department.Department = department.Department;
                    model.BuyerGroupId = data.BuyerGroupId;
                    var buyergroup = obj.MPRBuyerGroups.Where(x => x.BuyerGroupId == model.BuyerGroupId).FirstOrDefault();
                    model.buyergroup.BuyerGroup = buyergroup.BuyerGroup;
                    model.ProjectCode = data.ProjectCode;
                    model.ProjectName = data.ProjectName;
                    model.PackagingForwarding = data.PackagingForwarding;
                    model.Taxes = data.Taxes;
                    model.Freight = data.Freight;
                    model.Insurance = data.Insurance;
                    model.PAStatus = data.PAStatus;
                    model.DeliveryCondition = data.DeliveryCondition;
                    model.ShipmentMode = data.ShipmentMode;
                    model.PaymentTerms = data.PaymentTerms;
                    model.CreditDays = data.CreditDays;
                    model.Warranty = data.Warranty;
                    model.BankGuarantee = data.BankGuarantee;
                    model.LDPenaltyTerms = data.LDPenaltyTerms;
                    model.SpecialInstructions = data.SpecialInstructions;
                    model.FactorsForImports = data.FactorsForImports;
                    model.SpecialRemarks = data.SpecialRemarks;
                    model.SuppliersReference = data.SuppliersReference;
                    model.Deleteflag = data.DeleteFlag;

                    var statusdata = obj.LoadItemsByPAIDs.Where(x => x.itemstatus == "Approved" && x.PAId == PID).ToList();
                    model.Item = statusdata.Select(x => new RfqItemModel()
                    {
                        ItemDescription = x.ItemDescription,
                        UnitPrice = Convert.ToDecimal(x.UnitPrice),
                        QuotationQty =Convert.ToDouble(x.QuotationQty),
                        DocumentNo = x.DocumentNo,
                        SaleOrderNo = x.SaleOrderNo,
                        Department = x.Department,
                        TargetSpend = Convert.ToDecimal(x.TargetSpend),
                        PaymentTermCode = x.PaymentTermCode,
                        VendorName = x.VendorName,
                        DepartmentId = x.DepartmentId,
                        MRPItemsDetailsID = Convert.ToInt16(x.MPRItemDetailsid),
                        RFQRevisionId = x.rfqRevisionId,
                        paid = x.PAId,
                        paitemid = x.PAItemID,
                        POItemNo = x.POItemNo,
                        PONO = x.PONO,
                        Remarks = x.Remarks,
                        PODate = x.PODate.ToString(),
                        RFQNo=x.RFQNo,
                        HSNCode=x.HSNCode,
                        MPRRevisionId = Convert.ToInt32(x.MPRRevisionId)
                    }).ToList();
                    var approverdata = obj.GetmprApproverdeatils.Where(x => x.PAId == PID).ToList();
                    model.ApproversList = approverdata.Select(x => new MPRPAApproversModel()
                    {
                        ApproverName = x.Name,
                        RoleName = x.RoleName,
                        ApproversRemarks = x.ApproversRemarks,
                        ApprovalStatus = x.ApprovalStatus,
                        EmployeeNo = x.Approver,
                        ApprovedOn = x.ApprovedOn,
                        parequested = x.parequested,
                        PARequestedOn = x.RequestedOn
                    }).ToList();
                    var documentsdata = obj.MPRPADocuments.Where(x => x.paid == PID).ToList();
                    if (documentsdata.Count!=0)
                    {
                        
                            model.documents = documentsdata.Select(x => new PADocumentsmodel()
                            {
                                filename=x.Filename,
                                path=x.Filepath,
                                uploadeddate=x.uploadeddate,
                            }).ToList();
                     
                    }

                    var requested = obj.parequestedanddeletedemployees.Where(x => x.paid == PID).ToList();
                    model.request = requested.Select(x => new parequestedanddeletemodel()
                    {
                        parequested=x.parequested,
                        RequestedOn=x.RequestedOn,
                        PAStatus=x.PAStatus,
                        PAStatusUpdate=x.PAStatusUpdate,
                        DeleteFlag=x.DeleteFlag,
                        DeleteBy=x.DeleteBy,
                        DeleteOn=x.DeleteOn,
                        padeleted=x.padeleted,
                        Remarks=x.Remarks
                    }).ToList();

                    return model;
                }
                else
                {
                    return model;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //retreving the generated pa total list
        public async Task<List<MPRPADetailsModel>> GetAllMPRPAList()
        {
            List<MPRPADetailsModel> model = new List<MPRPADetailsModel>();
            try
            {
                var data = obj.MPRPADetails.ToList();
                if (data != null)
                {
                    model = data.Select(c => new MPRPADetailsModel()
                    {
                        PAId = c.PAId,
                        RequestedBy = c.RequestedBy,
                        RequestedOn = c.RequestedOn
                    }).ToList();
                    return model;
                }
                else
                {
                    return model;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //getting the functinal role of approvers 
        public async Task<List<PAFunctionalRolesModel>> GetAllPAFunctionalRoles()
        {
            List<PAFunctionalRolesModel> model = new List<PAFunctionalRolesModel>();
            try
            {
                var roles = obj.PAFunctionalRoles.OrderBy(x => x.XOrder).ToList();
                if (roles != null)
                {
                    model = roles.Select(x => new PAFunctionalRolesModel()
                    {
                        FunctionalRoleId = x.FunctionalRoleId,
                        FunctionalRole = x.FunctionalRole,
                        XOrder = x.XOrder
                    }).ToList();
                }
                else
                {
                    return model;
                }
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<EmployeemappingtocreditModel>> GetCreditSlabsandemployees()
        {
            List<EmployeemappingtocreditModel> model = new List<EmployeemappingtocreditModel>();
            try
            {
                var data = obj.Employeemappingtocredits.OrderByDescending(x => x.CreditdaysId).ToList();
                if (data != null)
                {
                    model = data.Select(x => new EmployeemappingtocreditModel()
                    {
                        Authid = x.Authid,
                        AuthorizationType = x.AuthorizationType,
                        CreditdaysId = x.CreditdaysId,
                        DeptId = x.DeptId,
                        EmployeeNo = x.EmployeeNo,
                        Name = x.Name,
                        MinDays = x.MinDays,
                        MaxDays = x.MaxDays,
                        MinPAValue = x.MinPAValue,
                        MaxPAValue = x.MaxPAValue,
                        CRApprovalId = x.CRApprovalId
                    }).ToList();
                }
                else
                {
                    return model;
                }
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<EmployeemappingtopurchaseModel>> GetPurchaseSlabsandMappedemployees()
        {
            List<EmployeemappingtopurchaseModel> model = new List<EmployeemappingtopurchaseModel>();
            try
            {
                var data = obj.Employeemappingtopurchases.OrderBy(x => x.Authid).ToList();
                if (data != null)
                {
                    model = data.Select(x => new EmployeemappingtopurchaseModel()
                    {
                        Authid = x.Authid,
                        AuthorizationType = x.AuthorizationType,
                        MaxPAValue = x.MaxPAValue,
                        MinPAValue = x.MinPAValue,
                        Employeeid = x.Employeeid,
                        LessBudget = x.LessBudget,
                        MoreBudget = x.MoreBudget,
                        DepartmentName = x.Department,
                        Name = x.Name,
                        FunctionalRoleId = x.FunctionalRoleId,
                        PAmapid = x.PAmapid
                    }).ToList();
                    return model;
                }
                else
                {
                    return model;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //getting allthe project managers based on employeeno
        public async Task<List<ProjectManagerModel>> LoadAllProjectManagers()
        {
            List<ProjectManagerModel> model = new List<ProjectManagerModel>();
            try
            {
                var data = obj.MPRRevisions.Where(x => x.BoolValidRevision == true).Select(x => x.ProjectManager).Distinct().ToList();
                if (data != null)
                {
                    var vmodel = obj.Employees.Where(x => data.Contains(x.EmployeeNo)).ToList();
                    model = vmodel.Select(x => new ProjectManagerModel()
                    {
                        EmployeeNo = x.EmployeeNo,
                        Name = x.Name
                    }).ToList();
                }
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<VendormasterModel>> LoadVendorByMprDetailsId(List<int?> MPRItemDetailsid)
        {
            List<VendormasterModel> model = new List<VendormasterModel>();
            try
            {
                //var data = obj.LoadItemsByIDs.Where(x => MPRItemDetailsid.Contains(x.MPRItemDetailsid)).ToList();
                var vendor = (from xx in obj.LoadItemsByIDs select xx).Where(y => MPRItemDetailsid.Contains(y.MPRItemDetailsid)).GroupBy(n => new { n.VendorId, n.VendorName }).Select(x => x.FirstOrDefault()).ToList();
                //var data = obj.LoadItemsByIDs.Distinct().Where(x => MPRItemDetailsid.Contains(x.MPRItemDetailsid)).Distinct().ToList();
                model = vendor.Select(x => new VendormasterModel()
                {
                    VendorName = x.VendorName,
                    Vendorid = x.VendorId
                }).ToList();
                return model;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<List<MPRPAApproversModel>> GetAllApproversList()
        {
            List<MPRPAApproversModel> model = new List<MPRPAApproversModel>();
            try
            {
                var data = obj.MPRPAApprovers.ToList();
                if (data != null)
                {
                    model = data.Select(x => new MPRPAApproversModel()
                    {
                        PAId = x.PAId,
                        ApproverLevel = x.ApproverLevel,
                        RoleName = x.RoleName,
                        ApproverName = x.Approver,
                        ApproversRemarks = x.ApproversRemarks,
                        ApprovalStatus = x.ApprovalStatus,
                        ApprovedOn = x.ApprovedOn
                    }).ToList();
                    return model;
                }
                else
                {
                    return model;
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //getting pa approved,pending and submitted
        public async Task<List<GetmprApproverdeatil>> GetMprApproverDetailsBySearch(PAApproverDetailsInputModel model)
        {
            List<GetmprApproverdeatil> details = new List<GetmprApproverdeatil>();
            int mprno = 0;
            int rfqno = 0;
            if (model.DocumentNumber != null && model.DocumentNumber != "")
            {
                if (model.DocumentNumber.StartsWith("MPR", StringComparison.CurrentCultureIgnoreCase))
                {
                    model.DocumentNumber = model.DocumentNumber;
                }
                else
                {
                    mprno = Convert.ToInt32(model.DocumentNumber);
                    model.DocumentNumber = null;
                }
            }
            if (model.rfqnumber != null && model.rfqnumber != null)
            {
                if (model.rfqnumber.StartsWith("RFQ", StringComparison.CurrentCultureIgnoreCase))
                {
                    model.rfqnumber = model.rfqnumber;
                }
                else
                {
                    rfqno = Convert.ToInt32(model.rfqnumber);
                    model.rfqnumber = null;
                }
            }
            CultureInfo provider = CultureInfo.InvariantCulture;
            try
            {
                var sqlquery = "";
                sqlquery = "select * from GetmprApproverdeatils where Approver='" + model.CreatedBy + "'";
                if (model.Paid != 0)
                    sqlquery += " and  PAId='" + model.Paid + "'";
                if (model.Status != null)
                    sqlquery += " and ApprovalStatus='" + model.Status + "'";
                if (model.FromDate != null && model.ToDate != null)
                    sqlquery += " and RequestedOn between '" + model.FromDate + "' and '" + model.ToDate + "'";
                if (model.Status == null)
                    sqlquery += " and ApprovalStatus in('submitted', 'pending') ";
                if (model.rfqnumber != null && model.rfqnumber != "")
                    sqlquery += " and  RFQNo='" + model.rfqnumber + "'";
                if (model.DocumentNumber != null && model.DocumentNumber != "")
                    sqlquery += " and  DocumentNo='" + model.DocumentNumber + "'";
                if (model.VendorId != 0)
                    sqlquery += " and  Vendorid='" + model.VendorId + "'";
                if (model.DepartmentId != 0)
                    sqlquery += " and  DepartmentID='" + model.DepartmentId + "'";
                if (mprno != 0)
                    sqlquery += " and MPRSeqNo = '" + mprno + "'";
                if (rfqno != 0)
                    sqlquery += " and RFQUniqueNo ='" + rfqno + "'";

                sqlquery += "  order by PAId desc ";
                
                details = obj.Database.SqlQuery<GetmprApproverdeatil>(sqlquery).ToList();
                return details;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //updating the pa status based on paid
        public async Task<statuscheckmodel> UpdateMprpaApproverStatus(MPRPAApproversModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                var approverdata = obj.MPRPAApprovers.Where(x => x.PAId == model.PAId && x.Approver == model.EmployeeNo).ToList();
                if (approverdata != null)
                {
                    foreach (var data in approverdata)
                    {
                        data.ApproversRemarks = model.ApproversRemarks;
                        data.ApprovalStatus = model.ApprovalStatus;
                        data.ApprovedOn = System.DateTime.Now;
                        obj.SaveChanges();
                    }
                    //approverdata.ApproversRemarks = model.ApproversRemarks;
                    //approverdata.ApprovalStatus = model.ApprovalStatus;
                    //approverdata.ApprovedOn = System.DateTime.Now;
                    //obj.SaveChanges();
                    //status.Sid = approverdata.PAId;
                    UpdatePAStatus(model.PAId, model.mprrevisionid, model.EmployeeNo, model.ApprovalStatus);
                    return status;
                }
                else
                {
                    return status;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //continuation to above api
        public bool UpdatePAStatus(int paid, int mprrevisionid, string employeeno, string ApprovalStatus)
        {
            List<approverFinalview> approver = new List<approverFinalview>();
            int statusid = 0;
            var sqlquery = "";
            var padetails = obj.MPRPADetails.Where(x => x.PAId == paid).FirstOrDefault();
            var pastatus = obj.MPRRevisions.Where(x => x.RevisionId == mprrevisionid).FirstOrDefault();
            sqlquery = "select * from approverFinalview where PAId='" + paid + "' ";
            approver = obj.Database.SqlQuery<approverFinalview>(sqlquery).ToList();
            if (ApprovalStatus == "Approved")
                statusid = 18;
            else if (ApprovalStatus == "Rejected")
                statusid = 21;

            if (approver == null || approver.Count == 0)
            {
                MPRStatusTrack statustrack = new MPRStatusTrack();
                statustrack.StatusId = statusid;
                statustrack.RevisionId = mprrevisionid;
                int requisitionid = obj.MPRRevisions.Where(x => x.RevisionId == mprrevisionid).FirstOrDefault().RequisitionId;
                statustrack.RequisitionId = requisitionid;
                statustrack.UpdatedBy = employeeno;
                statustrack.UpdatedDate = System.DateTime.Now;
                //statustrack.Status = "PA Approved";
                obj.MPRStatusTracks.Add(statustrack);
                obj.SaveChanges();
                if (padetails != null)
                    padetails.PAStatus = ApprovalStatus;
                    padetails.POStatus = "Pending";
                    padetails.POStatusUpdate = System.DateTime.Now;
                    padetails.PAStatusUpdate = System.DateTime.Now;
                obj.SaveChanges();
                pastatus.StatusId = 18;
                obj.SaveChanges();

                this.emailDA.paemailstatus(statusid, paid, mprrevisionid, ApprovalStatus, employeeno);
                int id = statustrack.StatusId;
                updatepaitems(mprrevisionid,paid);
            }
            else if (ApprovalStatus == "Rejected")
            {
                MPRStatusTrack statustrack = new MPRStatusTrack();
                statustrack.StatusId = statusid;
                statustrack.RevisionId = mprrevisionid;
                int requisitionid = obj.MPRRevisions.Where(x => x.RevisionId == mprrevisionid).FirstOrDefault().RequisitionId;
                statustrack.RequisitionId = requisitionid;
                statustrack.UpdatedBy = employeeno;
                statustrack.UpdatedDate = System.DateTime.Now;
                //statustrack.Status = "PA Approved";
                obj.MPRStatusTracks.Add(statustrack);
                obj.SaveChanges();

                if (padetails != null)
                    padetails.PAStatus = ApprovalStatus;
                obj.SaveChanges();

                pastatus.StatusId = 21;
                obj.SaveChanges();

                this.emailDA.paemailstatus(statusid, paid, mprrevisionid, ApprovalStatus, employeeno);
                
            }
            else
            {
                MPRStatusTrack statustrack = new MPRStatusTrack();
                statustrack.StatusId = statusid;
                statustrack.RevisionId = mprrevisionid;
                int requisitionid = obj.MPRRevisions.Where(x => x.RevisionId == mprrevisionid).FirstOrDefault().RequisitionId;
                statustrack.RequisitionId = requisitionid;
                statustrack.UpdatedBy = employeeno;
                statustrack.UpdatedDate = System.DateTime.Now;
                //statustrack.Status = "PA Approved";
                obj.MPRStatusTracks.Add(statustrack);
                obj.SaveChanges();
                padetails.PAStatus = "Pending";
                padetails.POStatus = "Pending";
                obj.SaveChanges();
            }
            //approver.approved;
            return true;
        }
        public bool updatepaitems(int mprrevisionid,int paid)
        {
            List<Updatepaitem> items = new List<Updatepaitem>();
            try
            {
                var sqlquery = "";
                sqlquery = "select * from Updatepaitem where MPRRevisionId='" + mprrevisionid + "' and paid='"+ paid +"' ";
                items = obj.Database.SqlQuery<Updatepaitem>(sqlquery).ToList();
                List<MPRRfqItemInfo> paitem = new List<MPRRfqItemInfo>();
                foreach (var item in items)
                {
                    var mprrfq = new MPRRfqItemInfo()
                    {
                        PAItemID = item.paitemid,
                    };
                    obj.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<List<DisplayRfqTermsByRevisionId>> getrfqtermsbyrevisionid(List<int> RevisionId)
        {
            List<DisplayRfqTermsByRevisionId> revision = new List<DisplayRfqTermsByRevisionId>();
            try
            {
                revision = obj.DisplayRfqTermsByRevisionIds.Where(x => RevisionId.Contains(x.RFQrevisionId)).ToList();
                return revision;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //public async Task<List<EmployeemappingtopurchaseModel>> GetPurchaseSlabsandMappedemployeesByDeptId(int deptid)
        //{
        //    List<EmployeemappingtopurchaseModel> model = new List<EmployeemappingtopurchaseModel>();
        //    try
        //    {
        //        var data = obj.Employeemappingtopurchases.Where(x => x.DeptId == deptid).ToList();
        //        if (data != null)
        //        {
        //            model = data.Select(x => new EmployeemappingtopurchaseModel()
        //            {
        //                Authid = x.Authid,
        //                AuthorizationType = x.AuthorizationType,
        //                MaxPAValue = x.MaxPAValue,
        //                MinPAValue = x.MinPAValue,
        //                Employeeid = x.Employeeid,
        //                LessBudget = x.LessBudget,
        //                MoreBudget = x.MoreBudget,
        //                DepartmentName = x.Department,
        //                Name = x.Name,
        //                FunctionalRoleId = x.FunctionalRoleId,
        //                PAmapid = x.PAmapid
        //            }).ToList();
        //            return model;
        //        }
        //        else
        //        {
        //            return model;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

            //getting mapped salbs and corresponding employee based on budget or deptid or employee id
        public async Task<List<Employeemappingtopurchase>> GetPurchaseSlabsandMappedemployeesByDeptId(EmployeeFilterModel model)
        {
            List<Employeemappingtopurchase> purchase = new List<Employeemappingtopurchase>();
            try
            {
                var sqlquery = "";
                sqlquery = "select * from Employeemappingtopurchase where AuthorizationType='PA' and DeleteFlag=0 ";
                if (model.DeptId != 0)
                    sqlquery += " and  DeptId='" + model.DeptId + "'";
                if (model.Employeeid != null && model.Employeeid != "0")
                    sqlquery += " and  Employeeid='" + model.Employeeid + "'";
                if (model.LessBudget != false)
                    sqlquery += " and  LessBudget='" + model.LessBudget + "'";
                if (model.MoreBudget != false)
                    sqlquery += " and  MoreBudget='" + model.MoreBudget + "'";
                if (model.Authid != 0)
                    sqlquery += " and  Authid='" + model.Authid + "'";
                purchase = obj.Database.SqlQuery<Employeemappingtopurchase>(sqlquery).ToList();

                return purchase;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //public async Task<statuscheckmodel> InsertPaitems(ItemsViewModel paitem)
        //{
        //    statuscheckmodel status = new statuscheckmodel();
        //    try
        //    {
        //        var data = obj.PAItems.Where(x => x.PAItemID == paitem.paitemid).FirstOrDefault();
        //        data.PONO = paitem.PONO;
        //        data.POItemNo = paitem.POItemNo;
        //        data.PODate = System.DateTime.Now;
        //        data.Remarks = paitem.Remarks;
        //        data.MPRItemDetailsId = paitem.MRPItemsDetailsID;
        //        obj.SaveChanges();

        //        return status;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        //inserting pa items after pa approval
        public async Task<statuscheckmodel> InsertPaitems(List<ItemsViewModel> paitem)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                foreach (var itemdata in paitem)
                {
                    var paitems = obj.PAItems.Where(x => x.PAItemID == itemdata.paitemid).FirstOrDefault();
                    if (paitems != null)
                    {
                        paitems.PONO = itemdata.PONO;
                        paitems.POItemNo = itemdata.POItemNo;
                        paitems.PODate = itemdata.PODate;
                        paitems.Remarks = itemdata.Remarks;
                        paitems.UpdatedDate = System.DateTime.Now;
                        paitems.UpdatedBy = itemdata.EmployeeNo;
                        obj.SaveChanges();
                    }
                    status.Sid = paitems.PAID;
                }
               
                var sqlquery = "";
                sqlquery = "select * from  PAItem where  PONO  is null and paid= " + status.Sid + " ";
                List<PAItem> item = new List<PAItem>();
                item = obj.Database.SqlQuery<PAItem>(sqlquery).ToList();
                if (item.Count==0)
                {
                    var data = obj.MPRPADetails.Where(x => x.PAId == status.Sid).FirstOrDefault();
                    data.POStatus = "PORelasesd";
                    data.POStatusUpdate = System.DateTime.Now;
                    obj.SaveChanges();
                }
                else
                {

                }
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<GetMappedSlab>> GetAllMappedSlabs()
        {
            List<GetMappedSlab> slab = new List<GetMappedSlab>();
            try
            {
                slab = obj.GetMappedSlabs.Where(x => x.DeleteFlag == false).OrderBy(x => x.Authid).ToList();
                return slab;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        //removing mapped slabs
        public async Task<statuscheckmodel> RemoveMappedSlab(PAAuthorizationLimitModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                var data = obj.PAAuthorizationLimits.Where(x => x.Authid == model.Authid).FirstOrDefault();
                if (data != null)
                {
                    data.DeleteFlag = true;
                    obj.SaveChanges();
                }
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //getting the all palist and also go by filetrs like mprno or vendor or buyergroup or rfqno or paid or postatus or pastatus or fromdate and todate
        public async Task<List<GetMprPaDetailsByFilter>> getMprPaDetailsBySearch(PADetailsModel model)
        {
            int mprno = 0;
            int rfqno = 0;
            if (model.DocumentNumber!=null && model.DocumentNumber!="")
            {
                if (model.DocumentNumber.StartsWith("MPR", StringComparison.CurrentCultureIgnoreCase))
                {
                    model.DocumentNumber = model.DocumentNumber;
                }
                else
                {
                    mprno = Convert.ToInt32(model.DocumentNumber);
                    model.DocumentNumber = null;
                }
            }
            if (model.rfqnumber!=null && model.rfqnumber!=null)
            {
                if (model.rfqnumber.StartsWith("RFQ", StringComparison.CurrentCultureIgnoreCase))
                {
                    model.rfqnumber = model.rfqnumber;
                }
                else
                {
                    rfqno = Convert.ToInt32(model.rfqnumber);
                    model.rfqnumber = null;
                }
            }
            List<GetMprPaDetailsByFilter> filter = new List<GetMprPaDetailsByFilter>();
            try
            {
                var sqlquery = "";
                sqlquery = "select * from GetMprPaDetailsByFilters  where PAStatus in ('Pending','rejected','Approved') and DeleteFlag=0 ";
                if (model.DocumentNumber != null && model.DocumentNumber!="")
                    sqlquery += " and  DocumentNo='" + model.DocumentNumber + "'";
                if (model.DepartmentId != 0)
                    sqlquery += " and DepartmentId='" + model.DepartmentId + "'";
                if (model.PONO != null)
                    sqlquery += " and PONO='" + model.PONO + "'";
                if (model.Paid != 0)
                    sqlquery += " and PAId='" + model.Paid + "'";
                if (model.POdate != null)
                    sqlquery += " and PODate='" + model.POdate + "'";
                if (model.fromDate != null && model.toDate != null)
                    sqlquery += " and RequestedOn between '" + model.fromDate + "' and '" + model.toDate + "'";
                if (model.BuyerGroupId != 0)
                    sqlquery += " and BuyerGroupId='" + model.BuyerGroupId + "'";
                if (model.VendorId != 0)
                    sqlquery += " and VendorId='" + model.VendorId + "'";
                if (model.PAStatus != null)
                    sqlquery += " and PAStatus='" + model.PAStatus + "'";
                if (model.POStatus != null)
                    sqlquery += " and POStatus='" + model.POStatus + "'";
                if (mprno != 0)
                    sqlquery += " and MPRSeqNo = '" + mprno + "'";
                if (model.rfqnumber != null && model.rfqnumber != "")
                    sqlquery += " and RFQNo ='" + model.rfqnumber + "'";
                if (rfqno != 0)
                    sqlquery += " and RFQUniqueNo ='" + rfqno + "'";


                sqlquery += " order by PAId desc ";
                //if (model.FromDate != null && model.ToDate != null)
                //    sqlquery += " and RequestedOn between '" + model.FromDate + "' and '" + model.ToDate + "'";

                filter = obj.Database.SqlQuery<GetMprPaDetailsByFilter>(sqlquery).ToList();
                return filter;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //getting ps status reports 
        public async Task<List<MPRDate>> GetPaStatusReports(PAReportInputModel model)
        {
            List<MPRDate> report = new List<MPRDate>();
            //DateTime? fromdate= model.FromDate;
            string fromdate = model.FromDate?.ToString("yyyy-MM-dd");
            string todate = model.ToDate?.ToString("yyyy-MM-dd");
            //DateTime?  todate = model.ToDate;
            try
            {
                var sqlquery = "";
                //sqlquery = "select * from pareports where PreparedOn between '" + fromdate + "' and '" + todate + "'";
                sqlquery = "select * from MPRDates where mprrevisionid!=0 ";
                if (fromdate != null && todate != null)
                    sqlquery += " and preparedon between '" + fromdate + "' and '" + todate + "' ";
                if (model.MPRRevisionId != 0)
                    sqlquery += " and mprrevisionid='" + model.MPRRevisionId + "'";
                //sqlquery = " select * from (select ms.Status, ms.StatusId, mr.RevisionId, mst.UpdatedDate, mr.ApprovalStatus, mr.ApprovedOn, mr.SecondApproversStatus, mr.ThirdApproverStatus, mr.SecondApprovedOn from MPRStatusTrack mst inner join MPRRevisions mr on mr.RevisionId = mst.RevisionId inner join MPRStatus ms on ms.StatusId = mst.StatusId) t pivot(count(statusid) for status in (submitted, Checked, approved, rejected, Acknowledged,[Clarification to End User],[RFQ Generated],[RFQ Responded],[Technical Spec Approved],[Quote Finalized],[PA Generated],[PO Released],[Raising PO Checked],[Raising PO Approved],[MPR Rejected],[MPR On Hold],[RFQ Finalized],[PA Approved],[MPR Closed])) as pivot_table ";
                report = obj.Database.SqlQuery<MPRDate>(sqlquery).ToList();
                return report;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //sending email to approver that to approve pa
        public async Task<statuscheckmodel> UpdateApproverforRequest(MPRPAApproversModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                string email = obj.Employees.Where(x => x.EmployeeNo == model.EmployeeNo).FirstOrDefault().EMail;
                this.emailDA.PAEmailRequestForApproval(model.PAId, email,model.EmployeeNo);
                return status;
            }
            catch (Exception ex)
            {
                throw; 
            }
        }
        public async Task<statuscheckmodel> DeletePAByPAid(padeletemodel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                var data = obj.MPRPADetails.Where(x => x.PAId == model.PAId).FirstOrDefault();
                if (data!=null)
                {
                    data.Remarks = model.Remarks;
                    data.DeleteFlag = true;
                    data.DeleteBy = model.employeeno;
                    data.DeleteOn = DateTime.Now;
                    obj.SaveChanges();
                }
                status.Sid = data.PAId;
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<IncompletedPAlist>> GetIncompletedPAlist(painputmodel model)
        {
            List<IncompletedPAlist> filter = new List<IncompletedPAlist>();
            try
            {
                var sqlquery = "";
                sqlquery = "select * from IncompletedPAlist where PAStatus in ('Inprogress','submitted')";
                if (model.PAId != 0)
                    sqlquery += " and PAId='" + model.PAId + "'";
                filter = obj.Database.SqlQuery<IncompletedPAlist>(sqlquery).ToList();
                return filter;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<statuscheckmodel> UpdatePurchaseAuthorization(MPRPADetailsModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            var dateAndTime = DateTime.Now;
            try
            {
                var authorization = obj.MPRPADetails.Where(x => x.PAId == model.PAId).FirstOrDefault();
                authorization.PurchaseTypeId = model.PurchaseTypeId;
                authorization.PurchaseModeId = model.PurchaseModeId;
                authorization.RequestedOn = dateAndTime.Date;
                authorization.RequestedBy = model.RequestedBy;
                authorization.DepartmentID = model.DepartmentID;
                authorization.BuyerGroupId = model.BuyerGroupId;
                authorization.ProjectCode = model.ProjectCode;
                authorization.ProjectName = model.ProjectName;
                authorization.PackagingForwarding = model.PackagingForwarding;
                authorization.Taxes = model.Taxes;
                authorization.Freight = model.Freight;
                authorization.Insurance = model.Insurance;
                authorization.DeliveryCondition = model.DeliveryCondition;
                authorization.ShipmentMode = model.ShipmentMode;
                authorization.PaymentTerms = model.PaymentTerms;
                authorization.CreditDays = model.CreditDays;
                authorization.Warranty = model.Warranty;
                authorization.BankGuarantee = model.BankGuarantee;
                authorization.LDPenaltyTerms = model.LDPenaltyTerms;
                authorization.SpecialInstructions = model.SpecialInstructions;
                authorization.FactorsForImports = model.FactorsForImports;
                authorization.SpecialRemarks = model.SpecialRemarks;
                authorization.SuppliersReference = model.SuppliersReference;
                obj.SaveChanges();
                status.Sid = authorization.PAId;
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public DataTable getrfqtermsbyrevisionsid1(List<int> revisionid)
        {
            SqlConnection Conn1 = new SqlConnection(@"Data Source=10.29.15.183;User ID=sa;Password=yil@1234;initial catalog=YSCM;Integrated Security=false;");
            EmployeModel employee = new EmployeModel();
            DataTable Ds = new DataTable();
            string data = string.Join(" ',' ", revisionid.Distinct());
            try
            {
                SqlParameter[] Param = new SqlParameter[1];
                Param[0] = new SqlParameter("@revisionid", data);
                string spname = "Getrfqtermsbyrevision";
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter Adp = new SqlDataAdapter();
                cmd = new SqlCommand();
                cmd.Connection = Conn1;
                cmd.CommandText = spname;
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;

                if (Param != null)
                {
                    foreach (SqlParameter sqlParam in Param)
                    {
                        cmd.Parameters.Add(sqlParam);
                    }
                }

                Adp = new SqlDataAdapter(cmd);

                Adp.Fill(Ds);
                cmd.Parameters.Clear();
                //Ds.Clear();
                return Ds;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
