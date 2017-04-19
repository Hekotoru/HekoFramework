using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc
{
    /// <summary>
    /// This class is a representative class of a user in the application.
    /// </summary>
    public class User
    {
        /// <summary>
        /// This property represents the user name of user.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// This property represents the password of user.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// This property represents the secret word for user token.
        /// </summary>
        public string SecretWord { get; set; }

        /// <summary>
        /// This Field represent the lifetime of the session with the token.
        /// </summary>
        public double ExpirationTime;
    }
}
