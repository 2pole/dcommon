using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DCommon.Security
{
    public interface IAuthenticationService
    {
        /// <summary>
        /// Validate user
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <returns>Result</returns>
        bool ValidateUser(string username, string password);

        /// <summary>
        /// User Login system
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <returns></returns>
        IResult<IAuthorizedUser> LogIn(string username, string password);

        /// <summary>
        /// Get authenticated user
        /// </summary>
        /// <returns></returns>
        IAuthorizedUser GetAuthenticatedUser();

        /// <summary>
        /// Current user login out system
        /// </summary>
        void LogOut();
    }
}
