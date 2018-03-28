using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DCommon.Security
{
    public static class AuthorizedUserExtensions
    {
        public static bool IsAnonymous(this IAuthorizedUser user)
        {
            return user == null || user.UserId == null;
        }

        public static bool IsInRole(this IAuthorizedUser user, string role)
        {
            return !user.IsAnonymous() && user.Roles != null && user.Roles.Contains(role, StringComparer.OrdinalIgnoreCase);
        }

        public static bool IsInRole(this IAuthorizedUser user, params string[] roles)
        {
            if (user.IsAnonymous() || user.Roles == null || roles == null || roles.Length == 0)
                return false;

            return roles.Any(x => user.Roles.Contains(x, StringComparer.OrdinalIgnoreCase));
        }
    }
}
