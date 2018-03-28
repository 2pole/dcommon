using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace DCommon.Mvc.Filters
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public abstract class InjectionFilterAttribute : ActionFilterAttribute
    {
        public string ParameterName { get; set; }

        protected abstract Type ParameterType { get; }

        public InjectionMode Mode { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var canbeInjected = CanbeInjected(filterContext);
            if (canbeInjected)
            {
                var injectedModel = GetInjectedModel(filterContext);
                switch (Mode)
                {
                    case InjectionMode.Type:
                        InjectByType(filterContext, injectedModel);
                        break;
                    case InjectionMode.ParameterName:
                        InjectByName(filterContext, injectedModel);
                        break;
                }
            }

            base.OnActionExecuting(filterContext);
        }

        protected virtual bool CanbeInjected(ActionExecutingContext filterContext)
        {
            return true;
        }

        protected abstract object GetInjectedModel(ActionExecutingContext filterContext);

        private void InjectByName(ActionExecutingContext filterContext, object injectedModel)
        {
            if (!string.IsNullOrEmpty(this.ParameterName))
            {
                if (filterContext.ActionParameters.ContainsKey(this.ParameterName))
                {
                    filterContext.ActionParameters[this.ParameterName] = injectedModel;
                }
            }
        }

        private void InjectByType(ActionExecutingContext filterContext, object injectedModel)
        {
            var actionParameters = filterContext.ActionDescriptor
                .GetParameters()
                .Where(d => d.ParameterType == ParameterType || d.ParameterType.IsAssignableFrom(ParameterType));

            foreach (var parameterDescriptor in actionParameters)
            {
                filterContext.ActionParameters[parameterDescriptor.ParameterName] = injectedModel;
            }
        }
    }
}
