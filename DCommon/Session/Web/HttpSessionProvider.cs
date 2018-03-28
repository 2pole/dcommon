using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace DCommon.Session.Web
{
    public class HttpSessionProvider : ISessionProvider
    {
        private readonly HttpSessionStateBase _httpSessionState;
        public HttpSessionProvider(HttpContextBase httpContext)
        {
            _httpSessionState = httpContext.Session;
        }

        public string SessionId
        {
            get
            {
                return _httpSessionState.SessionID;
            }
        }

        public bool TryGetItem<T>(string sessionKey, out T value)
        {
            bool hasValue = false;
            value = default(T);
            if (string.IsNullOrEmpty(sessionKey))
            {
                throw new ArgumentNullException("sessionKey");
            }

            object sessionItem = _httpSessionState[sessionKey];
            if (sessionItem != null)
            {
                hasValue = true;
                value = (T)sessionItem;
            }
            return hasValue;
        }

        public T GetItem<T>(string sessionKey)
        {
            return GetItem<T>(sessionKey, default(T));
        }

        public T GetItem<T>(string sessionKey, T defaultValue)
        {
            T result;
            if (TryGetItem(sessionKey, out result))
            {
                return result;
            }
            else
            {
                return defaultValue;
            }
        }

        public T GetItem<T>(string sessionKey, Func<T> lazyLoader)
        {
            T result;
            if (!TryGetItem(sessionKey, out result))
            {
                result = lazyLoader();
                SetItem(sessionKey, result);
            }
            return result;
        }

        public void SetItem(string sessionKey, object value)
        {
            _httpSessionState[sessionKey] = value;
        }

        public void RemoveItem(string sessionKey)
        {
            _httpSessionState.Remove(sessionKey);
        }

        public void Clear()
        {
            _httpSessionState.Clear();
        }
    }
}
