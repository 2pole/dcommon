using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DCommon.Security
{
    public interface IAuthorizedUser
    {
        object UserId { get; }
        string Username { get; }
        string Email { get; }
        string DisplayName { get;  }
        DateTime LoginTime { get;  }

        string[] Roles { get; }
    }
}
