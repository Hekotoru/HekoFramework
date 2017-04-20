using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute
    {

        public bool IsAuthorized(Request request)
        {
            string token = request.Headers["auth-token"];
            string username = request.Headers["username"];
            string secret = request.Headers["secret-key"];
            return Session.GetUserAuthority(username, token, secret);
        }
    }
}
