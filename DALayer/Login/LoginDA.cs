using SCMModels;
using SCMModels.SCMModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.DirectoryServices.AccountManagement;


namespace DALayer.Login
{
    public class LoginDA: ILoginDA
    {
        YSCMEntities DB = new YSCMEntities();

        public bool ValidateLoginCredentials(DynamicSearchResult Result)
        {
            bool loginFlag = false;
            string[] UserCredentials = Result.columnValues.Split(',');
            string DomainId = UserCredentials[0].ToString(), Password = UserCredentials[1].ToString();
            PrincipalContext ctx = new PrincipalContext(ContextType.Domain);

            UserPrincipal user = UserPrincipal.FindByIdentity(ctx, DomainId.Trim());
            if (user != null)
            {
                if (ctx.ValidateCredentials(DomainId, Password))
                {
                    loginFlag = true;
                }
            }
            else if (user == null)
            {
                string tableName = Result.tableName;
                loginFlag = DB.Employees.Any(u => u.DomainId == DomainId && u.PWD == Password);
            }
            return loginFlag;
        }
    }
}
