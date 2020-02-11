using SCMModels;
using SCMModels.RFQModels;
using SCMModels.SCMModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.DirectoryServices.AccountManagement;
using System.Web;
using System.Web.Services;

namespace DALayer.Login
{
    public class LoginDA : ILoginDA
    {
        YSCMEntities DB = new YSCMEntities();
        //public bool ValidateLoginCredentials(DynamicSearchResult Result)
        //{
        //    bool loginFlag = false;
        //    string[] UserCredentials = Result.columnValues.Split(',');
        //    string Id = UserCredentials[0].ToString();
        //    PrincipalContext ctx = new PrincipalContext(ContextType.Domain);

        //    UserPrincipal user = UserPrincipal.FindByIdentity(ctx, UserCredentials[0].Trim());
        //    if (user != null)
        //    {
        //        if (ctx.ValidateCredentials(UserCredentials[0], UserCredentials[1]))
        //        {
        //            loginFlag = true;
        //        }
        //    }
        //    else if (user == null)
        //    {
        //        string empNo = DB.Employees.Where(li => li.DomainId == Id).Select(li => li.EmployeeNo).SingleOrDefault();
        //        if (empNo != "")
        //        {
        //            loginFlag = true;
        //        }
        //    }
        //    return loginFlag;
        //}


        public EmployeeModel ValidateLoginCredentials(DynamicSearchResult Result)
        {
            //var session = HttpContext.Current.Session;
            DB.Configuration.ProxyCreationEnabled = false;
            EmployeeModel employee = new EmployeeModel();
            string[] UserCredentials = Result.columnValues.Split(',');
            string Id = UserCredentials[0].ToString();
            PrincipalContext ctx = new PrincipalContext(ContextType.Domain);

            UserPrincipal user = UserPrincipal.FindByIdentity(ctx, UserCredentials[0].Trim());
            if (user != null)
            {
                if (ctx.ValidateCredentials(UserCredentials[0], UserCredentials[1]))
                {
                    var data = DB.Employees.Where(li => li.DomainId == Id).FirstOrDefault();
                    if (data != null)
                    {
                        employee.EmployeeNo = data.EmployeeNo;
                        employee.Name = data.Name;
                        employee.EMail = data.EMail;
                        employee.OrgDepartmentId = data.OrgDepartmentId;
                        employee.DOL = data.DOL;
                        employee.RoleId = data.RoleId;
                    }
                }
            }
            else if (user == null)
            {
                var data = DB.Employees.Where(li => li.DomainId == Id).FirstOrDefault();
                if (data != null)
                {
                    employee.EmployeeNo = data.EmployeeNo;
                    employee.Name = data.Name;
                    employee.EMail = data.EMail;
                    employee.OrgDepartmentId = data.OrgDepartmentId;
                    if (data.OrgDepartmentId != null)
                        employee.OrgDepartmentName = DB.OrgDepartments.Where(li => li.OrgDepartmentId == data.OrgDepartmentId).FirstOrDefault().OrgDepartment1;
                    employee.DOL = data.DOL;
                    employee.RoleId = data.RoleId;
                }
                //else
                //{
                //    InValidUser();
                //}
            }
            //if (session != null)
            //{
            //    session["name"] = employee.Name;
            //    session["id"] = employee.EmployeeNo.ToString();
            //    string SessionID = session.SessionID;
            //}
            return employee;
        }
        public void InValidUser()
        {
            HttpResponse resp = HttpContext.Current.Response;
            resp.StatusCode = 401;
            resp.End();
        }
    }

}
