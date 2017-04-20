using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JWT;
using Newtonsoft.Json;

namespace Mvc
{
    /// <summary>
    /// This class is responsible for handling the session with the user and the token.
    /// </summary>
    public class Session
    {



        /// <summary>
        /// This method evaluate a token if is correct according the user data.
        /// </summary>
        /// <param name="user">object: user</param>
        /// <param name="token">string: token</param>
        /// <returns>True: If is correct, false: isn't</returns>
        static public bool GetUserAuthority(string username, string token, string secret)
        {
            if (string.IsNullOrEmpty(username)
                || string.IsNullOrEmpty(token) || string.IsNullOrEmpty(secret))
            {
                Console.WriteLine("The token was not verified. Any field of user are empty.");
                return false;
            }

            string tokenDecoded = JsonWebToken.Decode(token, secret);

            User userGenerated = JsonConvert.DeserializeObject<User>(tokenDecoded);

            if (userGenerated.Username == username)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// This method verify id the time was expired.
        /// </summary>
        /// <param name="user">object: user</param>
        /// <param name="token">string: token</param>
        /// <returns>True: if was expired , false: Was not expired.</returns>
        static public bool ExpiredTime(string username, string token, string secret)
        {

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(token) || string.IsNullOrEmpty(secret))
            {
                Console.WriteLine("The lifetime of token was not evaluated. Maybe any parameter are empty.");
                return true;
            }

            string strJson = JsonWebToken.Decode(token, secret, false);

            User userGenerated = JsonConvert.DeserializeObject<User>(strJson);

            DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            double now = Math.Round((DateTime.Now - unixEpoch).TotalSeconds);

            if (userGenerated.ExpirationTime < now)
            {
                return true;
            }

            return false;
        }


        /// <summary>
        /// This Method generate a token code for 40 minutes to expire.
        /// </summary>
        /// <param name="user">Object: User</param>
        /// /// <param name="secretword">string: The secret Word.</param>
        /// <returns>string: Token.</returns>
        public static string GenerateToken(string username, string password, string secret)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)
                || string.IsNullOrEmpty(secret))
            {
                Console.WriteLine("The token was not generated. Any field of user are empty.");
                return null;
            }

            DateTime now = DateTime.Now.AddMinutes(40);
            DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            double secondToExpire = Math.Round((now - unixEpoch).TotalSeconds);

            var payload = new Dictionary<string, object>
            {
                { "Userame", username },
                { "Password", password},
                { "ExpirationTime", secondToExpire}
            };

            string token = JsonWebToken.Encode(payload, secret, JwtHashAlgorithm.HS256);

            return token;
        }
    }
}
