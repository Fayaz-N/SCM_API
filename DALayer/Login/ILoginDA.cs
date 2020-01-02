using SCMModels;
using SCMModels.SCMModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALayer.Login
{
    public interface ILoginDA
    {
        EmployeeModel ValidateLoginCredentials(DynamicSearchResult Result);
    }
}
