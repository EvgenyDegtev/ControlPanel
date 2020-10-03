using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ControlPanel.Infastructure.Abstract;
using System.Web.Security;

namespace ControlPanel.Infastructure.Concrete
{
    public class FormsAuthProvider: IAuthProvider
    {
        public bool Authenticate(string username, string password)
        {
            bool result = FormsAuthentication.Authenticate(username, password);
            if (result)
            {
                FormsAuthentication.SetAuthCookie(username, false);
            }
            return result;
        }
    }
}