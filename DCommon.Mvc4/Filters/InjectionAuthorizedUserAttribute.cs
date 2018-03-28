using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using CommonServiceLocator;
using DCommon.Mvc.Filters;
using DCommon.Security;

namespace DCommon.Mvc.Filters
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class InjectionAuthorizedUserAttribute : InjectionFilterAttribute
    {
        protected override Type ParameterType
        {
            get { return typeof(IAuthorizedUser); }
        }

        protected override object GetInjectedModel(ActionExecutingContext filterContext)
        {
            var authService = ServiceLocator.Current.GetInstance<IAuthenticationService>();
            var user = authService.GetAuthenticatedUser();
            return user;
        }
    }
}
