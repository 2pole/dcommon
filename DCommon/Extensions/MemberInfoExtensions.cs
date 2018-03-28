using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace DCommon
{
    /// <summary>
    /// Defines extension methods for <see cref="MemberInfo"/>.
    /// </summary>
    public static class MemberInfoExtensions
    {
        /// <summary>
        /// Gets the attribute from the <see cref="MemberInfo"/>.
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static TAttribute GetAttributeOf<TAttribute>(this MemberInfo memberInfo)
        {
            //AssertUtils.ArgumentNotNull(memberInfo, "memberInfo");
            object[] attributes = memberInfo.GetCustomAttributes(typeof(TAttribute), true);
            if (attributes.Length == 0)
                return default(TAttribute);
            return (TAttribute)attributes[0];
        }
    }
}
