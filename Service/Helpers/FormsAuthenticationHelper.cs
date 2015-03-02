using System;
using System.Web.Security;
using System.Web;

namespace Service.Helpers
{

    public class FormsAuthenticationHelper
    {
        public static void SetAuthCookie(int userID, string role, TimeSpan timeOut, bool createPersistentCookie)
        {
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                1, // Ticket version
                userID.ToString(), // Username associated with ticket
                DateTime.Now, // Date/time issued
                DateTime.Now.Add(timeOut), // Date/time to expire
                createPersistentCookie, // "true" for a persistent user cookie
                role, // User-data, in this case the roles
                FormsAuthentication.FormsCookiePath);// Path cookie valid for

            // Encrypt the cookie using the machine key for secure transport
            string hash = FormsAuthentication.Encrypt(ticket);
            HttpCookie cookie = new HttpCookie(
                FormsAuthentication.FormsCookieName, // Name of auth cookie
                hash); // Hashed ticket

            cookie.Path = FormsAuthentication.FormsCookiePath;
            cookie.Secure = FormsAuthentication.RequireSSL;
            // Set the cookie's expiration time to the tickets expiration time
            if (ticket.IsPersistent) cookie.Expires = ticket.Expiration;

            // Add the cookie to the list for outgoing response
            HttpContext.Current.Response.Cookies.Add(cookie);


        }

        /// <summary>
        /// Redirect from Login page: timeOut = 30 minutes, defaultReturnUrl = ~/Default.aspx
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="roles"></param>
        /// <param name="createPersistentCookie"></param>
        public static void SetAuthCookie(int userID, string role, bool createPersistentCookie, TimeSpan timeOut)
        {
            SetAuthCookie(userID, role, timeOut, createPersistentCookie);
        }
    }
}
