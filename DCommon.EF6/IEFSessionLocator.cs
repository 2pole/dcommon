using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;

namespace DCommon.EF
{
    public interface IEFSessionLocator: IDisposable
    {
        /// <summary>
        /// Gets the underlying <see cref="DbContext"/>
        /// </summary>
        DbContext DbContext { get; }

        /// <summary>
        /// Gets the Connection used by the <see cref="ObjectContext"/>
        /// </summary>
        IDbConnection Connection { get; }

        /// <summary>
        /// Creates an <see cref="GetDbSet"/> of <typeparamref name="T"/> that can be used
        /// to query the entity.
        /// </summary>
        /// <typeparam name="T">The entityt type to query.</typeparam>
        /// <returns>A <see cref="GetDbSet{T}"/> instance.</returns>
        DbSet<T> GetDbSet<T>() where T : class;

        /// <summary>
        /// Get the object by key from object context.
        /// <param name="key"></param>
        /// <returns>A <see cref="{T}"/> instance.</returns>
        /// </summary>
        T Get<T>(object key) where T : class;

        /// <summary>
        /// Adds a transient instance to the context associated with the session.
        /// </summary>
        /// <param name="entity"></param>
        void Add<T>(T entity) where T : class;

        /// <summary>
        /// Deletes an entity instance from the context.
        /// </summary>
        /// <param name="entity"></param>
        void Delete<T>(T entity) where T : class;

        /// <summary>
        /// Attaches an entity to the context. Changes to the entityt will be tracked by the underlying <see cref="ObjectContext"/>
        /// </summary>
        /// <param name="entity"></param>
        void Attach<T>(T entity) where T : class;

        /// <summary>
        /// Detaches an entity from the context. Changes to the entity will not be tracked by the underlying <see cref="ObjectContext"/>.
        /// </summary>
        /// <param name="entity"></param>
        void Detach<T>(T entity) where T : class;

        /// <summary>
        /// Refreshes an entity.
        /// </summary>
        /// <param name="entity"></param>
        void Update<T>(T entity) where T : class;

        void CreateOrUpdate<T>(T entity) where T : class;

        /// <summary>
        /// Saves changes made to the object context to the database.
        /// </summary>
        void SaveChanges();
    }
}
