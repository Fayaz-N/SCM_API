using DALayer.Login;
using SCMModels;
using SCMModels.SCMModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BALayer
{
    public class LoginBA : ILoginBA
    {
        public readonly ILoginDA _mprLoginAccess;
        public LoginBA(ILoginDA LoginDA)
        {
            this._mprLoginAccess = LoginDA;
        }
        public EmployeeModel ValidateLoginCredentials(DynamicSearchResult Result)
        {
            return this._mprLoginAccess.ValidateLoginCredentials(Result);
        }
    }
}
