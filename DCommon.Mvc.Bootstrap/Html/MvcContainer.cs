using System;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace DCommon.Mvc.Bootstrap.Html
{
    public class MvcContainer : IDisposable
    {
        private readonly ViewContext _viewContext;
        private readonly string _tagName = "div";

        public MvcContainer(ViewContext viewContext)
        {
            if (viewContext == null)
            {
                throw new ArgumentNullException("viewContext");
            }

            _viewContext = viewContext;
        }

        public MvcContainer(ViewContext viewContext, string tagName)
        {
            if (viewContext == null)
            {
                throw new ArgumentNullException("viewContext");
            }

            _viewContext = viewContext;
            _tagName = tagName;
        }

        public void EndContainer()
        {
            Dispose(true);
        }

        #region Implementation of IDisposable

        private bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _disposed = true;
                ContainerExtensions.EndContainer(_viewContext, _tagName);
            }
        }

        #endregion
    }
}
