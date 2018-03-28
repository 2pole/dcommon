using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using DCommon.Utility;

namespace DCommon.EF
{
    public class EFSessionLocator : IEFSessionLocator
    {
        /// <summary>
        ///   Internal implementation of the <see cref = "IEFSessionLocator" /> interface.
        /// </summary>
        private bool _disposed;
        private readonly DbContext _context;

        /// <summary>
        ///   Default Constructor.
        ///   Creates a new instance of the <see cref = "EFSession" /> class.
        /// </summary>
        /// <param name = "context"></param>
        public EFSessionLocator(DbContext context)
        {
            Guard.Against<ArgumentNullException>(context == null, "Expected a non-null DbContext instance.");
            _context = context;
        }

        /// <summary>
        ///   Gets the underlying <see cref = "DbContext" />
        /// </summary>
        public DbContext DbContext
        {
            get { return _context; }
        }

        /// <summary>
        ///   Gets the Connection used by the <see cref = "DbContext" />
        /// </summary>
        public IDbConnection Connection
        {
            get { return _context.Database.Connection; }
        }

        /// <summary>
        /// Creates an <see cref="DbQuery"/> of <typeparamref name="T"/> that can be used
        /// to query the entity.
        /// </summary>
        /// <typeparam name="T">The entityt type to query.</typeparam>
        /// <returns>A <see cref="ObjectQuery{T}"/> instance.</returns>
        public DbSet<T> GetDbSet<T>() where T : class
        {
            return _context.Set<T>();
        }

        public T Get<T>(object key) where T : class
        {
            return _context.Set<T>().Find(key);
        }

        /// <summary>
        /// Saves changes made to the object context to the database.
        /// </summary>
        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        /// <summary>
        ///   Adds a transient instance to the context associated with the session.
        /// </summary>
        /// <param name = "entity"></param>
        public void Add<T>(T entity) where T : class
        {
            GetDbSet<T>().Add(entity);
        }

        /// <summary>
        /// Deletes an entity instance from the context.
        /// </summary>
        /// <param name="entity"></param>
        public void Delete<T>(T entity) where T : class
        {
            var entry = DbContext.Entry(entity);
            entry.State = EntityState.Deleted;
        }

        /// <summary>
        /// Attaches an entity to the context. Changes to the entityt will be tracked by the underlying <see cref="DbContext"/>
        /// </summary>
        /// <param name="entity"></param>
        public void Attach<T>(T entity) where T : class
        {
            this.GetDbSet<T>().Attach(entity);
        }

        /// <summary>
        /// Detaches an entity from the context. Changes to the entity will not be tracked by the underlying <see cref="DbContext"/>.
        /// </summary>
        /// <param name="entity"></param>
        public void Detach<T>(T entity) where T : class
        {
            var entry = DbContext.Entry(entity);
            entry.State = EntityState.Detached;
        }

        public void Update<T>(T entity) where T : class
        {
            var entry = DbContext.Entry(entity);
            entry.State = EntityState.Modified; 
            //if (entry == null)
            //{
            //    var dbSet = this.CreateDbSet<T>();
            //    dbSet.Attach(entity);
            //    entry = Context.Entry(entity);
            //}
            //else if(entry.State == EntityState.Detached)
            //{
            //}
        }

        public void CreateOrUpdate<T>(T entity) where T : class
        {
            var entry = DbContext.Entry(entity);
            entry.State = DbContext.KeyValuesFor(entity).All(IsDefaultValue)
                        ? EntityState.Added
                        : EntityState.Modified;
        }

        #region Implementation of IDisposable

        /// <summary>
        ///   Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///   Disposes off the managed and un-managed resources used.
        /// </summary>
        /// <param name = "disposing"></param>
        protected void Dispose(bool disposing)
        {
            if (!disposing)
                return;
            if (_disposed)
                return;

            _context.Dispose();
            _disposed = true;
        }

        #endregion

        private static bool IsDefaultValue(object keyValue)
        {
            return keyValue == null
                   || (keyValue.GetType().IsValueType
                       && Equals(Activator.CreateInstance(keyValue.GetType()), keyValue));
        }
    }
}
