using SCMModels.SCMModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCMAPI
{
    public class UserMasterRepository : IDisposable
    {
        // SECURITY_DBEntities it is your context class       
        YSCMEntities context = new YSCMEntities();

        //This method is used to check and validate the user credentials
        public Employee ValidateUser(string username, string password)
        {
            return context.Employees.FirstOrDefault(user =>
            user.DomainId.Equals(username, StringComparison.OrdinalIgnoreCase)
            && user.PWD == password);
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}