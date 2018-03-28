using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using DCommon;
using DCommon.Utility;
using IsolationLevel = System.Data.IsolationLevel;

namespace DCommon.EF
{
    public class EFUnitOfWork : IEFUnitOfWork
    {
        private bool _disposed;
        private readonly IEFSessionResolver _resolver;
        private IDictionary<Guid, IEFSessionLocator> _openSessions = new Dictionary<Guid, IEFSessionLocator>();

        /// <summary>
        /// Initializes a new instance of the <see cref="DLinqUnitOfWork"/> class.
        /// </summary>
        /// <param name="dataContext">The data context.</param>
        public EFUnitOfWork(IEFSessionResolver resolver)
        {
            Guard.Against<ArgumentNullException>(resolver == null,
                                               "Expected a non-null EFUnitOfWorkSettings instance.");
            _resolver = resolver;
            
        }

        /// <summary>
        /// Gets a <see cref="IEFSession"/> that can be used to query and update the specified type.
        /// </summary>
        /// <typeparam name="T">The type for which an <see cref="IEFSession"/> should be returned.</typeparam>
        /// <returns>An <see cref="IEFSession"/> that can be used to query and update the specified type.</returns>
        public IEFSessionLocator GetSession<T>()
        {
            Guard.Against<ObjectDisposedException>(_disposed,
                                                   "The current EFUnitOfWork instance has been disposed. " +
                                                   "Cannot get sessions from a disposed UnitOfWork instance.");

            var sessionKey = _resolver.GetSessionKeyFor<T>();
            if (_openSessions.ContainsKey(sessionKey))
                return _openSessions[sessionKey];

            //Opening a new session...
            var context = _resolver.GetDbContextFor<T>();
            var session = new EFSessionLocator(context);
            _openSessions.Add(sessionKey, session);
            return session;
        }

        #region IUnitOfWork
        /// <summary>
        /// Flushes the changes made in the unit of work to the data store.
        /// </summary>
        public void Flush()
        {
            Guard.Against<ObjectDisposedException>(_disposed,
                                                  "The current EFUnitOfWork instance has been disposed. " +
                                                  "Cannot get sessions from a disposed UnitOfWork instance.");

            _openSessions.ForEach(session => session.Value.SaveChanges());
        }

        #endregion

        #region IDisposable
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
       
        /// <summary>
        /// Disposes off the managed and unmanaged resources used.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                if (_openSessions != null && _openSessions.Count > 0)
                {
                    _openSessions.ForEach(session => session.Value.Dispose());
                    _openSessions.Clear();
                }
            }
            _openSessions = null;
            _disposed = true;
        }

        #endregion
    }
}
