using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DCommon.Utility;

namespace DCommon.Security
{
    [Serializable]
    public class AuthorizedUser : IAuthorizedUser
    {
        public readonly static IAuthorizedUser AnonymousUser = new AuthorizedUser();

        public virtual object UserId { get; protected set; }

        public virtual string Username { get; protected set; }

        public virtual string Email { get; protected set; }

        public virtual string DisplayName { get; protected set; }

        public virtual string[] Roles { get; protected set; }

        public virtual DateTime LoginTime { get; protected set; }

        public AuthorizedUser(object userId, string username, string email = null,
            string displayName = null,
            IEnumerable<string> roles = null)
        {
            Guard.Against<ArgumentNullException>(string.IsNullOrWhiteSpace(username), "Username is required.");
            Guard.Against<ArgumentOutOfRangeException>(userId == null, "UserId is required");

            this.UserId = userId;
            this.Username = username;
            this.Email = email;
            this.DisplayName = displayName ?? string.Empty;
            this.LoginTime = DateTime.Now;
            this.Roles = roles == null ? new string[0] : roles.ToArray();
        }

        protected AuthorizedUser()
        {
            this.UserId = null;
            this.Email = this.DisplayName = this.Username = string.Empty;
            this.Roles = new string[0];
            this.LoginTime = DateTime.MinValue;
        }
    }
}
