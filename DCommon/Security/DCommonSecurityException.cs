using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace DCommon.Security
{
    public class DCommonSecurityException : Exception
    {
        public DCommonSecurityException(string message) : base(message) { }

        public DCommonSecurityException(string message, Exception innerException) : base(message, innerException) { }

        protected DCommonSecurityException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public string PermissionName { get; set; }
        public IAuthorizedUser User { get; set; }
    }
}
