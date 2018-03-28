using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DCommon.Session
{
    public interface ISessionProvider
    {
        string SessionId { get; }

        bool TryGetItem<T>(string sessionKey, out T value);

        T GetItem<T>(string sessionKey);

        T GetItem<T>(string sessionKey, Func<T> lazyLoader);

        void SetItem(string sessionKey, object value);

        void RemoveItem(string sessionKey);

        void Clear();
    }
}
