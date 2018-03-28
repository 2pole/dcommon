using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using DCommon;
using DCommon;
using DCommon.Utility;

namespace DCommon.NHibernate
{
    /// <summary>
    /// Implements the <see cref="IUnitOfWork"/> interface to provide an implementation
    /// of a IUnitOfWork that uses NHibernate to query and update the underlying store.
    /// </summary>
    public class NHUnitOfWork : INHUnitOfWork
    {
        private bool _disposed;
        private readonly INHSessionResolver _sessionResolver;
        private readonly IDictionary<Guid, ISession> _openSessions = new Dictionary<Guid, ISession>();

        /// <summary>
        /// Default Constructor.
        /// Creates a new instance of the <see cref="NHUnitOfWork"/> that uses the provided
        /// NHibernate <see cref="ISession"/> instance.
        /// </summary>
        /// <param name="sessionResolver">An instance of <see cref="NHUnitOfWorkSettings"/>.</param>
        public NHUnitOfWork(INHSessionResolver sessionResolver)
        {
            Guard.Against<ArgumentNullException>(sessionResolver == null,
                                                 "Expected a non-null instance of NHUnitOfWorkSettings.");
            _sessionResolver = sessionResolver;
        }

        /// <summary>
        /// Gets a <see cref="ISession"/> instance that can be used for querying and updating
        /// instances of <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">The type for which a <see cref="ISession"/> is retrieved.</typeparam>
        /// <returns>An instance of <see cref="ISession"/> that can be used for querying and updating
        /// instances of <typeparamref name="T"/></returns>
        public ISession GetSession<T>()
        {
            Guard.Against<ObjectDisposedException>(_disposed,
                                                 "The current EFUnitOfWork instance has been disposed. " +
                                                 "Cannot get sessions from a disposed UnitOfWork instance.");

            var sessionKey = _sessionResolver.GetSessionKeyFor<T>();
            if (_openSessions.ContainsKey(sessionKey))
                return _openSessions[sessionKey];

            //Opening a new session...
            var session = _sessionResolver.OpenSessionFor<T>();
            _openSessions.Add(sessionKey, session);
            return session;
        }

        #region IUnitOfWork
        /// <summary>
        /// Flushes the changes made in the unit of work to the data store.
        /// </summary>
        public void Flush()
        {
            _openSessions.ForEach(session => session.Value.Flush());
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
        /// Disposes off managed resources used by the NHUnitOfWork instance.
        /// </summary>
        /// <param name="disposing"></param>
        private void Dispose(bool disposing)
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
            _disposed = true;
        }

        #endregion
    }
}
