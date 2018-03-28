using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DCommon.Security
{
    public interface IFormsAuthenticationService : IAuthenticationService
    {
        void SignIn(IAuthorizedUser user, bool createPersistentCookie);

        void SignOut();
    }

    public interface IFormsAuthenticationService<TUser> : IFormsAuthenticationService
        where TUser : IAuthorizedUser
    {
        void SignIn(TUser user, bool createPersistentCookie);

        /// <summary>
        /// User Login system
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <returns></returns>
        IResult<TUser> LogIn(string username, string password);
    }
}
